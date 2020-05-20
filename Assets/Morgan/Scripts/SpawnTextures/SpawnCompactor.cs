using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class SpawnCompactor : MonoBehaviour
{
    public int ParticleCount { get; private set; }
    
    public Mesh m_Mesh;
    public bool[] m_SubMeshes = new bool[0];
    public int m_ObjectID = -1;

    [Space(10)]
    
    [Range(8, 1024)]
    [Tooltip("Size of a square texture the mesh will be rendered to as UVs")]
    public int m_TextureSize = 32;

    // -----

    const int k_GroupThreadCountSqrt = 8;

    [HideInInspector][SerializeField] ComputeShader m_Compute = null;
    
    static class Uniforms
    {
        internal static int _ObjectID = Shader.PropertyToID("_ObjectID");
        internal static int _ObjectIDs = Shader.PropertyToID("_ObjectIDs");
        internal static int _ObjectIDsSize = Shader.PropertyToID("_ObjectIDsSize");
        internal static int _MaskingSize = Shader.PropertyToID("_MaskingSize");
        internal static int _AppendBuffer = Shader.PropertyToID("_AppendBuffer");
        internal static int _CompactUVBuffer = Shader.PropertyToID("_CompactUVBuffer");
        internal static int _CompactUVTexture = Shader.PropertyToID("_CompactUVTexture");
        internal static int _CompactUVTextureSize = Shader.PropertyToID("_CompactUVTextureSize");
    }

    static class Kernels
    {
        internal static string AppendId = "appendId";
        internal static string Copy = "copy";
    }

    void OnValidate()
    {
        m_TextureSize = (m_TextureSize / k_GroupThreadCountSqrt) * k_GroupThreadCountSqrt;
        ValidateSubmeshArray();
    }
    
    public void Generate(Texture objectIds, out int particleCount, out RenderTexture compactRT)
    {
        ParticleCount = particleCount = 0;
        compactRT = null;

        if (m_Mesh == null)
        {
            Debug.LogWarning($"No mesh assigned ({name})");
            return;
        }

        if (objectIds == null || m_ObjectID < 0)
        {
            Debug.LogWarning($"ObjectID not properly configured ({name})");
            return;
        }

        using(var appendBuffer = new ComputeBuffer(m_TextureSize * m_TextureSize, sizeof(float) * 2, ComputeBufferType.Append))
        {
            var appendKernel = m_Compute.FindKernel(Kernels.AppendId);
            
            appendBuffer.SetCounterValue(0);
            
            m_Compute.SetInt(Uniforms._ObjectID, m_ObjectID);
            m_Compute.SetFloat(Uniforms._ObjectIDsSize, objectIds.width);
            m_Compute.SetTexture(appendKernel, Uniforms._ObjectIDs, objectIds);
            m_Compute.SetFloat(Uniforms._MaskingSize, 1.0f / m_TextureSize);
            m_Compute.SetBuffer(appendKernel, Uniforms._AppendBuffer, appendBuffer);
            m_Compute.Dispatch(appendKernel, m_TextureSize / k_GroupThreadCountSqrt, m_TextureSize / k_GroupThreadCountSqrt, 1);

            using(var counterCopyBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw))
            {
                ComputeBuffer.CopyCount(appendBuffer, counterCopyBuffer, 0);
                var counterCopyArray = new int[1];
                counterCopyBuffer.GetData(counterCopyArray);
                ParticleCount = particleCount = counterCopyArray[0];
            }

            if (ParticleCount == 0)
            {
                Debug.LogWarning($"No valid particles read back. ({name})");
            }
            else
            {
                var compactUVTextureSize = Mathf.CeilToInt(Mathf.Sqrt(ParticleCount));
        
                compactRT = new RenderTexture(compactUVTextureSize, compactUVTextureSize, 0, RenderTextureFormat.RGHalf) {
                    hideFlags = HideFlags.DontSave, enableRandomWrite = true, filterMode = FilterMode.Point, name = $"CompactUV({name})"
                };
                compactRT.Create();

                int copyKernel = m_Compute.FindKernel(Kernels.Copy);
                m_Compute.SetBuffer(copyKernel, Uniforms._CompactUVBuffer, appendBuffer);
                m_Compute.SetTexture(copyKernel, Uniforms._CompactUVTexture, compactRT);
                m_Compute.SetInt(Uniforms._CompactUVTextureSize, compactUVTextureSize);
                
                m_Compute.Dispatch(copyKernel, compactUVTextureSize, compactUVTextureSize, 1); // TODO: increase group size to 8,8,1, but early out threads with id above packedPixelCount
            }
        }
    }

    void ValidateSubmeshArray()
    {
        if (m_Mesh == null)
        {
            m_SubMeshes = new bool[0];
            return;
        }

        if (m_SubMeshes != null && m_SubMeshes.Length == m_Mesh.subMeshCount)
            return;
        
        m_SubMeshes = new bool[m_Mesh.subMeshCount];
        for (int i = 0; i < m_SubMeshes.Length; i++)
            m_SubMeshes[i] = true;
    }
}
