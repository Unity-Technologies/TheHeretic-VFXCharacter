using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[ExecuteAlways]
public class SpawnTexturesRenderer : MonoBehaviour
{
    [Header("Input Sets")]
    public TargetSet[] targetSets;

    [Header("Output Options")]
    public TextureSize rtSize = TextureSize.x1024;

    //--------------------------------------------------

    public enum TextureSize { x256 = 256, x512 = 512, x1024 = 1024, x2048 = 2048 }
    
    [System.Serializable]
    public struct MeshBinding
    {
        public Renderer renderer;
        public Mesh sourceMesh;
        public Vector2Int subMeshRange;
        public Vector3Int objectIds;
    }

    [System.Serializable]
    public struct TargetSet
    {
        public VisualEffect[] visualEffects;
        public MeshBinding[] meshBindings;
        public Texture sharedAlbedo;
        public string rtBindingSuffix;
    }

    public void RefreshGeneratedData()
    {
        OnDisable();
        OnEnable();

        foreach (var targetSet in targetSets)
            foreach (var visualEffect in targetSet.visualEffects)
                visualEffect.Reinit();
    }
    
    //--------------------------------------------------
    
    static readonly int s_spObjectId = Shader.PropertyToID("_STRObjectID");
    
    static readonly int s_spSpawnCountProperty = Shader.PropertyToID("spawn_count");
    static readonly int s_spSpawnUVProperty = Shader.PropertyToID("spawn_uv");
    static readonly int s_spSpawnUVSizeProperty = Shader.PropertyToID("spawn_uv_size");

    static readonly string[] s_AttributeNames = { "position", "normals", "tangents", "vertex_color", "object_id", "albedo" };
    static readonly GraphicsFormat[] s_AttributesFormats = { GraphicsFormat.R32G32B32A32_SFloat, GraphicsFormat.R16G16B16A16_SFloat, GraphicsFormat.R16G16B16A16_SFloat, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.R32_SFloat };
    static readonly int s_AttributesCountOutput = s_AttributeNames.Length;
    static readonly int s_AttributesCountWriteable = s_AttributesCountOutput - 1;

    enum Attributes { Position, Normals, Tangents, Colors, Id, Albedo, Count}
    enum ShaderPass { Static = 0, Transform = 1 }
    
    struct TargetState
    {
        public Texture[] BindTextures;
        public RenderTexture[] RenderTextures;
        public RenderTargetIdentifier[] RTIsStatic;
        public RenderTargetIdentifier[] RTIsTransform;

        public struct InstanceState
        {
            public int ParticleCount;
            public RenderTexture SpawnUV;
        }

        public InstanceState[] InstanceStates;
    }

    // Propagated from default meta assignment
    [HideInInspector][SerializeField] Shader shader = null; 

    CommandBuffer m_Commands;
    Material m_Material;
    TargetState[] m_TargetStates;

    void OnEnable()
    {
        m_Commands = new CommandBuffer {name = "SpawnTexturesRenderer"};
        m_Material = new Material(shader) {name = "SpawnTexturesRenderer", hideFlags = HideFlags.DontSave};
    
        CreateTargetStates();
        InitializeOnce();
        
        RenderPipelineManager.beginFrameRendering += BeginFrameRendering;
    }

    // private bool once;
    // void Update()
    // {
    //     if (!once)
    //     {
    //         Debug.Log($"{Time.frameCount}: Update");
    //         once = true;
    //         RefreshGeneratedData();
    //     }
    // }
    
    void OnDisable()
    {
        RenderPipelineManager.beginFrameRendering -= BeginFrameRendering;

        ReleaseTargetStates();

        SafeDestroy(m_Material);
        m_Commands.Dispose();
    }
    
    void SafeDestroy(Object o) { if (Application.isPlaying) Destroy(o); else DestroyImmediate(o); }

    void InitializeOnce()
    {
        ScheduleTargetSets(true);
        Graphics.ExecuteCommandBuffer(m_Commands);
        m_Commands.Clear();

        for (var i = 0; i < targetSets.Length; ++i)
        {
            ref var targetState = ref m_TargetStates[i];
            
            targetState.InstanceStates = new TargetState.InstanceState[targetSets[i].visualEffects.Length];
            for (var j = 0; j < targetSets[i].visualEffects.Length; ++j)
            {
                var visualEffect = targetSets[i].visualEffects[j];
                
                if (visualEffect.TryGetComponent<SpawnCompactor>(out var spawnCompactor))
                {
                    var objectIds = targetState.BindTextures[(int) Attributes.Id];
                    spawnCompactor.Generate(objectIds, out var particleCount, out var compactRt);
                    
                    targetState.InstanceStates[j] = new TargetState.InstanceState
                    {
                        ParticleCount = particleCount,
                        SpawnUV = compactRt
                    };
                }
            }

            AssignOutputs(targetSets[i], targetState);
            
#if UNITY_EDITOR
            foreach (var visualEffect in targetSets[i].visualEffects)
                PrefabUtility.RecordPrefabInstancePropertyModifications(visualEffect);
#endif
        }
    }
    
    void CreateTargetStates()
    {
        m_TargetStates = new TargetState[targetSets.Length];
        for (var i = 0; i < targetSets.Length; ++i)
            m_TargetStates[i] = CreateTargetState(targetSets[i]);
    }

