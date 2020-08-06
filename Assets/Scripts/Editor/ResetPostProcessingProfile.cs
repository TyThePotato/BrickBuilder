using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEditor;

[InitializeOnLoad]
public static class ResetPostProcessingProfile
{
    public static PostProcessProfile profile;

    static ResetPostProcessingProfile() {
        profile = (PostProcessProfile)AssetDatabase.LoadAssetAtPath("Assets/BB PostProcessing Profile.asset", typeof(PostProcessProfile)); // ew hardcoded path gross
        EditorApplication.playModeStateChanged += ModeChanged;
    }

    static void ModeChanged (PlayModeStateChange playModeState) {
        if (playModeState == PlayModeStateChange.EnteredEditMode) {
            // set colorgrading to default values
            ColorGrading cg = profile.GetSetting<ColorGrading>();
            cg.mixerRedOutRedIn.value = 100f;
            cg.mixerRedOutGreenIn.value = 0f;
            cg.mixerRedOutBlueIn.value = 0f;
            cg.mixerGreenOutRedIn.value = 0f;
            cg.mixerGreenOutGreenIn.value = 100f;
            cg.mixerGreenOutBlueIn.value = 0f;
            cg.mixerBlueOutRedIn.value = 0f;
            cg.mixerBlueOutGreenIn.value = 0f;
            cg.mixerBlueOutBlueIn.value = 100f;
            cg.lift.value = new Vector4(1, 1, 1, 0);
        }
    }
}
