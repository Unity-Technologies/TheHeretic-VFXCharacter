using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple scripts that "negates" the rotation of target object - used for Morgan as in order to fix the VFX Graph SDF issues that occur
//when Body_Null is offseted and the prefab is rotated.
//A proper fix inside the VFX Graph is desirable but after trying for a bit I couldn't find any.


[ExecuteInEditMode]
public class NegateRotation : MonoBehaviour
{
    public Transform rotationReference;

    void Update()
    {
        if (rotationReference) transform.localEulerAngles = new Vector3(transform.eulerAngles.x, -rotationReference.localRotation.eulerAngles.y, transform.eulerAngles.z);
        //else Debug.LogWarning("Rotation Reference Not Assigned!");
    }
}
