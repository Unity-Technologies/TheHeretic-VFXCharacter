using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways]
public class Morgan : MonoBehaviour
{
    public bool skipVFXUpdates = false;
    public Renderer bodyRenderer, clothRenderer, eyeRightRenderer, eyeLeftRenderer;
    public SkinnedMeshRenderer bodySkinnedMesh, helmetSkinnedMesh, helmetMaleSkinnedMesh, hairSkinnedMesh;
    public Material debugMatBody, debugMatHair, debugMatHelmet, debugMatHelmetMale, debugMatEyes, debugMatCloth, debugMatVertexColors, vfxMatBody, vfxMatFace, vfxMatHair, vfxHelmetFemale, vfxMatHelmetTop, vfxHelmetMaleA, vfxHelmetMaleB, vfxMatCloth, vfxMatEyes;

    public VisualEffect[] morganVFXGraphs;
    public ConnectMorganMorph connectMorganMorph;
    public GameObject eyeRight, eyeLeft;
    public Light lightEyeRight, lightEyeLeft;
    public GameObject blendShapes_Active;

    void OnEnable()
    {
        VfSetFloat("scale", 30); //hardcoded for Morgan standalone package
    }

    #region Morph Variables
    //Morphing
    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morph;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morphBody;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morph_aggression;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morph_mask_influence;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morphNoiseAInfluence;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morphNoiseBInfluence;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float face_morph_noise;

    [HideInInspector, SerializeField]
    [Range(0, 2)] public float face_morph_noise_speed;

    [HideInInspector, SerializeField]
    [Range(0, 5)] public float face_morph_noise_frequency;

    [HideInInspector, SerializeField]
    [Range(0, 2)] public int morph_emissive_mode;

    [HideInInspector, SerializeField]
    public float morph_emissive_intensity;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morph_emissive_width_min;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morph_emissive_width_max;

    [HideInInspector, SerializeField]
    public Gradient morph_emissive_gradient;

    [HideInInspector, SerializeField]
    public bool morph_emissive_flicker = true;

    [HideInInspector, SerializeField]
    public bool morphDrivenByBlendShape = true;

    [HideInInspector, SerializeField]
    public bool reverse_morph = true;

    //Morphing debris
    [HideInInspector, SerializeField]
    public AnimationCurve body_morph_interval;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float body_morph_debris_size_min = 3;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float body_morph_debris_size_max = 4;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_morph_debris_balance = 0.5f;

    [HideInInspector, SerializeField]
    public Gradient body_morph_debris_emissive_gradient;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float body_morph_debris_emissive_intensity = 1;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_morph_debris_emissive_velocity_contribution = 1;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_morph_debris_emissive_velocity_min;

    [HideInInspector, SerializeField]
    [Range(0, 200)] public float body_morph_debris_emissive_velocity_max = 50;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_morph_debris_rate = 0;

    private Vector3 eyeLeftLocationCalm = new Vector3(-0.03358399f, 0.01745684f, 0.07195219f);
    private Vector3 eyeLeftLocationAngry = new Vector3(-0.0388f, 0.0241f, 0.072f);
    private Vector3 eyeRightLocationCalm = new Vector3(0.03358097f, 0.01795983f, 0.07201382f);
    private Vector3 eyeRightLocationAngry = new Vector3(0.0413f, 0.022f, 0.072f);
    #endregion Morph Variables
    
    private void Morph()
    {
        connectMorganMorph.useBlendValue = !morphDrivenByBlendShape;

        if (!morphDrivenByBlendShape)
        {
            if (connectMorganMorph) connectMorganMorph.blendValue = morphBody * 100;

            if (eyeLeft && eyeRight)
            {
                eyeLeft.transform.localPosition = Vector3.Lerp(eyeLeftLocationCalm, eyeLeftLocationAngry, morphBody);
                eyeRight.transform.localPosition = Vector3.Lerp(eyeRightLocationCalm, eyeRightLocationAngry, morphBody);
            }
        }

        VfSetFloat("morph", morph);
        VfSetFloat("morph_aggression", morph_aggression);

        VfSetFloat("face_morph_noise", face_morph_noise);
        VfSetFloat("face_morph_noise_speed", face_morph_noise_speed);
        VfSetFloat("face_morph_noise_frequency", face_morph_noise_frequency);

        VfSetFloat("face_mask", 1);
        VfSetBool("reverse_morph", reverse_morph);

        VfSetInt("morph_emissive_mode", morph_emissive_mode);
        VfSetFloat("morph_emissive_intensity", morph_emissive_intensity);
        VfSetFloat("morph_emissive_width_min", morph_emissive_width_min);
        VfSetFloat("morph_emissive_width_max", morph_emissive_width_max);
        VfSetGradient("morph_emissive_gradient", morph_emissive_gradient);
        VfSetBool("morph_emissive_flicker", morph_emissive_flicker);

        //Morphing debris
        VfSetCurve("body_morph_interval", body_morph_interval);
        VfSetFloat("body_morph_debris_size_min", body_morph_debris_size_min);
        VfSetFloat("body_morph_debris_size_max", body_morph_debris_size_max);
        VfSetFloat("body_morph_debris_balance", body_morph_debris_balance);
        VfSetGradient("body_morph_debris_emissive_gradient", body_morph_debris_emissive_gradient);
        VfSetFloat("body_morph_debris_emissive_intensity", body_morph_debris_emissive_intensity);
        VfSetFloat("body_morph_debris_emissive_velocity_contribution", body_morph_debris_emissive_velocity_contribution);
        VfSetFloat("body_morph_debris_emissive_velocity_min", body_morph_debris_emissive_velocity_min);
        VfSetFloat("body_morph_debris_emissive_velocity_max", body_morph_debris_emissive_velocity_max);
        VfSetFloat("body_morph_debris_rate", body_morph_debris_rate);
    }

    #region Appearance Settings
    #region Morphing Peel Variables
    [HideInInspector, SerializeField]
    [Range(0, 1)] public float morph_peel_intensity = 0;

    [HideInInspector, SerializeField]
    public float morph_peel_frequency = 10;

    [HideInInspector, SerializeField]
    [Range(1, 5)] public int morph_peel_octaves = 3;

    [HideInInspector, SerializeField]
    public float morph_peel_power = 0.95f;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float morph_peel_new_range_min = -0.1f;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float morph_peel_new_range_max = 0.115f;

    [HideInInspector, SerializeField]
    [Range(0.0f, 5.0f)] public float morph_peel_roughness = 1;

    [HideInInspector, SerializeField]
    public float morph_peel_multiply = 2;

    [HideInInspector, SerializeField]
    public float morph_peel_period = 2000;
    #endregion
    private void SetMorphingPeelVariables()
    {
        //Morph Peel
        VfSetFloat("morph_peel_intensity", morph_peel_intensity);
        VfSetFloat("morph_peel_frequency", morph_peel_frequency);
        VfSetInt("morph_peel_octaves", morph_peel_octaves);
        VfSetFloat("morph_peel_roughness", morph_peel_roughness);
        VfSetFloat("morph_peel_new_range_min", morph_peel_new_range_min);
        VfSetFloat("morph_peel_new_range_max", morph_peel_new_range_max);
        VfSetFloat("morph_peel_power", morph_peel_power);
        VfSetFloat("morph_peel_multiply", morph_peel_multiply);
        VfSetFloat("morph_peel_period", morph_peel_period);
    }

    #region Pullback Variables
    [HideInInspector, SerializeField]
    [Range(0, 500)] public float pullback_speed_multiplier;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_mask_influence;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float pullback_distance_old_min;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float pullback_distance_old_max;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float pullback_distance_new_min;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float pullback_distance_new_max;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_a_min;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_a_max;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_b_min;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_b_max;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float pullback_noise_mask_speed;

    [HideInInspector, SerializeField]
    [Range(0, 50)] public float pullback_noise_mask_frequency;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_mask_range_min;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float pullback_noise_mask_range_max;
    #endregion Pullback Variables
    private void SetPullbackVariables ()
    {
        VfSetFloat("pullback_speed_multiplier", pullback_speed_multiplier);
        VfSetFloat("pullback_distance_old_min", pullback_distance_old_min);
        VfSetFloat("pullback_distance_old_max", pullback_distance_old_max);
        VfSetFloat("pullback_distance_new_min", pullback_distance_new_min);
        VfSetFloat("pullback_distance_new_max", pullback_distance_new_max);
        VfSetFloat("pullback_noise_mask_influence", pullback_noise_mask_influence);
        VfSetFloat("pullback_noise_a_min", pullback_noise_a_min);
        VfSetFloat("pullback_noise_a_max", pullback_noise_a_max);
        VfSetFloat("pullback_noise_b_min", pullback_noise_b_min);
        VfSetFloat("pullback_noise_b_max", pullback_noise_b_max);
        VfSetFloat("pullback_noise_mask_speed", pullback_noise_mask_speed);
        VfSetFloat("pullback_noise_mask_frequency", pullback_noise_mask_frequency);
        VfSetFloat("pullback_noise_mask_range_min", pullback_noise_mask_range_min);
        VfSetFloat("pullback_noise_mask_range_max", pullback_noise_mask_range_max);
    }

