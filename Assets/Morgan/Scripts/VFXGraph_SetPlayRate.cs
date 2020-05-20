using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways]
[ExecuteInEditMode]
public class VFXGraph_SetPlayRate : MonoBehaviour
{
    [SerializeField]
    public float playRate = 1;

    [SerializeField]
    public bool reinitOnEnabled = true;

    [SerializeField]
    public bool limitFPS = false;

    [SerializeField]
    public int fps = 30;

    public VisualEffect vf;

    // Start is called before the first frame update
    void Start()
    {
        //vf = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled) vf.playRate = playRate;
    }

    void Awake()
    {
#if UNITY_EDITOR

        int originalvSyncCount = QualitySettings.vSyncCount;
        int originalTargetFrameRate = Application.targetFrameRate;

        if (limitFPS)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fps;
        }
        else
        {
            QualitySettings.vSyncCount = originalvSyncCount;
            Application.targetFrameRate = originalTargetFrameRate;
        }
#endif
    }

    void OnEnable()
    {
        if (reinitOnEnabled) vf.Reinit();
    }
}