    void AssignOutputs(in TargetSet targetSet, in TargetState targetState)
    {
        for (var i = 0; i < targetSet.visualEffects.Length; ++i)
        {
            var visualEffect = targetSet.visualEffects[i];
            
            for (var j = 0; j < s_AttributesCountOutput; ++j)
            {
                if (targetState.BindTextures[j] != null)
                {
                    var attribName = s_AttributeNames[j] + targetSet.rtBindingSuffix;
                    if (visualEffect.HasTexture(attribName))
                        visualEffect.SetTexture(attribName, targetState.BindTextures[j]);
                }
            }

            var instanceState = targetState.InstanceStates[i];
            if (instanceState.SpawnUV != null)
            {
                if(visualEffect.HasUInt(s_spSpawnCountProperty))
                    visualEffect.SetUInt(s_spSpawnCountProperty, (uint)instanceState.ParticleCount);
                if(visualEffect.HasTexture(s_spSpawnUVProperty))
                    visualEffect.SetTexture(s_spSpawnUVProperty, instanceState.SpawnUV);
                if(visualEffect.HasUInt(s_spSpawnUVSizeProperty))
                    visualEffect.SetUInt(s_spSpawnUVSizeProperty, (uint)instanceState.SpawnUV.width);
            }
        }
    }

    void ReleaseTargetStates()
    {
        foreach (var targetState in m_TargetStates)
            ReleaseTargetState(targetState);
        
        m_TargetStates = null;
    }
    
    TargetState CreateTargetState(in TargetSet targetSet)
    {
        var rtCount = s_AttributesCountWriteable;
        var rts = new RenderTexture[s_AttributesCountOutput];
        var rtisEverything = new RenderTargetIdentifier[rtCount];
        var textures = new Texture[s_AttributesCountOutput];
        
        for(var i = 0; i < rtCount; ++i)
        {
            textures[i] = rts[i] = new RenderTexture((int)rtSize, (int)rtSize, 0, s_AttributesFormats[i]) {
                hideFlags = HideFlags.DontSave, filterMode = FilterMode.Point , name = s_AttributeNames[i] + targetSet.rtBindingSuffix
            };
            rtisEverything[i] = new RenderTargetIdentifier(rts[i]);
        }

        textures[(int) Attributes.Albedo] = targetSet.sharedAlbedo;
        var rtisStatic =  new[] {rtisEverything[(int)Attributes.Colors], rtisEverything[(int)Attributes.Id]};
        var rtisTransform = new[] {rtisEverything[(int)Attributes.Position], rtisEverything[(int)Attributes.Normals], rtisEverything[(int)Attributes.Tangents]};
        
        return new TargetState {BindTextures = textures, RenderTextures = rts, RTIsStatic = rtisStatic, RTIsTransform = rtisTransform};
    }

    void ReleaseTargetState(TargetState targetState)
    {
        foreach (var rt in targetState.RenderTextures)
            SafeDestroy(rt);
    }

    void BeginFrameRendering(ScriptableRenderContext context, Camera[] _)
    {
        ScheduleTargetSets();

        context.ExecuteCommandBuffer(m_Commands);
        context.Submit();
        
        m_Commands.Clear();
    }

    void ScheduleTargetSets(bool updateStatic = false)
    {
        for (int i = 0, n = targetSets.Length; i < n; ++i)
        {
            if(updateStatic)
                ScheduleTargetSetStatic(targetSets[i], m_TargetStates[i], m_Material, m_Commands);
                
            ScheduleTargetSet(updateStatic, targetSets[i], m_TargetStates[i], m_Material, m_Commands);
        }
    }

    static void ScheduleTargetSet(bool updateStatic, in TargetSet targetSet, in TargetState targetState, Material mat, CommandBuffer cmd)
    {
        cmd.SetRenderTarget(targetState.RTIsTransform, targetState.RTIsTransform[0]);

        if (updateStatic)
            cmd.ClearRenderTarget(false, true, Color.clear);

        foreach (var meshBinding in targetSet.meshBindings)
            for (int i = meshBinding.subMeshRange.x, n = meshBinding.subMeshRange.y; i < n; ++i)
                cmd.DrawRenderer(meshBinding.renderer, mat, i, (int)ShaderPass.Transform);
    }

    static void ScheduleTargetSetStatic(in TargetSet targetSet, in TargetState targetState, Material mat, CommandBuffer cmd)
    {
        cmd.SetRenderTarget(targetState.RTIsStatic, targetState.RTIsStatic[0]);
        cmd.ClearRenderTarget(false, true, Color.clear);

        foreach (var meshBinding in targetSet.meshBindings)
        {
            for (int i = meshBinding.subMeshRange.x, n = meshBinding.subMeshRange.y; i < n; ++i)
            {
                cmd.SetGlobalInt(s_spObjectId, meshBinding.objectIds[i]);
                
                if (meshBinding.sourceMesh != null)
                    cmd.DrawMesh(meshBinding.sourceMesh, Matrix4x4.identity, mat, i, (int)ShaderPass.Static);
                else
                    cmd.DrawRenderer(meshBinding.renderer, mat, i, (int)ShaderPass.Static);
            }
        }
    }
}
