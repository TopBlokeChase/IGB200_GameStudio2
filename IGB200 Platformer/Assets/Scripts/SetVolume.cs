using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SetVolume : MonoBehaviour
{   
    public string exposedParamKey;
    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(exposedParamKey, Mathf.Log10(sliderValue) * 20);
        if (exposedParamKey == "MasterVol")
        {
            VolumeTracker.masterSliderValue  = sliderValue;
        }

        if (exposedParamKey == "MusicVol")
        {
            VolumeTracker.musicSliderValue = sliderValue;
        }
    }
}