    #region Helmet Calm Variables
    //Helmet Calm
    [HideInInspector, SerializeField]
    public Mesh helmet_mesh;

    [HideInInspector, SerializeField]
    public float helmet_size_multiplier;

    [HideInInspector, SerializeField]
    public Color helmet_base_color;

    [HideInInspector, SerializeField]
    public Texture2D helmet_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_normal_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_emissive_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_metallic;

    //Customize Look: Helmet Detail
    [HideInInspector, SerializeField]
    public Mesh helmet_detail_mesh;

    [HideInInspector, SerializeField]
    public float helmet_detail_size_multiplier;

    [HideInInspector, SerializeField]
    public Color helmet_detail_base_color;

    [HideInInspector, SerializeField]
    public Texture2D helmet_detail_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_detail_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_detail_normal_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_detail_emissive_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_detail_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_detail_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_detail_metallic;

    //Helmet Rotation General
    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_rotation_x_shape_a = 33.0f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_rotation_x_shape_b = -180.0f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_rotation_y_shape_a = 180f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_rotation_y_shape_b = -4.0f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_rotation_z_shape_a = 28.18f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_rotation_z_shape_b = -49.0f;

    //Helmet rotation top
    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_top_rotation_x_shape_a = -33.4f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_top_rotation_x_shape_b = -22.8f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_top_rotation_y_shape_a = 148.0f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_top_rotation_y_shape_b = 159.0f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_top_rotation_z_shape_a = 28.18f;

    [HideInInspector, SerializeField]
    [Range(-180.0f, 180.0f)] public float helmet_top_rotation_z_shape_b = -20.0f;
    #endregion Helmet Calm Variables
    private void SetHelmetCalmVariables()
    {
        //Helmet Calm
        VfSetMesh("helmet_mesh", helmet_mesh);
        VfSetFloat("helmet_size_multiplier", helmet_size_multiplier);
        VfSetV4("helmet_base_color", helmet_base_color);
        VfSetTexture("helmet_base_color_map", helmet_base_color_map);
        VfSetTexture("helmet_mask_map", helmet_mask_map);
        VfSetTexture("helmet_normal_map", helmet_normal_map);
        VfSetTexture("helmet_emissive_map", helmet_emissive_map);
        VfSetFloat("helmet_normal_scale", helmet_normal_scale);
        VfSetFloat("helmet_smoothness", helmet_smoothness);
        VfSetFloat("helmet_metallic", helmet_metallic);

        //Helmet Calm Details
        VfSetMesh("helmet_detail_mesh", helmet_detail_mesh);
        VfSetFloat("helmet_detail_size_multiplier", helmet_detail_size_multiplier);
        VfSetV4("helmet_detail_base_color", helmet_detail_base_color);
        VfSetTexture("helmet_detail_base_color_map", helmet_detail_base_color_map);
        VfSetTexture("helmet_detail_mask_map", helmet_detail_mask_map);
        VfSetTexture("helmet_detail_normal_map", helmet_detail_normal_map);
        VfSetTexture("helmet_detail_emissive_map", helmet_detail_emissive_map);
        VfSetFloat("helmet_detail_normal_scale", helmet_detail_normal_scale);
        VfSetFloat("helmet_detail_smoothness", helmet_detail_smoothness);
        VfSetFloat("helmet_detail_metallic", helmet_detail_metallic);

        //Helmet Rotation General
        VfSetV2("helmet_rotation_vertical_x_ab", new Vector2(helmet_rotation_x_shape_a, helmet_rotation_x_shape_b));
        VfSetV2("helmet_rotation_vertical_y_ab", new Vector2(helmet_rotation_y_shape_a, helmet_rotation_y_shape_b));
        VfSetV2("helmet_rotation_vertical_z_ab", new Vector2(helmet_rotation_z_shape_a, helmet_rotation_z_shape_b));

        //Helmet Rotation Top
        VfSetV2("helmet_rotation_top_x_ab_vc_red", new Vector2(helmet_top_rotation_x_shape_a, helmet_top_rotation_x_shape_b));
        VfSetV2("helmet_rotation_top_y_ab_vc_red", new Vector2(helmet_top_rotation_y_shape_a, helmet_top_rotation_y_shape_b));
        VfSetV2("helmet_rotation_top_z_ab_vc_red", new Vector2(helmet_top_rotation_z_shape_a, helmet_top_rotation_z_shape_b));
    }

    #region Helmet Angry Variables
    [HideInInspector, SerializeField]
    public Mesh helmet_angry_mesh;

    [HideInInspector, SerializeField]
    public float helmet_angry_size_multiplier;

    [HideInInspector, SerializeField]
    public Color helmet_angry_base_color;

    [HideInInspector, SerializeField]
    public Texture2D helmet_angry_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_angry_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_angry_normal_map;

    [HideInInspector, SerializeField]
    public Texture2D helmet_angry_emissive_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_angry_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_angry_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float helmet_angry_metallic;
    #endregion Helmet Angry Variables
    private void SetHelmetAngryVariables()
    {
        //Helmet Angry
        VfSetMesh("helmet_angry_mesh", helmet_angry_mesh);
        VfSetFloat("helmet_angry_size_multiplier", helmet_angry_size_multiplier);
        VfSetV4("helmet_angry_base_color", helmet_angry_base_color);
        VfSetTexture("helmet_angry_base_color_map", helmet_angry_base_color_map);
        VfSetTexture("helmet_angry_mask_map", helmet_angry_mask_map);
        VfSetTexture("helmet_angry_normal_map", helmet_angry_normal_map);
        VfSetFloat("helmet_angry_normal_scale", helmet_angry_normal_scale);
        VfSetFloat("helmet_angry_smoothness", helmet_angry_smoothness);
        VfSetFloat("helmet_angry_metallic", helmet_angry_metallic);
    }

    #region Face Calm Valiables
    [HideInInspector, SerializeField]
    public Mesh face_calm_mesh;

    [HideInInspector, SerializeField]
    public float face_calm_size_multiplier;

    [HideInInspector, SerializeField]
    public Color face_calm_color;

    [HideInInspector, SerializeField]
    public Texture2D face_calm_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D face_calm_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D face_calm_normal_map;

    [HideInInspector, SerializeField]
    public Texture2D face_calm_emissive_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_calm_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_calm_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_calm_metallic;

    [HideInInspector, SerializeField]
    public float face_calm_emissive_scale;
    #endregion Face Calm Valiables
    private void SetFaceCalmVariables()
    {
        //Face Calm
        VfSetMesh("face_calm_mesh", face_calm_mesh);
        VfSetFloat("face_calm_size_multiplier", face_calm_size_multiplier);
        VfSetV4("face_calm_color", face_calm_color);
        VfSetTexture("face_calm_base_color_map", face_calm_base_color_map);
        VfSetTexture("face_calm_mask_map", face_calm_mask_map);
        VfSetTexture("face_calm_normal_map", face_calm_normal_map);
        VfSetTexture("face_calm_emissive_map", face_calm_emissive_map);
        VfSetFloat("face_calm_normal_scale", face_calm_normal_scale);
        VfSetFloat("face_calm_emissive_scale", face_calm_emissive_scale);
        VfSetFloat("face_calm_smoothness", face_calm_smoothness);
        VfSetFloat("face_calm_metallic", face_calm_metallic);
    }

    #region Face Angry Variables 
    [HideInInspector, SerializeField]
    public Mesh face_angry_mesh;

    [HideInInspector, SerializeField]
    public Mesh face_angry_mesh_mirrored;

    [HideInInspector, SerializeField]
    public float face_angry_size_multiplier;

    [HideInInspector, SerializeField]
    public Color face_angry_color;

