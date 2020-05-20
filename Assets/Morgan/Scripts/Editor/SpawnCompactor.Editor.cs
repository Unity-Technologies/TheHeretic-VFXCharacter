using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnCompactor))]
public class SpawnCompactorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        using (var ccs = new EditorGUI.ChangeCheckScope())
        {
            DrawDefaultInspector();

            if (ccs.changed)
            {
                var str = FindObjectOfType<SpawnTexturesRenderer>();
                if(str != null)
                    str.RefreshGeneratedData();
            }
        }

        var compactor = (SpawnCompactor)target;
        EditorGUILayout.HelpBox("Particle count: " + compactor.ParticleCount + "\n\nThis many particles will be spawned. " +
                                "UV-space mesh is rendered as a mask into a texture of size as above. UVs of touched pixels are written into a tightly packed texture, and each one will spawn a particle." +
                                "The texture and the spawn count are automatically set on the VisualEffect." , MessageType.Info);
    }
}
