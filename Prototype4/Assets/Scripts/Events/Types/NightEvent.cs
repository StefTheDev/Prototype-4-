using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NightEvent : Event
{
    public PostProcessVolume volume;
    private ColorGrading colorGrading;
    private MotionBlur motionBlur;

    /*
     * TODO:
     * 
     * 
     * 
     */

    public override void OnStart()
    {
        colorGrading = volume.profile.GetSetting<ColorGrading>();
        motionBlur = volume.profile.GetSetting<MotionBlur>();
    }

    public override void OnEnd() {

    }

    private void Update()
    {
        if (colorGrading != null)
        {
            if (colorGrading.temperature.value >= -60.0f) colorGrading.temperature.value -= Time.deltaTime * 16;
            if (colorGrading.postExposure.value > 0.0f) colorGrading.postExposure.value -= Time.deltaTime / 2;
        }
    }
}