    [HideInInspector, SerializeField]
    public Texture2D face_angry_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D face_angry_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D face_angry_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_angry_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_angry_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_angry_metallic;
    #endregion Face Angry Variables
    private void SetFaceAngryVariables()
    {
        //Face Angry
        VfSetMesh("face_angry_mesh", face_angry_mesh);
        VfSetMesh("face_angry_mesh_mirrored", face_angry_mesh_mirrored);
        VfSetFloat("face_angry_size_multiplier", face_angry_size_multiplier);
        VfSetV4("face_angry_color", face_angry_color);
        VfSetTexture("face_angry_base_color_map", face_angry_base_color_map);
        VfSetTexture("face_angry_mask_map", face_angry_mask_map);
        VfSetTexture("face_angry_normal_map", face_angry_normal_map);
        VfSetFloat("face_angry_normal_scale", face_angry_normal_scale);
        VfSetFloat("face_angry_smoothness", face_angry_smoothness);
        VfSetFloat("face_angry_metallic", face_angry_metallic);
    }

    #region Face Detail Variables - shared between calm and angry
    [HideInInspector, SerializeField]
    public Mesh face_detail_mesh;

    [HideInInspector, SerializeField]
    public float face_detail_size_multiplier;

    [HideInInspector, SerializeField]
    public Color face_detail_color;

    [HideInInspector, SerializeField]
    public Texture2D face_detail_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D face_detail_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D face_detail_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_detail_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_detail_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float face_detail_metallic;
    #endregion Face Detail Variables - shared between calm and angry
    private void SetFaceDetailVariables()
    {
        //Face Detail - shared between calm and angry
        VfSetMesh("face_detail_mesh", face_detail_mesh);
        VfSetFloat("face_detail_size_multiplier", face_detail_size_multiplier);
        VfSetV4("face_detail_color", face_detail_color);
        VfSetTexture("face_detail_base_color_map", face_detail_base_color_map);
        VfSetTexture("face_detail_mask_map", face_detail_mask_map);
        VfSetTexture("face_detail_normal_map", face_detail_normal_map);
        VfSetFloat("face_detail_normal_scale", face_detail_normal_scale);
        VfSetFloat("face_detail_smoothness", face_detail_smoothness);
        VfSetFloat("face_detail_metallic", face_detail_metallic);
    }

    #region Eyes Variables
    [HideInInspector, SerializeField]
    public Mesh eyes_mesh;

    [HideInInspector, SerializeField]
    public float eyes_size_multiplier;

    [HideInInspector, SerializeField]
    public Gradient eyes_calm_angry_color_gradient;

    [HideInInspector, SerializeField]
    public Gradient eyesLightCalmAngryColorGradient; //set on light component, not vfx graph

    [HideInInspector, SerializeField]
    public float eyesCalmLumes = 385; //set on light component, not vfx graph

    [HideInInspector, SerializeField]
    public float eyesAngryLumens = 385; //set on light component, not vfx graph

    [HideInInspector, SerializeField]
    public float eyesCalmRange = 1.3f; //set on light component, not vfx graph

    [HideInInspector, SerializeField]
    public float eyesAngryRange = 1.3f; //set on light component, not vfx graph
    #endregion Eyes Variables
    private void SetEyesVariables()
    {
        //Eyes
        VfSetMesh("eyes_mesh", eyes_mesh);
        VfSetFloat("eyes_size_multiplier", eyes_size_multiplier);
        VfSetGradient("eyes_calm_angry_color_gradient", eyes_calm_angry_color_gradient);

        Color newEyeColor = eyesLightCalmAngryColorGradient.Evaluate(morph);
        float eyesLumen = Mathf.Lerp(eyesCalmLumes, eyesAngryLumens, morph);
        float eyesRange = Mathf.Lerp(eyesCalmRange, eyesAngryRange, morph);

        if (lightEyeRight)
        {
            lightEyeRight.color = newEyeColor;
            //lightEyeRight.intensity = eyesLumen;
            lightEyeRight.range = eyesRange;
        }

        if (lightEyeLeft)
        {
            lightEyeLeft.color = newEyeColor;
            //lightEyeLeft.intensity = eyesLumen;
            lightEyeLeft.range = eyesRange;
        }
    }

    #region Body Calm Variables
    //Body Calm Transform
    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_scales;

    [HideInInspector, SerializeField]
    [Range(0.1f, 20)] public float body_calm_anim_pattern_period;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_anim_pattern_intensity;

    //Body Calm Detail Pivot
    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_red_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_red_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_red_z;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_green_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_green_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_green_z;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_blue_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_blue_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_calm_pivot_ab_blue_z;

    //Body Calm Detail Pivot
    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_red_x;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_red_y;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_red_z;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_green_x;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_green_y;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_green_z;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_blue_x;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_blue_y;

    [HideInInspector, SerializeField]
    [Range(-10, 10)] public float body_calm_detail_pivot_ab_blue_z;

    //Body Calm Look
    [HideInInspector, SerializeField]
    public Mesh body_calm_mesh;

    [HideInInspector, SerializeField]
    public float body_calm_size_multiplier;

    [HideInInspector, SerializeField]
    public Color body_calm_color;

    [HideInInspector, SerializeField]
    public Texture2D body_calm_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D body_calm_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D body_calm_normal_map;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_smoothness;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_metallic;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_normal_scale;

    //Body Calm Look Detail
    [HideInInspector, SerializeField]
    public Mesh body_calm_detail_mesh;

    [HideInInspector, SerializeField]
    public float body_calm_detail_size_multiplier = 1;

    [HideInInspector, SerializeField]
    public Color body_calm_detail_color;

    [HideInInspector, SerializeField]
    public Texture2D body_calm_detail_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D body_calm_detail_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D body_calm_detail_normal_map;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_detail_smoothness;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_detail_metallic;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_calm_detail_normal_scale;
    #endregion Body Calm Variables
    private void SetBodyCalmVariables()
    {
        //Body Calm Transform
        VfSetFloat("body_calm_scales", body_calm_scales);
        VfSetFloat("body_calm_anim_pattern_period", body_calm_anim_pattern_period);
        VfSetFloat("body_calm_anim_pattern_intensity", body_calm_anim_pattern_intensity);

        //Body Calm Pivot
        VfSetFloat("body_calm_pivot_ab_red_x", body_calm_pivot_ab_red_x);
        VfSetFloat("body_calm_pivot_ab_red_y", body_calm_pivot_ab_red_y);
        VfSetFloat("body_calm_pivot_ab_red_z", body_calm_pivot_ab_red_z);
        VfSetFloat("body_calm_pivot_ab_green_x", body_calm_pivot_ab_green_x);
        VfSetFloat("body_calm_pivot_ab_green_y", body_calm_pivot_ab_green_y);
        VfSetFloat("body_calm_pivot_ab_green_z", body_calm_pivot_ab_green_z);
        VfSetFloat("body_calm_pivot_ab_blue_x", body_calm_pivot_ab_blue_x);
        VfSetFloat("body_calm_pivot_ab_blue_y", body_calm_pivot_ab_blue_y);
        VfSetFloat("body_calm_pivot_ab_blue_z", body_calm_pivot_ab_blue_z);

        //Body Calm Detail Pivot
        VfSetFloat("body_calm_detail_pivot_ab_red_x", body_calm_detail_pivot_ab_red_x);
        VfSetFloat("body_calm_detail_pivot_ab_red_y", body_calm_detail_pivot_ab_red_y);
        VfSetFloat("body_calm_detail_pivot_ab_red_z", body_calm_detail_pivot_ab_red_z);
        VfSetFloat("body_calm_detail_pivot_ab_green_x", body_calm_detail_pivot_ab_green_x);
        VfSetFloat("body_calm_detail_pivot_ab_green_y", body_calm_detail_pivot_ab_green_y);
        VfSetFloat("body_calm_detail_pivot_ab_green_z", body_calm_detail_pivot_ab_green_z);
        VfSetFloat("body_calm_detail_pivot_ab_blue_x", body_calm_detail_pivot_ab_blue_x);
        VfSetFloat("body_calm_detail_pivot_ab_blue_y", body_calm_detail_pivot_ab_blue_y);
        VfSetFloat("body_calm_detail_pivot_ab_blue_z", body_calm_detail_pivot_ab_blue_z);

        //Body Calm Look
        VfSetMesh("body_calm_mesh", body_calm_mesh);
        VfSetFloat("body_calm_size_multiplier", body_calm_size_multiplier);
        VfSetV4("body_calm_color", body_calm_color);
        VfSetTexture("body_calm_base_color_map", body_calm_base_color_map);
        VfSetTexture("body_calm_mask_map", body_calm_mask_map);
        VfSetTexture("body_calm_normal_map", body_calm_normal_map);
        VfSetFloat("body_calm_normal_scale", body_calm_normal_scale);
        VfSetFloat("body_calm_smoothness", body_calm_smoothness);
        VfSetFloat("body_calm_metallic", body_calm_metallic);

        //Body Calm Look Detail
        VfSetMesh("body_calm_detail_mesh", body_calm_detail_mesh);
        VfSetFloat("body_calm_detail_size_multiplier", body_calm_detail_size_multiplier);
        VfSetV4("body_calm_detail_color", body_calm_detail_color);
        VfSetTexture("body_calm_detail_base_color_map", body_calm_detail_base_color_map);
        VfSetTexture("body_calm_detail_mask_map", body_calm_detail_mask_map);
        VfSetTexture("body_calm_detail_normal_map", body_calm_detail_normal_map);
        VfSetFloat("body_calm_detail_normal_scale", body_calm_detail_normal_scale);
        VfSetFloat("body_calm_detail_smoothness", body_calm_detail_smoothness);
        VfSetFloat("body_calm_detail_metallic", body_calm_detail_metallic);
    }

