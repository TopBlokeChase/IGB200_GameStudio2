using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : MonoBehaviour
{
    [SerializeField] private float saturationIntensity;
    [SerializeField] private float lensDistortionIntensity;
    [SerializeField] private float lensDistortionTime;

    private PostProcessVolume processVolume;
    private LensDistortion _LensDistortion;
    private ColorGrading _ColorGrading;

    private float timer;
    private float minDistortionValue;
    private float maxDistortionValue;

    // Start is called before the first frame update
    void Start()
    {
        processVolume = GetComponent<PostProcessVolume>();
        processVolume.profile.TryGetSettings(out _LensDistortion);
        processVolume.profile.TryGetSettings(out _ColorGrading);
        minDistortionValue = -lensDistortionIntensity;
        maxDistortionValue = lensDistortionIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        _ColorGrading.saturation.value = saturationIntensity;

        if (timer < lensDistortionTime)
        {
            _LensDistortion.intensity.value = Mathf.Lerp(minDistortionValue, maxDistortionValue, timer / lensDistortionTime);
            timer += Time.deltaTime;

        }
        else
        {
            timer = 0;
            float temp = maxDistortionValue;
            maxDistortionValue = minDistortionValue;
            minDistortionValue = temp;
        }
    }
}
