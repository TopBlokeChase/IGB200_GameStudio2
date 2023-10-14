using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderOnAwake : MonoBehaviour
{
    public string exposedParamKey;
    public Slider slider;
    private void Awake()
    {
        if (exposedParamKey == "MasterVol")
        {
            slider.value = VolumeTracker.masterSliderValue;
        }

        if (exposedParamKey == "MusicVol")
        {
            slider.value = VolumeTracker.musicSliderValue;
        }
    }
}