    #region Body Angry Variables
    //Body Angry Transform
    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_scales;

    [HideInInspector, SerializeField]
    [Range(0.1f, 20)] public float body_angry_anim_pattern_period;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_anim_pattern_intensity;

    //Body Angry Detail Pivot
    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_red_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_red_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_red_z;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_green_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_green_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_green_z;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_blue_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_blue_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_pivot_ab_blue_z;

    //Body Angry Detail Pivot
    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_red_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_red_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_red_z;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_green_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_green_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_green_z;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_blue_x;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_blue_y;

    [HideInInspector, SerializeField]
    [Range(-1, 1)] public float body_angry_detail_pivot_ab_blue_z;

    //Body Angry Look
    [HideInInspector, SerializeField]
    public Mesh body_angry_mesh;

    [HideInInspector, SerializeField]
    public float body_angry_size_multiplier;

    [HideInInspector, SerializeField]
    public Color body_angry_color;

    [HideInInspector, SerializeField]
    public Texture2D body_angry_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D body_angry_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D body_angry_normal_map;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_smoothness;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_metallic;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_normal_scale;

    //Body Angry Detail
    [HideInInspector, SerializeField]
    public Mesh body_angry_detail_mesh;

    [HideInInspector, SerializeField]
    public float body_angry_detail_size_multiplier = 1;

    [HideInInspector, SerializeField]
    public Color body_angry_detail_color;

    [HideInInspector, SerializeField]
    public Texture2D body_angry_detail_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D body_angry_detail_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D body_angry_detail_normal_map;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_detail_smoothness;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_detail_metallic;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float body_angry_detail_normal_scale;
    #endregion Body Angry Variables
    private void SetBodyAngryVariables()
    {
        //Body Angry Transform
        VfSetFloat("body_angry_scales", body_angry_scales);
        VfSetFloat("body_angry_anim_pattern_period", body_angry_anim_pattern_period);
        VfSetFloat("body_angry_anim_pattern_intensity", body_angry_anim_pattern_intensity);

        //Body Angry Pivot
        VfSetFloat("body_angry_pivot_ab_red_x", body_angry_pivot_ab_red_x);
        VfSetFloat("body_angry_pivot_ab_red_y", body_angry_pivot_ab_red_y);
        VfSetFloat("body_angry_pivot_ab_red_z", body_angry_pivot_ab_red_z);
        VfSetFloat("body_angry_pivot_ab_green_x", body_angry_pivot_ab_green_x);
        VfSetFloat("body_angry_pivot_ab_green_y", body_angry_pivot_ab_green_y);
        VfSetFloat("body_angry_pivot_ab_green_z", body_angry_pivot_ab_green_z);
        VfSetFloat("body_angry_pivot_ab_blue_x", body_angry_pivot_ab_blue_x);
        VfSetFloat("body_angry_pivot_ab_blue_y", body_angry_pivot_ab_blue_y);
        VfSetFloat("body_angry_pivot_ab_blue_z", body_angry_pivot_ab_blue_z);

        //Body Angry Detail Pivot
        VfSetFloat("body_angry_detail_pivot_ab_red_x", body_angry_detail_pivot_ab_red_x);
        VfSetFloat("body_angry_detail_pivot_ab_red_y", body_angry_detail_pivot_ab_red_y);
        VfSetFloat("body_angry_detail_pivot_ab_red_z", body_angry_detail_pivot_ab_red_z);
        VfSetFloat("body_angry_detail_pivot_ab_green_x", body_angry_detail_pivot_ab_green_x);
        VfSetFloat("body_angry_detail_pivot_ab_green_y", body_angry_detail_pivot_ab_green_y);
        VfSetFloat("body_angry_detail_pivot_ab_green_z", body_angry_detail_pivot_ab_green_z);
        VfSetFloat("body_angry_detail_pivot_ab_blue_x", body_angry_detail_pivot_ab_blue_x);
        VfSetFloat("body_angry_detail_pivot_ab_blue_y", body_angry_detail_pivot_ab_blue_y);
        VfSetFloat("body_angry_detail_pivot_ab_blue_z", body_angry_detail_pivot_ab_blue_z);

        //Body Angry
        VfSetMesh("body_angry_mesh", body_angry_mesh);
        VfSetFloat("body_angry_size_multiplier", body_angry_size_multiplier);
        VfSetV4("body_angry_color", body_angry_color);
        VfSetTexture("body_angry_base_color_map", body_angry_base_color_map);
        VfSetTexture("body_angry_mask_map", body_angry_mask_map);
        VfSetTexture("body_angry_normal_map", body_angry_normal_map);
        VfSetFloat("body_angry_normal_scale", body_angry_normal_scale);
        VfSetFloat("body_angry_smoothness", body_angry_smoothness);
        VfSetFloat("body_angry_metallic", body_angry_metallic);
        VfSetFloat("body_angry_anim_pattern_period", body_angry_anim_pattern_period);
        VfSetFloat("body_angry_anim_pattern_intensity", body_angry_anim_pattern_intensity);

        //Body Angry Detail
        VfSetMesh("body_angry_detail_mesh", body_angry_detail_mesh);
        VfSetFloat("body_angry_detail_size_multiplier", body_angry_detail_size_multiplier);
        VfSetV4("body_angry_detail_color", body_angry_detail_color);
        VfSetTexture("body_angry_detail_base_color_map", body_angry_detail_base_color_map);
        VfSetTexture("body_angry_detail_mask_map", body_angry_detail_mask_map);
        VfSetTexture("body_angry_detail_normal_map", body_angry_detail_normal_map);
        VfSetFloat("body_angry_detail_normal_scale", body_angry_detail_normal_scale);
        VfSetFloat("body_angry_detail_smoothness", body_angry_detail_smoothness);
        VfSetFloat("body_angry_detail_metallic", body_angry_detail_metallic);
    }

    #region Cloth Calm Variables
    [HideInInspector, SerializeField]
    public Mesh cloth_calm_mesh;

    [HideInInspector, SerializeField]
    public float cloth_calm_size_multiplier;

    [HideInInspector, SerializeField]
    public Color cloth_calm_color;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_metallic;

    //Cloth Calm Detail A
    [HideInInspector, SerializeField]
    public Mesh cloth_calm_detail_a_mesh;

    [HideInInspector, SerializeField]
    public float cloth_calm_detail_a_size_multiplier;

    [HideInInspector, SerializeField]
    public Color cloth_calm_detail_a_color;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_detail_a_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_detail_a_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_detail_a_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_detail_a_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_detail_a_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_detail_a_metallic;

    //Cloth Calm Detail B
    [HideInInspector, SerializeField]
    public Mesh cloth_calm_detail_b_mesh;

    [HideInInspector, SerializeField]
    public float cloth_calm_detail_b_size_multiplier;

