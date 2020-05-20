using UnityEngine;
using UnityEditor;
using UnityEngine.VFX;

[CustomEditor(typeof(Morgan))]
public class MorganEditor : Editor
{
    protected static bool showMorph = true;
    protected static bool showMorphSettings, showFireSettings, showCrumbleSettings, showLookCustomizationSettings, showPullbackSettings, showHelmetCalmCustomizationSettings, showHelmetAngryCustomizationSettings, showFaceCalmCustomization, showFaceAngryCustomization, showFaceDetailCustomization, showEyesCustomization, showBodyCalmCustomization, showBodyAngryCustomization, showClothCalmCustomization, showClothAngryCustomization, showStatsSettings, showDebugSettings = false;

    const int SpaceA = 30, SpaceB = 10;

    GUIStyle centeredLabelStyle, foldoutStyle, subFoldoutStyle;

    void EnsureStyles()
    {
        if (centeredLabelStyle == null)
        {
            centeredLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            centeredLabelStyle.alignment = TextAnchor.UpperCenter;

            foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;
            foldoutStyle.fontSize = 14;

            subFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            subFoldoutStyle.fontStyle = FontStyle.Bold;
            subFoldoutStyle.fontSize = 12;
        }
    }
    
    public override void OnInspectorGUI()
    {
        EnsureStyles();
        
        Morgan morgan = (Morgan)target;
        
        GUILayout.Space(SpaceA);

        EditorGUI.BeginChangeCheck();
        
        showMorph = EditorGUILayout.Foldout(showMorph, "Morph", foldoutStyle);
        if (showMorph)
        {
            //Morph Intensity Slider
            EditorGUILayout.PropertyField(serializedObject.FindProperty("morph"), new GUIContent("Morph VFX"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("morphBody"), new GUIContent("Morph Body"));

            GUILayout.Space(SpaceB);
            showMorphSettings = EditorGUILayout.Foldout(showMorphSettings, "Morph Settings", subFoldoutStyle);
            if (showMorphSettings)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_interval"), new GUIContent("Morphing Debris Interval"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_rate"), new GUIContent("Morphing Debris Rate"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_size_min"), new GUIContent("Morphing Debris Size Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_size_max"), new GUIContent("Morphing Debris Size Max"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_balance"), new GUIContent("Morphing Debris Calm/Angry %"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_emissive_gradient"), new GUIContent("Morphing Debris Emissive Gradient"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_emissive_intensity"), new GUIContent("Morphing Debris Emissive Intensity"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_emissive_velocity_contribution"), new GUIContent("Morphing Debris Emissive Velocity Contribution"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_emissive_velocity_min"), new GUIContent("Morphing Debris Emissive Velocity Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_morph_debris_emissive_velocity_max"), new GUIContent("Morphing Debris Emissive Velocity Max"));

                GUILayout.Space(SpaceB);

                //Face Noise Morphing
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_morph_noise"), new GUIContent("Face Morph Noise Intensity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_morph_noise_speed"), new GUIContent("Face Morph Noise Speed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_morph_noise_frequency"), new GUIContent("Face Morph Noise Frequency"));

                GUILayout.Space(SpaceB);

                //Morph Emissive Mode
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_emissive_mode"), new GUIContent("Face Morph Emissive Mode"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_emissive_intensity"), new GUIContent("Face Morph Emissive Intensity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_emissive_width_min"), new GUIContent("Face Morph Emissive Width Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_emissive_width_max"), new GUIContent("Face Morph Emissive Width Max"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_emissive_gradient"), new GUIContent("Face Morph Emissive Gradient"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_emissive_flicker"), new GUIContent("Face Morph Emissive Flicker"));

                GUILayout.Space(SpaceB);

                //Morph Peel
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_intensity"), new GUIContent("Morph Peel Influence"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_frequency"), new GUIContent("Morph Peel Frequency"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_octaves"), new GUIContent("Morph Peel Octaves"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_roughness"), new GUIContent("Morph Peel Roughness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_new_range_min"), new GUIContent("Morph Peel New Range Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_new_range_max"), new GUIContent("Morph Peel New Range Max"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_power"), new GUIContent("Morph Peel Power"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_multiply"), new GUIContent("Morph Peel Multiply"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_peel_period"), new GUIContent("Morph Peel A Period"));

                GUILayout.Space(SpaceB);

                //Morph: if it should be driven by the slider or by the blend shape value
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morph_aggression"), new GUIContent("Morph Aggression"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("morphDrivenByBlendShape"), new GUIContent("Body Morphs From Blendshapes"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("reverse_morph"), new GUIContent("Reverse Morph"));
            }

            GUILayout.Space(SpaceA);
        }

        showLookCustomizationSettings = EditorGUILayout.Foldout(showLookCustomizationSettings, "Appearance", foldoutStyle);
        if (showLookCustomizationSettings)
        {
            //Pullback Setting
            showPullbackSettings = EditorGUILayout.Foldout(showPullbackSettings, "Pullback Settings", subFoldoutStyle);
            if (showPullbackSettings)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_speed_multiplier"), new GUIContent("Pullback Speed Multiplier"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_mask_influence"), new GUIContent("Pullback Noise Mask Influence"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_distance_old_min"), new GUIContent("Pullback Distance Old Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_distance_old_max"), new GUIContent("Pullback Distance Old Max"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_distance_new_min"), new GUIContent("Pullback Distance New Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_distance_new_max"), new GUIContent("Pullback Distance New Max"));

                GUILayout.Space(SpaceB);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_a_min"), new GUIContent("Pullback Noise A Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_a_max"), new GUIContent("Pullback Noise A Max"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_b_min"), new GUIContent("Pullback Noise B Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_b_max"), new GUIContent("Pullback Noise B Max"));

                GUILayout.Space(SpaceB);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_mask_speed"), new GUIContent("Pullback Noise Mask Speed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_mask_frequency"), new GUIContent("Pullback Noise Mask Frequency"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_mask_range_min"), new GUIContent("Pullback Noise Mask Range Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("pullback_noise_mask_range_max"), new GUIContent("Pullback Noise Mask Range Max"));

                GUILayout.Space(SpaceB);
            }

            //Customize Helmet
            showHelmetCalmCustomizationSettings = EditorGUILayout.Foldout(showHelmetCalmCustomizationSettings, "Customize Helmet Calm", subFoldoutStyle);
            if (showHelmetCalmCustomizationSettings)
            {
                EditorGUILayout.LabelField("~ Helmet Calm Transform ~", centeredLabelStyle);

                //Helmet Rotation
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_rotation_x_shape_a"), new GUIContent("Helmet Calm Rot X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_rotation_y_shape_a"), new GUIContent("Helmet Calm Rot Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_rotation_z_shape_a"), new GUIContent("Helmet Calm Rot Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_top_rotation_x_shape_a"), new GUIContent("Helmet Calm Top Rot X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_top_rotation_y_shape_a"), new GUIContent("Helmet Calm Top Rot Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_top_rotation_z_shape_a"), new GUIContent("Helmet Calm Top Rot Z"));
                GUILayout.Space(SpaceB);

                //Helmet Calm Look
                EditorGUILayout.LabelField("~ Helmet Calm Look ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_mesh"), new GUIContent("Helmet Calm Mesh"));
                if (morgan.helmet_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.helmet_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_size_multiplier"), new GUIContent("Helmet Calm Size Multiplier"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_base_color_map"), new GUIContent("Helmet Calm Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_base_color"), new GUIContent("Helmet Calm Base Color"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_mask_map"), new GUIContent("Helmet Calm Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_smoothness"), new GUIContent("Helmet Calm Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_metallic"), new GUIContent("Helmet Calm Metallic"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_normal_map"), new GUIContent("Helmet Calm Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_normal_scale"), new GUIContent("Helmet Calm Normal Scale"));
                GUILayout.Space(SpaceB);

                //Helmet Calm Detail Look
                EditorGUILayout.LabelField("~ Helmet Calm Detail Look ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_mesh"), new GUIContent("Helmet Calm Detail Mesh"));
                if (morgan.helmet_detail_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.helmet_detail_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_size_multiplier"), new GUIContent("Helmet Calm Detail Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_base_color_map"), new GUIContent("Helmet Calm Detail Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_base_color"), new GUIContent("Helmet Calm Detail Base Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_mask_map"), new GUIContent("Helmet Calm Detail Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_smoothness"), new GUIContent("Helmet Calm Detail Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_metallic"), new GUIContent("Helmet Calm Detail Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_normal_map"), new GUIContent("Helmet Calm Detail Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_detail_normal_scale"), new GUIContent("Helmet Calm Detail Normal Scale"));
                GUILayout.Space(SpaceB);
            }

            showHelmetAngryCustomizationSettings = EditorGUILayout.Foldout(showHelmetAngryCustomizationSettings, "Customize Helmet Angry", subFoldoutStyle);
            if (showHelmetAngryCustomizationSettings)
            {
                //Helmet Angry Look
                EditorGUILayout.LabelField("~ Helmet Angry Look ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_mesh"), new GUIContent("Helmet Angry Mesh"));
                if (morgan.helmet_angry_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.helmet_angry_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_size_multiplier"), new GUIContent("Helmet Angry Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_base_color_map"), new GUIContent("Helmet Angry Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_base_color"), new GUIContent("Helmet Angry Base Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_mask_map"), new GUIContent("Helmet Angry Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_smoothness"), new GUIContent("Helmet Angry Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_metallic"), new GUIContent("Helmet Angry Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_normal_map"), new GUIContent("Helmet Angry Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("helmet_angry_normal_scale"), new GUIContent("Helmet Angry Normal Scale"));
                GUILayout.Space(SpaceB);
            }


            //Customize Face Calm
            showFaceCalmCustomization = EditorGUILayout.Foldout(showFaceCalmCustomization, "Customize Face Calm", subFoldoutStyle);
            if (showFaceCalmCustomization)
            {
                //Face Calm Look
                EditorGUILayout.LabelField("~ Face Calm ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_mesh"), new GUIContent("Face Calm Mesh"));
                if (morgan.face_calm_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.face_calm_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_size_multiplier"), new GUIContent("Face Calm Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_base_color_map"), new GUIContent("Face Calm Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_color"), new GUIContent("Face Calm Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_emissive_map"), new GUIContent("Face Calm Emissive Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_emissive_scale"), new GUIContent("Face Calm Emissive Scale"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_mask_map"), new GUIContent("Face Calm Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_smoothness"), new GUIContent("Face Calm Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_metallic"), new GUIContent("Face Calm Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_normal_map"), new GUIContent("Face Calm Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_calm_normal_scale"), new GUIContent("Face Calm Normal Scale"));

                GUILayout.Space(SpaceB);
            }

            //Customize Face Angry
            showFaceAngryCustomization = EditorGUILayout.Foldout(showFaceAngryCustomization, "Customize Face Angry", subFoldoutStyle);
            if (showFaceAngryCustomization)
            {
                //Face Angry Look
                EditorGUILayout.LabelField("~ Face Angry ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_mesh"), new GUIContent("Face Angry Mesh"));
                if (morgan.face_angry_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.face_angry_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_mesh_mirrored"), new GUIContent("Face Angry Mesh Mirrored"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_size_multiplier"), new GUIContent("Face Angry Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_base_color_map"), new GUIContent("Face Angry Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_color"), new GUIContent("Face Angry Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_mask_map"), new GUIContent("Face Angry Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_smoothness"), new GUIContent("Face Angry Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_metallic"), new GUIContent("Face Angry Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_normal_map"), new GUIContent("Face Angry Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_angry_normal_scale"), new GUIContent("Face Angry Normal Scale"));

                GUILayout.Space(SpaceB);
            }

            //Customize Face Details
            showFaceDetailCustomization = EditorGUILayout.Foldout(showFaceDetailCustomization, "Customize Face Detail", subFoldoutStyle);
            if (showFaceDetailCustomization)
            {
                //Face Detail Look
                EditorGUILayout.LabelField("~ Face Detail - used for both Calm and Angry states ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_mesh"), new GUIContent("Face Detail Mesh"));
                if (morgan.face_detail_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.face_detail_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_size_multiplier"), new GUIContent("Face Detail Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_base_color_map"), new GUIContent("Face Detail Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_color"), new GUIContent("Face Detail Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_mask_map"), new GUIContent("Face Detail Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_smoothness"), new GUIContent("Face Detail Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_metallic"), new GUIContent("Face Detail Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_normal_map"), new GUIContent("Face Detail Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("face_detail_normal_scale"), new GUIContent("Face Detail Normal Scale"));

                GUILayout.Space(SpaceB);
            }

            //Customize Eyes
            showEyesCustomization = EditorGUILayout.Foldout(showEyesCustomization, "Customize Eyes", subFoldoutStyle);
            if (showEyesCustomization)
            {
                //Face Eyes Look
                EditorGUILayout.LabelField("~ Eyes ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("eyes_mesh"), new GUIContent("Eyes Mesh"));
                if (morgan.eyes_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.eyes_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("eyes_size_multiplier"), new GUIContent("Eyes Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("eyes_calm_angry_color_gradient"), new GUIContent("Eyes Calm / Angry Gradient"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("eyesLightCalmAngryColorGradient"), new GUIContent("Eyes Light Calm / Angry Gradient"));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("eyesCalmLumes"), new GUIContent("Eyes Light Calm Lumens"));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("eyesAngryLumens"), new GUIContent("Eyes Light Angry Lumens"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("eyesCalmRange"), new GUIContent("Eyes Light Calm Range"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("eyesAngryRange"), new GUIContent("Eyes Light Angry Range"));
                GUILayout.Space(SpaceB);
            }

            //Customize Body Calm
            showBodyCalmCustomization = EditorGUILayout.Foldout(showBodyCalmCustomization, "Customize Body Calm", subFoldoutStyle);
            if (showBodyCalmCustomization)
            {
                //Body Calm Transform
                EditorGUILayout.LabelField("~ Body Calm Transform ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_anim_pattern_intensity"), new GUIContent("Body Calm Anim Pattern Intensity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_anim_pattern_period"), new GUIContent("Body Calm Anim Pattern Period"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_scales"), new GUIContent("Body Calm Scales Intensity"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_red_x"), new GUIContent("Body Calm Detail Pivot AB Red X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_red_y"), new GUIContent("Body Calm Detail Pivot AB Red Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_red_z"), new GUIContent("Body Calm Detail Pivot AB Red Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_green_x"), new GUIContent("Body Calm Detail Pivot AB Green X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_green_y"), new GUIContent("Body Calm Detail Pivot AB Green Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_green_z"), new GUIContent("Body Calm Detail Pivot AB Green Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_blue_x"), new GUIContent("Body Calm Detail Pivot AB Blue X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_blue_y"), new GUIContent("Body Calm Detail Pivot AB Blue Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_pivot_ab_blue_z"), new GUIContent("Body Calm Detail Pivot AB Blue Z"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_red_x"), new GUIContent("Body Calm Pivot AB Red X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_red_y"), new GUIContent("Body Calm Pivot AB Red Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_red_z"), new GUIContent("Body Calm Pivot AB Red Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_green_x"), new GUIContent("Body Calm Pivot AB Green X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_green_y"), new GUIContent("Body Calm Pivot AB Green Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_green_z"), new GUIContent("Body Calm Pivot AB Green Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_blue_x"), new GUIContent("Body Calm Pivot AB Blue X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_blue_y"), new GUIContent("Body Calm Pivot AB Blue Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_pivot_ab_blue_z"), new GUIContent("Body Calm Pivot AB Blue Z"));


                //Body Calm Look
                EditorGUILayout.LabelField("~ Body Calm ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_mesh"), new GUIContent("Body Calm Mesh"));
                if (morgan.body_calm_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.body_calm_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_size_multiplier"), new GUIContent("Body Calm Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_base_color_map"), new GUIContent("Body Calm Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_color"), new GUIContent("Body Calm Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_mask_map"), new GUIContent("Body Calm Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_smoothness"), new GUIContent("Body Calm Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_metallic"), new GUIContent("Body Calm Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_normal_map"), new GUIContent("Body Calm Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_normal_scale"), new GUIContent("Body Calm Normal Scale"));
                GUILayout.Space(SpaceB);

                //Body Calm Detail Look
                EditorGUILayout.LabelField("~ Body Calm Detail ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_mesh"), new GUIContent("Body Calm Detail Mesh"));
                if (morgan.body_calm_detail_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.body_calm_detail_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_size_multiplier"), new GUIContent("Body Calm Detail Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_base_color_map"), new GUIContent("Body Calm Detail Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_color"), new GUIContent("Body Calm Detail Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_mask_map"), new GUIContent("Body Calm Detail Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_smoothness"), new GUIContent("Body Calm Detail Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_metallic"), new GUIContent("Body Calm Detail Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_normal_map"), new GUIContent("Body Calm Detail Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_calm_detail_normal_scale"), new GUIContent("Body Calm Detail Normal Scale"));

                GUILayout.Space(SpaceB);
            }

            //Customize Body Angry
            showBodyAngryCustomization = EditorGUILayout.Foldout(showBodyAngryCustomization, "Customize Body Angry", subFoldoutStyle);
            if (showBodyAngryCustomization)
            {
                //Body Angry Transform
                EditorGUILayout.LabelField("~ Body Angry Transform ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_anim_pattern_period"), new GUIContent("Body Angry Anim Pattern Period"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_anim_pattern_intensity"), new GUIContent("Body Angry Anim Pattern Intensity"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_scales"), new GUIContent("Body Angry Scales Intensity"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_red_x"), new GUIContent("Body Angry Detail Pivot AB Red X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_red_y"), new GUIContent("Body Angry Detail Pivot AB Red Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_red_z"), new GUIContent("Body Angry Detail Pivot AB Red Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_green_x"), new GUIContent("Body Angry Detail Pivot AB Green X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_green_y"), new GUIContent("Body Angry Detail Pivot AB Green Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_green_z"), new GUIContent("Body Angry Detail Pivot AB Green Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_blue_x"), new GUIContent("Body Angry Detail Pivot AB Blue X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_blue_y"), new GUIContent("Body Angry Detail Pivot AB Blue Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_pivot_ab_blue_z"), new GUIContent("Body Angry Detail Pivot AB Blue Z"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_red_x"), new GUIContent("Body Angry Pivot AB Red X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_red_y"), new GUIContent("Body Angry Pivot AB Red Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_red_z"), new GUIContent("Body Angry Pivot AB Red Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_green_x"), new GUIContent("Body Angry Pivot AB Green X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_green_y"), new GUIContent("Body Angry Pivot AB Green Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_green_z"), new GUIContent("Body Angry Pivot AB Green Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_blue_x"), new GUIContent("Body Angry Pivot AB Blue X"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_blue_y"), new GUIContent("Body Angry Pivot AB Blue Y"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_pivot_ab_blue_z"), new GUIContent("Body Angry Pivot AB Blue Z"));

                //Body Angry Look
                EditorGUILayout.LabelField("~ Body Angry ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_mesh"), new GUIContent("Body Angry Mesh"));
                if (morgan.body_angry_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.body_angry_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_size_multiplier"), new GUIContent("Body Angry Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_base_color_map"), new GUIContent("Body Angry Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_color"), new GUIContent("Body Angry Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_mask_map"), new GUIContent("Body Angry Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_smoothness"), new GUIContent("Body Angry Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_metallic"), new GUIContent("Body Angry Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_normal_map"), new GUIContent("Body Angry Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_normal_scale"), new GUIContent("Body Angry Normal Scale"));
                GUILayout.Space(SpaceB);

                //Body Angry Detail Look
                EditorGUILayout.LabelField("~ Body Angry Detail ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_mesh"), new GUIContent("Body Angry Detail Mesh"));
                if (morgan.body_angry_detail_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.body_angry_detail_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_size_multiplier"), new GUIContent("Body Angry Detail Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_base_color_map"), new GUIContent("Body Angry Detail Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_color"), new GUIContent("Body Angry Detail Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_mask_map"), new GUIContent("Body Angry Detail Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_smoothness"), new GUIContent("Body Angry Detail Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_metallic"), new GUIContent("Body Angry Detail Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_normal_map"), new GUIContent("Body Angry Detail Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("body_angry_detail_normal_scale"), new GUIContent("Body Angry Detail Normal Scale"));

                GUILayout.Space(SpaceB);
            }

            //Cloth Calm
            showClothCalmCustomization = EditorGUILayout.Foldout(showClothCalmCustomization, "Customize Cloth Calm", subFoldoutStyle);
            if (showClothCalmCustomization)
            {
                //Cloth Calm Look
                EditorGUILayout.LabelField("~ Cloth Calm ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_mesh"), new GUIContent("Cloth Calm Mesh"));
                if (morgan.cloth_calm_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.cloth_calm_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_size_multiplier"), new GUIContent("Cloth Calm Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_base_color_map"), new GUIContent("Cloth Calm Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_color"), new GUIContent("Cloth Calm Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_mask_map"), new GUIContent("Cloth Calm Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_smoothness"), new GUIContent("Cloth Calm Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_metallic"), new GUIContent("Cloth Calm Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_normal_map"), new GUIContent("Cloth Calm Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_normal_scale"), new GUIContent("Cloth Calm Normal Scale"));

                GUILayout.Space(SpaceB);

                //Cloth Calm Detail A 
                EditorGUILayout.LabelField("~ Cloth Calm Detail A ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_mesh"), new GUIContent("Cloth Calm Detail A Mesh"));
                if (morgan.cloth_calm_detail_a_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.cloth_calm_detail_a_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_size_multiplier"), new GUIContent("Cloth Calm Detail A Size Multiplier"));

                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_base_color_map"), new GUIContent("Cloth Calm Detail A Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_color"), new GUIContent("Cloth Calm Detail A Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_mask_map"), new GUIContent("Cloth Calm Detail A Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_smoothness"), new GUIContent("Cloth Calm Detail A Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_metallic"), new GUIContent("Cloth Calm Detail A Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_normal_map"), new GUIContent("Cloth Calm Detail A Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_a_normal_scale"), new GUIContent("Cloth Calm Detail A Normal Scale"));

                GUILayout.Space(SpaceB);

                //Cloth Calm Detail B 
                EditorGUILayout.LabelField("~ Cloth Calm Detail B ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_mesh"), new GUIContent("Cloth Calm Detail B Mesh"));
                if (morgan.cloth_calm_detail_b_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.cloth_calm_detail_b_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_size_multiplier"), new GUIContent("Cloth Calm Detail B Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_base_color_map"), new GUIContent("Cloth Calm Detail B Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_color"), new GUIContent("Cloth Calm Detail B Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_mask_map"), new GUIContent("Cloth Calm Detail B Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_smoothness"), new GUIContent("Cloth Calm Detail B Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_metallic"), new GUIContent("Cloth Calm Detail B Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_normal_map"), new GUIContent("Cloth Calm Detail B Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_calm_detail_b_normal_scale"), new GUIContent("Cloth Calm Detail B Normal Scale"));

                GUILayout.Space(SpaceB);
            }

            //Cloth Angry
            showClothAngryCustomization = EditorGUILayout.Foldout(showClothAngryCustomization, "Customize Cloth Angry", subFoldoutStyle);
            if (showClothAngryCustomization)
            {
                //Cloth Angry Look
                EditorGUILayout.LabelField("~ Cloth Angry ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_mesh"), new GUIContent("Cloth Angry Mesh"));
                if (morgan.cloth_angry_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.cloth_angry_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_size_multiplier"), new GUIContent("Cloth Angry Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_base_color_map"), new GUIContent("Cloth Angry Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_color"), new GUIContent("Cloth Angry Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_mask_map"), new GUIContent("Cloth Angry Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_smoothness"), new GUIContent("Cloth Angry Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_metallic"), new GUIContent("Cloth Angry Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_normal_map"), new GUIContent("Cloth Angry Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_normal_scale"), new GUIContent("Cloth Angry Normal Scale"));

                GUILayout.Space(SpaceB);

                //Cloth Angry Detail A 
                EditorGUILayout.LabelField("~ Cloth Angry Detail A ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_mesh"), new GUIContent("Cloth Angry Detail A Mesh"));
                if (morgan.cloth_angry_detail_a_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.cloth_angry_detail_a_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_size_multiplier"), new GUIContent("Cloth Angry Detail A Size Multiplier"));

                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_base_color_map"), new GUIContent("Cloth Angry Detail A Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_color"), new GUIContent("Cloth Angry Detail A Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_mask_map"), new GUIContent("Cloth Angry Detail A Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_smoothness"), new GUIContent("Cloth Angry Detail A Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_metallic"), new GUIContent("Cloth Angry Detail A Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_normal_map"), new GUIContent("Cloth Angry Detail A Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_a_normal_scale"), new GUIContent("Cloth Angry Detail A Normal Scale"));

                GUILayout.Space(SpaceB);

                //Cloth Angry Detail B 
                EditorGUILayout.LabelField("~ Cloth Angry Detail B ~", centeredLabelStyle);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_mesh"), new GUIContent("Cloth Angry Detail B Mesh"));
                if (morgan.cloth_angry_detail_b_mesh) EditorGUILayout.LabelField("Mesh vertex count:", morgan.cloth_angry_detail_b_mesh.vertexCount.ToString("n0"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_size_multiplier"), new GUIContent("Cloth Angry Detail B Size Multiplier"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_base_color_map"), new GUIContent("Cloth Angry Detail B Base Color Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_color"), new GUIContent("Cloth Angry Detail B Color"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_mask_map"), new GUIContent("Cloth Angry Detail B Mask Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_smoothness"), new GUIContent("Cloth Angry Detail B Smoothness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_metallic"), new GUIContent("Cloth Angry Detail B Metallic"));
                GUILayout.Space(SpaceB);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_normal_map"), new GUIContent("Cloth Angry Detail B Normal Map"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cloth_angry_detail_b_normal_scale"), new GUIContent("Cloth Angry Detail B Normal Scale"));
                GUILayout.Space(SpaceB);
            }
            GUILayout.Space(SpaceA);
        }

        showFireSettings = EditorGUILayout.Foldout(showFireSettings, "Fire", foldoutStyle);
        if (showFireSettings)
        {

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fireMode"), new GUIContent("Fire Mode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_amount"), new GUIContent("Fire Amount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_direction"), new GUIContent("Fire Direction"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_outwards_force"), new GUIContent("Fire Outwards Force"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_contribution_face"), new GUIContent("Fire Contribution Face"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_contribution_helmet"), new GUIContent("Fire Contribution Helmet"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_contribution_eyes"), new GUIContent("Fire Contribution Eyes"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_contribution_cloth"), new GUIContent("Fire Contribution Cloth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_contribution_body"), new GUIContent("Fire Contribution Body"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_intensity_min"), new GUIContent("Fire Intensity Min"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_intensity_max"), new GUIContent("Fire Intensity Max"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_intensity_noise_frequency"), new GUIContent("Fire Intensity Noise Frequency"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_mesh"), new GUIContent("Fire Lit Mesh"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_blur_scale"), new GUIContent("Fire Blur Amount"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_distortion_scale"), new GUIContent("Fire Distortion Amount"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_size_min"), new GUIContent("Fire Size Min"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_size_max"), new GUIContent("Fire Size Max"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_emissive_use_velocity_for_emissive"), new GUIContent("Fire Use Velocity For Emissive"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_emissive_intensity"), new GUIContent("Fire Emissive Intensity"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_morph_curve"), new GUIContent("Fire Morph Color Curve"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_diffuse_gradient_calm"), new GUIContent("Fire Diffuse Gradient Calm"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_diffuse_gradient_angry"), new GUIContent("Fire Diffuse Gradient Angry"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_emissive_gradient_calm"), new GUIContent("Fire Emissive Gradient Calm"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_emissive_gradient_angry"), new GUIContent("Fire Emissive Gradient Angry"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_multiply_size_over_lifetime"), new GUIContent("Fire Multiply Size Over Lifetime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_speed"), new GUIContent("Fire Turbulence Speed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_scale"), new GUIContent("Fire Turbulence Scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_intensity"), new GUIContent("Fire Turbulence Intensity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_frequency"), new GUIContent("Fire Turbulence Frequency"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_drag"), new GUIContent("Fire Turbulence Drag"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_octaves"), new GUIContent("Fire Turbulence Octaves"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire_turbulence_roughness"), new GUIContent("Fire Turbulence Roughness"));

            GUILayout.Space(SpaceA);
        }

        showCrumbleSettings = EditorGUILayout.Foldout(showCrumbleSettings, "Crumble", foldoutStyle);
        if (showCrumbleSettings)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shockwave_origin"), new GUIContent("Shockwave & Crumble Origin"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("shockwave"), new GUIContent("Shockwave"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shockwave_debris"), new GUIContent("Shockwave Debris"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shockwave_debris_outwards_force"), new GUIContent("Shockwave Debris Outwards Force"));
  
            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble"), new GUIContent("Crumble A"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_b"), new GUIContent("Crumble B"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_debris"), new GUIContent("Crumble Debris"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_debris_outwards_force"), new GUIContent("Crumble Debris Outwards Force"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_force_min"), new GUIContent("Crumble Force Min"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_force_max"), new GUIContent("Crumble Force Max"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_angular_velocity_min"), new GUIContent("Crumble Angular Velocity Min"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_angular_velocity_max"), new GUIContent("Crumble Angular Velocity Max"));

            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_intensity"), new GUIContent("Crumble Turbulence Intensity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_scale"), new GUIContent("Crumble Turbulence Scale"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_speed"), new GUIContent("Crumble Turbulence Speed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_drag"), new GUIContent("Crumble Turbulence Drag"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_frequency"), new GUIContent("Crumble Turbulence Frequency"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_octaves"), new GUIContent("Crumble Turbulence Octaves"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("crumble_turbulence_roughness"), new GUIContent("Crumble Turbulence Roughness"));

            GUILayout.Space(SpaceA);
        }

        showStatsSettings = EditorGUILayout.Foldout(showStatsSettings, "Stats", foldoutStyle);
        if (showStatsSettings)
        {
            EditorGUILayout.LabelField("~Alive Particle Count~", centeredLabelStyle);

            EditorGUILayout.LabelField("Helmet Calm:", morgan.morganVFXGraphs[8].aliveParticleCount.ToString("n0"));
            EditorGUILayout.LabelField("Helmet Angry A:", morgan.morganVFXGraphs[11].aliveParticleCount.ToString("n0"));
            EditorGUILayout.LabelField("Helmet Angry B:", morgan.morganVFXGraphs[12].aliveParticleCount.ToString("n0"));

            EditorGUILayout.LabelField("Face Calm:", morgan.morganVFXGraphs[1].aliveParticleCount.ToString("n0"));
            EditorGUILayout.LabelField("Face Angry:", morgan.morganVFXGraphs[2].aliveParticleCount.ToString("n0"));
            EditorGUILayout.LabelField("Face Detail:", morgan.morganVFXGraphs[3].aliveParticleCount.ToString("n0"));
            EditorGUILayout.LabelField("Eyes:", morgan.morganVFXGraphs[4].aliveParticleCount.ToString("n0"));

            EditorGUILayout.LabelField("Body Calm:", morgan.morganVFXGraphs[0].aliveParticleCount.ToString("n0"));
            EditorGUILayout.LabelField("Body Angry:", morgan.morganVFXGraphs[5].aliveParticleCount.ToString("n0"));

            EditorGUILayout.LabelField("Cloth:", morgan.morganVFXGraphs[14].aliveParticleCount.ToString("n0"));

            EditorGUILayout.LabelField("Fire:", morgan.morganVFXGraphs[13].aliveParticleCount.ToString("n0"));

            GUILayout.Space(SpaceB);

            int totalAlivePartices = 0;

            foreach (VisualEffect vf in morgan.morganVFXGraphs)
            {
                totalAlivePartices += vf.aliveParticleCount;
            }

            EditorGUILayout.LabelField("TOTAL:", totalAlivePartices.ToString("n0"));

            GUILayout.Space(SpaceA);
        }

        showDebugSettings = EditorGUILayout.Foldout(showDebugSettings, "Debug", foldoutStyle);
        if (showDebugSettings)
        {
            //Display debug masks
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debugProp"), new GUIContent("Debug Properties"));
            GUILayout.Space(SpaceB);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("debug_color_a"), new GUIContent("Debug Color A"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debug_color_b"), new GUIContent("Debug Color B"));
            GUILayout.Space(SpaceA);

            //Play Rate
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playRate"), new GUIContent("Play Rate"));
            GUILayout.BeginHorizontal();

            //Play & Stop the VFX Graph
            morgan.pauseVFX = GUILayout.Toggle(morgan.pauseVFX, "||", "Button");
            morgan.PauseVFX(morgan.pauseVFX);

            //Advances on frame the VFX Graph
            if (GUILayout.Button(">|")) morgan.StepForwardVFX();

            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceA);

            //Displays Morgan as solid mesh
            if (GUILayout.Button("Show Morgan As Mesh")) morgan.MorganAsMesh(false);
            GUILayout.Space(SpaceB);

            //Displays Morgan Vertex Colors
            if (GUILayout.Button("Show Morgan Vertex Colors")) morgan.MorganAsMesh(true);

            GUILayout.Space(SpaceA);

            //Helmet
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Helmet As Mesh")) morgan.ShowHelmetAsMesh(true, false);
            if (GUILayout.Button("Helmet Vertex Colors")) morgan.ShowHelmetAsMesh(true, true);
            if (GUILayout.Button("Helmet As VFX")) morgan.ShowHelmetAsMesh(false, false);
            GUILayout.EndHorizontal();

            //Face
            GUILayout.Space(SpaceB);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Face As Mesh")) morgan.ShowFaceAsMesh(true, false);
            if (GUILayout.Button("Face Vertex Colors")) morgan.ShowFaceAsMesh(true, true);
            if (GUILayout.Button("Face As VFX")) morgan.ShowFaceAsMesh(false, false);
            GUILayout.EndHorizontal();

            //Body
            GUILayout.Space(SpaceB);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Body As Mesh")) morgan.ShowBodyAsMesh(true, false);
            if (GUILayout.Button("Body Vertex Colors")) morgan.ShowBodyAsMesh(true, true);
            if (GUILayout.Button("Body As VFX")) morgan.ShowBodyAsMesh(false, false);
            GUILayout.EndHorizontal();

            //Cloth
            GUILayout.Space(SpaceB);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Cloth As Mesh")) morgan.ShowClothAsMesh(true, false);
            if (GUILayout.Button("Cloth Vertex Colors")) morgan.ShowClothAsMesh(true, true);
            if (GUILayout.Button("Cloth As VFX")) morgan.ShowClothAsMesh(false, false);

            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceA);

            //Disables the Helmet VFX inside the Morgan VFX Graph
            morgan.disableHelmetVFX = GUILayout.Toggle(morgan.disableHelmetVFX, "Disable Helmet VFX", "Button");
            morgan.DisableVFX(1, morgan.disableHelmetVFX);
            GUILayout.Space(SpaceB);

            //Disables the Face VFX inside the Morgan VFX Graph
            morgan.disableFaceVFX = GUILayout.Toggle(morgan.disableFaceVFX, "Disable Face VFX", "Button");
            morgan.DisableVFX(2, morgan.disableFaceVFX);
            GUILayout.Space(SpaceB);

            //Disables the Body VFX inside the Morgan VFX Graph
            morgan.disableBodyVFX = GUILayout.Toggle(morgan.disableBodyVFX, "Disable Body VFX", "Button");
            morgan.DisableVFX(3, morgan.disableBodyVFX);
            GUILayout.Space(SpaceB);

            //Disables the Cloth VFX inside the Morgan VFX Graph
            morgan.disableClothVFX = GUILayout.Toggle(morgan.disableClothVFX, "Disable Cloth VFX", "Button");
            morgan.DisableVFX(5, morgan.disableClothVFX);
            GUILayout.Space(SpaceA);
        }

        GUILayout.Space(SpaceA);

        if (GUILayout.Button("R E S E T"))
        {
            morgan.Reset();
            morgan.disableBodyVFX = false;
            morgan.disableFaceVFX = false;
            morgan.disableHairVFX = false;
            morgan.disableClothVFX = false;
            morgan.disableHelmetVFX = false;
            morgan.disableVFXGraph = false;
            morgan.pauseVFX = false;
        }

        GUILayout.Space(SpaceB);

        //Recompiles the VFX Graph
        if (GUILayout.Button("Reinitialize VFX Graphs")) morgan.ReinitVFXGraphs();
        GUILayout.Space(SpaceB);

        //Regeneretes the render textures
        if (GUILayout.Button("Regenerate Render Textures")) morgan.RegenerateRenderTextures();

        GUILayout.Space(SpaceA*2);

        EditorGUILayout.LabelField("~Do not modify below~", centeredLabelStyle);

        GUILayout.Space(SpaceB);

        if(EditorGUI.EndChangeCheck())
            EditorApplication.QueuePlayerLoopUpdate();

        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
