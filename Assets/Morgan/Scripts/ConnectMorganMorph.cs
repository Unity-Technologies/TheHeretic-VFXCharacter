using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class ConnectMorganMorph : MonoBehaviour
{
    public bool useBlendValue;
    public float blendValue;
    public GameObject[] targetShapes;

    int blendShapeCount;
    GameObject sourceObject;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < targetShapes.Length; i++)
        {
            if (targetShapes[i] != null)
            {
                skinnedMeshRenderer = targetShapes[i].GetComponent<SkinnedMeshRenderer>();
                skinnedMesh = targetShapes[i].GetComponent<SkinnedMeshRenderer>().sharedMesh;
                blendShapeCount = skinnedMesh.blendShapeCount;
                for (int k = 0; k < blendShapeCount; k++)
                {
                    if (useBlendValue == true)
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(k, blendValue);
                    }
                    else
                    {
                        skinnedMeshRenderer.SetBlendShapeWeight(k, -transform.localPosition.x * 100f);
                    }
                }
            }
        }
    }
}