    [HideInInspector, SerializeField]
    public Color cloth_calm_detail_b_color;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_detail_b_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_detail_b_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_calm_detail_b_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_detail_b_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_detail_b_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_calm_detail_b_metallic;
    #endregion Cloth Calm Variables
    private void SetClothCalmVariables()
    {
        //Cloth Calm
        VfSetMesh("cloth_calm_mesh", cloth_calm_mesh);
        VfSetFloat("cloth_calm_size_multiplier", cloth_calm_size_multiplier);
        VfSetV4("cloth_calm_color", cloth_calm_color);
        VfSetTexture("cloth_calm_base_color_map", cloth_calm_base_color_map);
        VfSetTexture("cloth_calm_mask_map", cloth_calm_mask_map);
        VfSetTexture("cloth_calm_normal_map", cloth_calm_normal_map);
        VfSetFloat("cloth_calm_normal_scale", cloth_calm_normal_scale);
        VfSetFloat("cloth_calm_smoothness", cloth_calm_smoothness);
        VfSetFloat("cloth_calm_metallic", cloth_calm_metallic);

        //Cloth Calm Detail A
        VfSetMesh("cloth_calm_detail_a_mesh", cloth_calm_detail_a_mesh);
        VfSetFloat("cloth_calm_detail_a_size_multiplier", cloth_calm_detail_a_size_multiplier);
        VfSetV4("cloth_calm_detail_a_color", cloth_calm_detail_a_color);
        VfSetTexture("cloth_calm_detail_a_base_color_map", cloth_calm_detail_a_base_color_map);
        VfSetTexture("cloth_calm_detail_a_mask_map", cloth_calm_detail_a_mask_map);
        VfSetTexture("cloth_calm_detail_a_normal_map", cloth_calm_detail_a_normal_map);
        VfSetFloat("cloth_calm_detail_a_normal_scale", cloth_calm_detail_a_normal_scale);
        VfSetFloat("cloth_calm_detail_a_smoothness", cloth_calm_detail_a_smoothness);
        VfSetFloat("cloth_calm_detail_a_metallic", cloth_calm_detail_a_metallic);

        //Cloth Calm Detail B
        VfSetMesh("cloth_calm_detail_b_mesh", cloth_calm_detail_b_mesh);
        VfSetFloat("cloth_calm_detail_b_size_multiplier", cloth_calm_detail_b_size_multiplier);
        VfSetV4("cloth_calm_detail_b_color", cloth_calm_detail_b_color);
        VfSetTexture("cloth_calm_detail_b_base_color_map", cloth_calm_detail_b_base_color_map);
        VfSetTexture("cloth_calm_detail_b_mask_map", cloth_calm_detail_b_mask_map);
        VfSetTexture("cloth_calm_detail_b_normal_map", cloth_calm_detail_b_normal_map);
        VfSetFloat("cloth_calm_detail_b_normal_scale", cloth_calm_detail_b_normal_scale);
        VfSetFloat("cloth_calm_detail_b_smoothness", cloth_calm_detail_b_smoothness);
        VfSetFloat("cloth_calm_detail_b_metallic", cloth_calm_detail_b_metallic);
    }

    #region Cloth Angry Variables
    [HideInInspector, SerializeField]
    public Mesh cloth_angry_mesh;

    [HideInInspector, SerializeField]
    public float cloth_angry_size_multiplier;

    [HideInInspector, SerializeField]
    public Color cloth_angry_color;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_metallic;

    //Cloth Angry Detail A
    [HideInInspector, SerializeField]
    public Mesh cloth_angry_detail_a_mesh;

    [HideInInspector, SerializeField]
    public float cloth_angry_detail_a_size_multiplier;

    [HideInInspector, SerializeField]
    public Color cloth_angry_detail_a_color;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_detail_a_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_detail_a_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_detail_a_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_detail_a_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_detail_a_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_detail_a_metallic;

    //Cloth Angry Detail B
    [HideInInspector, SerializeField]
    public Mesh cloth_angry_detail_b_mesh;

    [HideInInspector, SerializeField]
    public float cloth_angry_detail_b_size_multiplier;

    [HideInInspector, SerializeField]
    public Color cloth_angry_detail_b_color;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_detail_b_base_color_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_detail_b_mask_map;

    [HideInInspector, SerializeField]
    public Texture2D cloth_angry_detail_b_normal_map;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_detail_b_normal_scale;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_detail_b_smoothness;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float cloth_angry_detail_b_metallic;
    #endregion Cloth Angry Variables
    private void SetClothAngryVariables()
    {
        //Cloth Angry
        VfSetMesh("cloth_angry_mesh", cloth_angry_mesh);
        VfSetFloat("cloth_angry_size_multiplier", cloth_angry_size_multiplier);
        VfSetV4("cloth_angry_color", cloth_angry_color);
        VfSetTexture("cloth_angry_base_color_map", cloth_angry_base_color_map);
        VfSetTexture("cloth_angry_mask_map", cloth_angry_mask_map);
        VfSetTexture("cloth_angry_normal_map", cloth_angry_normal_map);
        VfSetFloat("cloth_angry_normal_scale", cloth_angry_normal_scale);
        VfSetFloat("cloth_angry_smoothness", cloth_angry_smoothness);
        VfSetFloat("cloth_angry_metallic", cloth_angry_metallic);

        //Cloth Calm Angry A
        VfSetMesh("cloth_angry_detail_a_mesh", cloth_angry_detail_a_mesh);
        VfSetFloat("cloth_angry_detail_a_size_multiplier", cloth_angry_detail_a_size_multiplier);
        VfSetV4("cloth_angry_detail_a_color", cloth_angry_detail_a_color);
        VfSetTexture("cloth_angry_detail_a_base_color_map", cloth_angry_detail_a_base_color_map);
        VfSetTexture("cloth_angry_detail_a_mask_map", cloth_angry_detail_a_mask_map);
        VfSetTexture("cloth_angry_detail_a_normal_map", cloth_angry_detail_a_normal_map);
        VfSetFloat("cloth_angry_detail_a_normal_scale", cloth_angry_detail_a_normal_scale);
        VfSetFloat("cloth_angry_detail_a_smoothness", cloth_angry_detail_a_smoothness);
        VfSetFloat("cloth_angry_detail_a_metallic", cloth_angry_detail_a_metallic);

        //Cloth Calm Angry B
        VfSetMesh("cloth_angry_detail_b_mesh", cloth_angry_detail_b_mesh);
        VfSetFloat("cloth_angry_detail_b_size_multiplier", cloth_angry_detail_b_size_multiplier);
        VfSetV4("cloth_angry_detail_b_color", cloth_angry_detail_b_color);
        VfSetTexture("cloth_angry_detail_b_base_color_map", cloth_angry_detail_b_base_color_map);
        VfSetTexture("cloth_angry_detail_b_mask_map", cloth_angry_detail_b_mask_map);
        VfSetTexture("cloth_angry_detail_b_normal_map", cloth_angry_detail_b_normal_map);
        VfSetFloat("cloth_angry_detail_b_normal_scale", cloth_angry_detail_b_normal_scale);
        VfSetFloat("cloth_angry_detail_b_smoothness", cloth_angry_detail_b_smoothness);
        VfSetFloat("cloth_angry_detail_b_metallic", cloth_angry_detail_b_metallic);
    }
    #endregion Appearance Settings

    #region Fire Variables
    [HideInInspector, SerializeField]
    public enum fire_mode { LitMeshOpaque, UnlitTriangelsAlpha, ScreenSpaceBlurDistort };

    [HideInInspector, SerializeField]
    public fire_mode fireMode;

    [HideInInspector, SerializeField]
    public Mesh fire_mesh;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float fire_blur_scale;

    [HideInInspector, SerializeField]
    public Vector2 fire_distortion_scale;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_amount;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_intensity_min;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_intensity_max;

    [HideInInspector, SerializeField]
    [Range(0, 5)] float fire_intensity_noise_frequency;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float fire_emissive_intensity;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_emissive_use_velocity_for_emissive;

    [HideInInspector, SerializeField]
    public Vector3 fire_direction;

    [HideInInspector, SerializeField]
    public AnimationCurve fire_morph_curve;

    [HideInInspector, SerializeField]
    public Gradient fire_diffuse_gradient_calm;

    [HideInInspector, SerializeField]
    public Gradient fire_diffuse_gradient_angry;

    [HideInInspector, SerializeField]
    public Gradient fire_emissive_gradient_calm;

    [HideInInspector, SerializeField]
    public Gradient fire_emissive_gradient_angry;

    [HideInInspector, SerializeField]
    [Range(0, 5)] public float fire_size_min;

    [HideInInspector, SerializeField]
    [Range(0, 5)] public float fire_size_max;

    [HideInInspector, SerializeField]
    public AnimationCurve fire_multiply_size_over_lifetime;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float fire_turbulence_speed;

    [HideInInspector, SerializeField]
    public Vector3 fire_turbulence_scale;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float fire_turbulence_intensity;

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float fire_turbulence_drag;

    [HideInInspector, SerializeField]
    [Range(0, 2)] public float fire_turbulence_frequency;

    [HideInInspector, SerializeField]
    [Range(0, 5)] public int fire_turbulence_octaves;

    [HideInInspector, SerializeField]
    [Range(0, 5)] public float fire_turbulence_roughness;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_contribution_face;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_contribution_body;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_contribution_helmet;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_contribution_eyes;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float fire_contribution_cloth;

    [HideInInspector, SerializeField]
    [Range(0, 100)] public float fire_outwards_force;
    #endregion Fire Variables
    private void SetFireEffectVariables()
    {
        if (fireMode == fire_mode.LitMeshOpaque) VfSetInt("fire_mode", 0);
        if (fireMode == fire_mode.UnlitTriangelsAlpha) VfSetInt("fire_mode", 1);
        if (fireMode == fire_mode.ScreenSpaceBlurDistort) VfSetInt("fire_mode", 2);

        VfSetMesh("fire_mesh", fire_mesh);
        VfSetFloat("fire_blur_scale", fire_blur_scale);
        VfSetV2("fire_distortion_scale", fire_distortion_scale);

        VfSetFloat("fire_amount", fire_amount);
        VfSetFloat("fire_intensity_min", fire_intensity_min);
        VfSetFloat("fire_intensity_max", fire_intensity_max);
        VfSetFloat("fire_intensity_noise_frequency", fire_intensity_noise_frequency);

        VfSetV3("fire_direction", fire_direction);

        VfSetCurve("fire_morph_curve", fire_morph_curve);

        VfSetFloat("fire_emissive_intensity", fire_emissive_intensity);
        VfSetFloat("fire_emissive_use_velocity_for_emissive", fire_emissive_use_velocity_for_emissive);

        VfSetGradient("fire_diffuse_gradient_calm", fire_diffuse_gradient_calm);
        VfSetGradient("fire_diffuse_gradient_angry", fire_diffuse_gradient_angry);

        VfSetGradient("fire_emissive_gradient_calm", fire_emissive_gradient_calm);
        VfSetGradient("fire_emissive_gradient_angry", fire_emissive_gradient_angry);

        VfSetFloat("fire_size_min", fire_size_min);
        VfSetFloat("fire_size_max", fire_size_max);
        VfSetCurve("fire_multiply_size_over_lifetime", fire_multiply_size_over_lifetime);
        VfSetFloat("fire_turbulence_speed", fire_turbulence_speed);
        VfSetV3("fire_turbulence_scale", fire_turbulence_scale);
        VfSetFloat("fire_turbulence_intensity", fire_turbulence_intensity);
        VfSetFloat("fire_turbulence_drag", fire_turbulence_drag);
        VfSetFloat("fire_turbulence_frequency", fire_turbulence_frequency);
        VfSetInt("fire_turbulence_octaves", fire_turbulence_octaves);
        VfSetFloat("fire_turbulence_roughness", fire_turbulence_roughness);

        VfSetFloat("fire_contribution_face", fire_contribution_face);
        VfSetFloat("fire_contribution_body", fire_contribution_body);
        VfSetFloat("fire_contribution_helmet", fire_contribution_helmet);
        VfSetFloat("fire_contribution_eyes", fire_contribution_eyes);
        VfSetFloat("fire_contribution_cloth", fire_contribution_cloth);
        VfSetFloat("fire_outwards_force", fire_outwards_force);
    }

    #region Crumble Variables
    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float shockwave = 0.0f;

    [HideInInspector, SerializeField]
    [Range(0, 1)] public float shockwave_debris;

    [HideInInspector, SerializeField]
    [Range(0, 50)] public float shockwave_debris_outwards_force;

    [HideInInspector, SerializeField]
    public Transform shockwave_origin;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float crumble = 0.0f;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float crumble_b = 0.0f;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float crumble_debris = 0.0f;

    [HideInInspector, SerializeField]
    [Range(0, 50)] public float crumble_debris_outwards_force;

    [HideInInspector, SerializeField]
    [Range(0.0f, 10.0f)] public float crumble_turbulence_intensity = 1;

    [HideInInspector, SerializeField]
    public Vector3 crumble_turbulence_scale = new Vector3(1, 1, 1);

    [HideInInspector, SerializeField]
    [Range(0, 10)] public float crumble_turbulence_speed = 0.0f;

    [HideInInspector, SerializeField]
    [Range(0.0f, 10.0f)] public float crumble_turbulence_drag = 1;

    [HideInInspector, SerializeField]
    [Range(0.0f, 20.0f)] public float crumble_turbulence_frequency = 1;

    [HideInInspector, SerializeField]
    [Range(1, 5)] public int crumble_turbulence_octaves = 1;

    [HideInInspector, SerializeField]
    [Range(0.0f, 1.0f)] public float crumble_turbulence_roughness = 0.5f;

    [HideInInspector, SerializeField]
    public Vector3 crumble_force_min = new Vector3(-1.5f, -10.0f, -1.5f);

    [HideInInspector, SerializeField]
    public Vector3 crumble_force_max = new Vector3(1.5f, -15.0f, 1.5f);

    [HideInInspector, SerializeField]
    public Vector3 crumble_angular_velocity_min = new Vector3(0, 0, 0);

    [HideInInspector, SerializeField]
    public Vector3 crumble_angular_velocity_max = new Vector3(0, 0, 0);
    #endregion Crumble Variables
    private void SetCrumbleVariables ()
    {
        VfSetV3("shockwave_origin", shockwave_origin.position);

        VfSetFloat("shockwave", shockwave);
        VfSetFloat("shockwave_debris", shockwave_debris);
        VfSetFloat("shockwave_debris_outwards_force", shockwave_debris_outwards_force);

        VfSetFloat("crumble", crumble);
        VfSetFloat("crumble_b", crumble_b);
        VfSetFloat("crumble_debris", crumble_debris);
        VfSetFloat("crumble_debris_outwards_force", crumble_debris_outwards_force);

        VfSetV3("crumble_force_min", crumble_force_min);
        VfSetV3("crumble_force_max", crumble_force_max);

        VfSetV3("crumble_angular_velocity_min", crumble_angular_velocity_min);
        VfSetV3("crumble_angular_velocity_max", crumble_angular_velocity_max);

        VfSetFloat("crumble_turbulence_intensity", crumble_turbulence_intensity);
        VfSetV3("crumble_turbulence_scale", crumble_turbulence_scale);
        VfSetFloat("crumble_turbulence_speed", crumble_turbulence_speed);
        VfSetFloat("crumble_turbulence_drag", crumble_turbulence_drag);
        VfSetFloat("crumble_turbulence_frequency", crumble_turbulence_frequency);
        VfSetInt("crumble_turbulence_octaves", crumble_turbulence_octaves);
        VfSetFloat("crumble_turbulence_roughness", crumble_turbulence_roughness);
    }

    #region Debug Settings
    #region Debug Variables
    [HideInInspector, SerializeField]
    public enum debugProperties { None, Morph, ShockwaveMask, CrumbleMask, PullBackMask, MorphPeel }

    [HideInInspector, SerializeField]
    public Color debug_color_a, debug_color_b;

    [HideInInspector, SerializeField]
    public debugProperties debugProp;

    [HideInInspector, SerializeField]
    public bool disableBodyVFX;

    [HideInInspector, SerializeField]
    public bool disableOuterLayersVFX; //Legacy from The Heretic

    [HideInInspector, SerializeField]
    public bool disableFaceVFX;

    [HideInInspector, SerializeField]
    public bool disableHelmetVFX;

    [HideInInspector, SerializeField]
    public bool disableHairVFX;

    [HideInInspector, SerializeField]
    public bool disableClothVFX;

    [HideInInspector]
    public bool disableVFXGraph = false;

    [HideInInspector, SerializeField]
    public bool pauseVFX;

    [HideInInspector]
    [Range(0, 1)] public float playRate = 1;
    #endregion Debug Variables
    //Debug Properties
    private void DebugProperties()
    {
        VfSetV4("debug_color_a", debug_color_a);
        VfSetV4("debug_color_b", debug_color_b);

        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasInt("debug_properties"))
            {
                if (debugProp == debugProperties.None) vf.SetInt("debug_properties", 0);
                if (debugProp == debugProperties.Morph) vf.SetInt("debug_properties", 1);
                if (debugProp == debugProperties.ShockwaveMask) vf.SetInt("debug_properties", 5);
                if (debugProp == debugProperties.CrumbleMask) vf.SetInt("debug_properties", 6);
                if (debugProp == debugProperties.PullBackMask) vf.SetInt("debug_properties", 4);
                if (debugProp == debugProperties.MorphPeel) vf.SetInt("debug_properties", 2);
            }
        }
    }

    //Displays Morgan as mesh
    public void MorganAsMesh(bool vertexColors)
    {
        ShowBodyAsMesh(true, vertexColors);
        ShowFaceAsMesh(true, vertexColors);
        ShowHelmetAsMesh(true, vertexColors);
        ShowClothAsMesh(true, vertexColors);

        morganVFXGraphs[13].gameObject.SetActive(false); //the fire vfx

        disableBodyVFX = true;
        disableFaceVFX = true;
        disableHelmetVFX = true;
        disableClothVFX = true;
    }

    //Resets Morgan
    public void Reset()
    {
        ShowBodyAsMesh(false, false);
        ShowFaceAsMesh(false, false);
        ShowHelmetAsMesh(false, false);
        ShowClothAsMesh(false, false);

        morganVFXGraphs[13].gameObject.SetActive(true); //the fire vfx

        PauseVFX(false);

        DisableVFX(1, false); //Helmet
        DisableVFX(2, false); //Face
        DisableVFX(3, false); //Body
        DisableVFX(4, false); //Outer Layer
        DisableVFX(5, false); //Cloth

        skipVFXUpdates = false;

        //RegenerateRenderTextures();
        ReinitVFXGraphs();
    }

    //Reinitializes the VFX Graphs (not to be confused with recompiling them)
    public void ReinitVFXGraphs()
    {
        if (morganVFXGraphs.Length > 0)
            foreach (VisualEffect vf in morganVFXGraphs)
                vf.Reinit();
    }

    //Sets the Body Mesh to be displayed with the debug material or with the VFX material
    public void ShowBodyAsMesh(bool bodyAsMesh, bool vertexColors)
    {
        if (bodyRenderer != null)
        {
            Material[] bodyMaterials = bodyRenderer.sharedMaterials;

            if (bodyAsMesh)
            {
                if (vertexColors)
                {
                    bodyMaterials[0] = debugMatVertexColors;

                    bodyRenderer.sharedMaterials = bodyMaterials;

                    eyeRightRenderer.sharedMaterial = debugMatVertexColors;
                    eyeLeftRenderer.sharedMaterial = debugMatVertexColors;

                    Debug.Log("Display Body Vertex Colors");
                }

                else
                {
                    bodyMaterials[0] = debugMatBody;

                    bodyRenderer.sharedMaterials = bodyMaterials;

                    eyeRightRenderer.sharedMaterial = debugMatEyes;
                    eyeLeftRenderer.sharedMaterial = debugMatEyes;

                    Debug.Log("Display Body Mesh");
                }

                disableBodyVFX = true;
            }

            else
            {
                bodyMaterials[0] = vfxMatBody;

                bodyRenderer.sharedMaterials = bodyMaterials;

                disableBodyVFX = false;

                ReinitVFXGraphs();

                Debug.Log("Display Body VFX");
            }
        }
        else Debug.LogWarning("Body Mesh Renderer not assigned!");
    }

    //Sets the Face Mesh to be displayed with the debug material or with the VFX material
    public void ShowFaceAsMesh(bool faceAsMesh, bool vertexColors)
    {
        if (bodyRenderer != null)
        {
            Material[] faceMaterials = bodyRenderer.sharedMaterials;

            if (faceAsMesh)
            {
                if (vertexColors)
                {
                    faceMaterials[1] = debugMatVertexColors;

                    bodyRenderer.sharedMaterials = faceMaterials;

                    eyeRightRenderer.sharedMaterial = debugMatVertexColors;
                    eyeLeftRenderer.sharedMaterial = debugMatVertexColors;

                    Debug.Log("Display Face Vertex Colors");
                }

                else
                {
                    faceMaterials[1] = debugMatBody;

                    bodyRenderer.sharedMaterials = faceMaterials;

                    eyeRightRenderer.sharedMaterial = debugMatEyes;
                    eyeLeftRenderer.sharedMaterial = debugMatEyes;


                    Debug.Log("Display Face Mesh");
                }

                disableFaceVFX = true;
            }

            else
            {
                faceMaterials[1] = vfxMatFace;

                bodyRenderer.sharedMaterials = faceMaterials;

                eyeRightRenderer.sharedMaterial = vfxMatEyes;
                eyeLeftRenderer.sharedMaterial = vfxMatEyes;

                disableFaceVFX = false;

                ReinitVFXGraphs();

                Debug.Log("Display Face VFX");
            }
        }
        else Debug.LogWarning("Body Mesh Renderer not assigned!");
    }

    //Sets the Helmet Mesh to be displayed with the debug material or with the VFX material
    public void ShowHelmetAsMesh(bool hairAsMesh, bool vertexColors)
    {
        if (helmetSkinnedMesh != null)
        {
            Material[] helmetMaterials = helmetSkinnedMesh.sharedMaterials;
            Material[] helmetMaleMaterials = helmetMaleSkinnedMesh.sharedMaterials;

            if (hairAsMesh)
            {
                if (vertexColors)
                {
                    helmetMaterials[0] = debugMatVertexColors;
                    helmetMaterials[1] = debugMatVertexColors;
                    helmetSkinnedMesh.sharedMaterials = helmetMaterials;


                    helmetMaleMaterials[0] = debugMatVertexColors;
                    helmetMaleMaterials[1] = debugMatVertexColors;
                    helmetMaleSkinnedMesh.sharedMaterials = helmetMaleMaterials;

                    Debug.Log("Display Helmet Vertex Colors");
                }

                else
                {
                    helmetMaterials[0] = debugMatHelmet;
                    helmetMaterials[1] = debugMatHelmet;
                    helmetSkinnedMesh.sharedMaterials = helmetMaterials;

                    helmetMaleMaterials[0] = debugMatHelmetMale;
                    helmetMaleMaterials[1] = debugMatHelmetMale;
                    helmetMaleSkinnedMesh.sharedMaterials = helmetMaleMaterials;

                    Debug.Log("Display Helmet Mesh");
                }
                disableHelmetVFX = true;
            }

            else
            {
                helmetMaterials[0] = vfxMatHelmetTop;
                helmetMaterials[1] = vfxHelmetFemale;

                helmetSkinnedMesh.sharedMaterials = helmetMaterials;

                helmetMaleMaterials[0] = vfxHelmetMaleA;
                helmetMaleMaterials[1] = vfxHelmetMaleB;
                helmetMaleSkinnedMesh.sharedMaterials = helmetMaleMaterials;

                disableHelmetVFX = false;
                ReinitVFXGraphs();

                Debug.Log("Display Helmet VFX");
            }
        }
        else Debug.LogWarning("Helmet Mesh Renderer not assigned!");
    }

    //Sets the Hair Mesh to be displayed with the debug material or with the VFX material - Legacy from The Heretic
    public void ShowHairAsMesh(bool hairAsMesh, bool vertexColors)
    {
        if (helmetSkinnedMesh != null) //this needs to be fixed 
        {
            if (hairAsMesh)
            {
                if (vertexColors)
                {
                    hairSkinnedMesh.sharedMaterial = debugMatVertexColors;

                    Debug.Log("Display Hair Vertex Colors");
                }

                else
                {
                    hairSkinnedMesh.sharedMaterial = debugMatHelmet;

                    Debug.Log("Display Hair Mesh");
                }
                disableHairVFX = true;
            }

            else
            {
                hairSkinnedMesh.sharedMaterial = vfxMatHair;

                disableHairVFX = false;
                ReinitVFXGraphs();

                Debug.Log("Display Hair VFX");
            }
        }
        else Debug.LogWarning("Display Mesh Renderer not assigned!");
    }

    //Sets the Cloth Mesh to be displayed with the debug material or with the VFX material
    public void ShowClothAsMesh(bool clothAsMesh, bool vertexColors)
    {
        if (clothRenderer != null)
        {
            if (clothAsMesh)
            {
                if (vertexColors)
                {
                    clothRenderer.sharedMaterial = debugMatVertexColors;

                    Debug.Log("Display Cloth Vertex Colors");
                }

                else
                {
                    clothRenderer.sharedMaterial = debugMatCloth;

                    Debug.Log("Display Cloth Mesh");
                }

                disableClothVFX = true;
            }

            else
            {
                clothRenderer.sharedMaterial = vfxMatCloth;

                disableClothVFX = false;
                ReinitVFXGraphs();

                Debug.Log("Display Cloth VFX");
            }
        }
        else Debug.LogWarning("Cloth Mesh Renderer not assigned!");
    }

    //Used to disable/enable parts of the VFX Graph
    public void DisableVFX(int vfxGraphNumber, bool disable)
    {
        if (disable)
        {
            //Disables the helmet
            if (vfxGraphNumber == 1 && morganVFXGraphs[8].gameObject.activeSelf)
            {
                morganVFXGraphs[8].gameObject.SetActive(false);
                morganVFXGraphs[11].gameObject.SetActive(false);
                morganVFXGraphs[12].gameObject.SetActive(false);

                Debug.Log("Helmet VFX is disabled");
            }

            //Disables the face
            else if (vfxGraphNumber == 2 && morganVFXGraphs[1].gameObject.activeSelf)
            {
                morganVFXGraphs[1].gameObject.SetActive(false);
                morganVFXGraphs[2].gameObject.SetActive(false);
                morganVFXGraphs[3].gameObject.SetActive(false);
                morganVFXGraphs[4].gameObject.SetActive(false);
                morganVFXGraphs[9].gameObject.SetActive(false);
                morganVFXGraphs[10].gameObject.SetActive(false);
                morganVFXGraphs[18].gameObject.SetActive(false);

                Debug.Log("Face VFX is disabled");
            }

            //Disables the body
            else if (vfxGraphNumber == 3 && morganVFXGraphs[0].gameObject.activeSelf)
            {
                morganVFXGraphs[0].gameObject.SetActive(false);
                morganVFXGraphs[5].gameObject.SetActive(false);

                Debug.Log("Body VFX is disabled");
            }

            //Disables the outer layers
            else if (vfxGraphNumber == 4 && morganVFXGraphs[6].gameObject.activeSelf)
            {
                morganVFXGraphs[6].gameObject.SetActive(false);
                morganVFXGraphs[7].gameObject.SetActive(false);

                Debug.Log("Outer Layers VFX is disabled");
            }

            //Disables cloth
            else if (vfxGraphNumber == 5 && morganVFXGraphs[14].gameObject.activeSelf)
            {
                morganVFXGraphs[14].gameObject.SetActive(false);
                morganVFXGraphs[15].gameObject.SetActive(false);
                morganVFXGraphs[16].gameObject.SetActive(false);
                morganVFXGraphs[17].gameObject.SetActive(false);

                Debug.Log("Cloth VFX is disabled");
            }
        }

        else
        {
            //Enables the helmet
            if (vfxGraphNumber == 1 && !morganVFXGraphs[8].gameObject.activeSelf)
            {
                morganVFXGraphs[8].gameObject.SetActive(true);
                morganVFXGraphs[11].gameObject.SetActive(true);
                morganVFXGraphs[12].gameObject.SetActive(true);

                Debug.Log("Helmet VFX is enabled");
            }

            //Enables the face
            else if (vfxGraphNumber == 2 && !morganVFXGraphs[1].gameObject.activeSelf)
            {
                morganVFXGraphs[1].gameObject.SetActive(true);
                morganVFXGraphs[2].gameObject.SetActive(true);
                morganVFXGraphs[3].gameObject.SetActive(true);
                morganVFXGraphs[4].gameObject.SetActive(true);
                morganVFXGraphs[9].gameObject.SetActive(true);
                morganVFXGraphs[10].gameObject.SetActive(true);
                morganVFXGraphs[18].gameObject.SetActive(true);

                Debug.Log("Face VFX is enabled");
            }

            //Enables the body
            else if (vfxGraphNumber == 3 && !morganVFXGraphs[0].gameObject.activeSelf)
            {
                morganVFXGraphs[0].gameObject.SetActive(true);
                morganVFXGraphs[5].gameObject.SetActive(true);

                Debug.Log("Body VFX is enabled");
            }

            //Enables the outer layers
            else if (vfxGraphNumber == 4 && !morganVFXGraphs[6].gameObject.activeSelf)
            {
                morganVFXGraphs[6].gameObject.SetActive(true);
                morganVFXGraphs[7].gameObject.SetActive(true);

                Debug.Log("Outer Layers VFX is enabled");
            }

            //Enables cloth
            else if (vfxGraphNumber == 5 && !morganVFXGraphs[14].gameObject.activeSelf)
            {
                morganVFXGraphs[14].gameObject.SetActive(true);
                morganVFXGraphs[15].gameObject.SetActive(true);
                morganVFXGraphs[16].gameObject.SetActive(true);
                morganVFXGraphs[17].gameObject.SetActive(true);

                Debug.Log("Cloth VFX is enabled");
            }
        }
    }

    //Play & Stop the VFX Graph
    public void PauseVFX(bool stop)
    {
        if (stop)
        {
            if (morganVFXGraphs[0].GetComponent<VisualEffect>().pause == false)
            {
                foreach (VisualEffect vf in morganVFXGraphs)
                {
                    vf.GetComponent<VisualEffect>().pause = true;
                }
                Debug.Log("Morgan VFX Graph paused");
            }
        }
        else if (morganVFXGraphs[0].GetComponent<VisualEffect>().pause == true)
        {
            foreach (VisualEffect vf in morganVFXGraphs)
            {
                vf.GetComponent<VisualEffect>().pause = false;
            }
            Debug.Log("Morgan VFX Graph un-paused");
        }
    }

    //Advances one frame the VFX Graphs
    public void StepForwardVFX()
    {
        pauseVFX = true;
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            vf.GetComponent<VisualEffect>().AdvanceOneFrame();
        }
    }

    //Sets the playrate for the Graphs (the game time stays the same)
    private void SetPlayRate()
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            vf.playRate = playRate;
        }
    }

    //Regenerates the render textures used by the VFX Graph
    public void RegenerateRenderTextures()
    {
        var str = FindObjectOfType<SpawnTexturesRenderer>();
        if(str != null)
            str.RefreshGeneratedData();
    }
    #endregion Debug Settings

    #region Set variables on the VFX Graphs

    //Sets a Float exposed parameters
    public void VfSetFloat(string parameterName, float floatValue)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasFloat(parameterName)) vf.SetFloat(parameterName, floatValue);
        }
    }

    //Sets an Integer exposed parameters
    public void VfSetInt(string parameterName, int intValue)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasInt(parameterName)) vf.SetInt(parameterName, intValue);
        }
    }

    //Sets a Vector2 exposed parameters
    public void VfSetV2(string parameterName, Vector2 v2Value)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasVector2(parameterName)) vf.SetVector2(parameterName, v2Value);
        }
    }

    //Sets a Vector3 exposed parameters
    public void VfSetV3(string parameterName, Vector3 v3Value)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasVector3(parameterName)) vf.SetVector3(parameterName, v3Value);
        }
    }

    //Sets a Vector4 exposed parameters
    public void VfSetV4(string parameterName, Vector4 v4Value)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasVector4(parameterName)) vf.SetVector4(parameterName, v4Value);
        }
    }

    //Sets a 2D Texture exposed parameters
    public void VfSetTexture(string parameterName, Texture2D texture)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasTexture(parameterName)) vf.SetTexture(parameterName, texture);
        }
    }

    //Sets a Boolean exposed parameters
    public void VfSetBool(string parameterName, bool boolValue)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasBool(parameterName)) vf.SetBool(parameterName, boolValue);
        }
    }

    //Sets an Animation Curve exposed parameters
    public void VfSetCurve(string parameterName, AnimationCurve animCurve)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasAnimationCurve(parameterName)) vf.SetAnimationCurve(parameterName, animCurve);
        }
    }

    //Sets a Gradient exposed parameters
    public void VfSetGradient(string parameterName, Gradient gradient)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasGradient(parameterName)) vf.SetGradient(parameterName, gradient);
        }
    }

    //Sets a Mesh exposed parameters
    public void VfSetMesh(string parameterName, Mesh mesh)
    {
        foreach (VisualEffect vf in morganVFXGraphs)
        {
            if (vf.HasMesh(parameterName)) vf.SetMesh(parameterName, mesh);
        }
    }
    #endregion Set variables on the VFX Graphs

    void Update()
    {
    #if UNITY_EDITOR
        if (skipVFXUpdates)
            return;
    #endif
        Morph();

        SetPlayRate();
        SetMorphingPeelVariables();
        SetPullbackVariables();

        SetHelmetCalmVariables();
        SetHelmetAngryVariables();

        SetFaceCalmVariables();
        SetFaceAngryVariables();
        SetFaceDetailVariables();

        SetEyesVariables();

        SetBodyCalmVariables();
        SetBodyAngryVariables();

        SetClothCalmVariables();
        SetClothAngryVariables();

        SetFireEffectVariables();
        SetCrumbleVariables();
        DebugProperties();
    }
}
