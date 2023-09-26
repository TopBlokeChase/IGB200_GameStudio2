using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessHandler : MonoBehaviour
{
    [SerializeField] private float enterExitEffectTime;
    [SerializeField] private float enterExitEffectIntensityMultiplier;
    [SerializeField] private float saturationIntensity;
    [SerializeField] private float lensDistortionIntensity;
    [SerializeField] private float lensDistortionTime;
    [SerializeField] private float bloomLerpTime;

    private PostProcessVolume processVolume;
    private LensDistortion _LensDistortion;
    private ColorGrading _ColorGrading;
    private Bloom _Bloom;

    private float timer;
    private float enterExitTimer;

    private float minDistortionValue;
    private float maxDistortionValue;

    // Start is called before the first frame update
    void Start()
    {
        processVolume = GetComponent<PostProcessVolume>();
        processVolume.profile.TryGetSettings(out _LensDistortion);
        processVolume.profile.TryGetSettings(out _ColorGrading);
        processVolume.profile.TryGetSettings(out _Bloom);
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

        if (enterExitTimer < enterExitEffectTime)
        {

        }
    }

    public void StartEnterEffect()
    {
        StartCoroutine(EnterEffect());
    }

    public void StartExitEffect()
    {
        StartCoroutine(ExitEffect());
    }
    public void StartBloomEffect(float amount)
    {
        StartCoroutine(ActivateBloom(amount));
    }

    public void StopBloomEffect(float amount)
    {
        StartCoroutine(DeactivateBloom(amount));
    }

    public void StopPostEffectAndDisable()
    {
        StartCoroutine(DisableEffect());
    }

    IEnumerator EnterEffect()
    {
        float timer = 0;

        float intensity = saturationIntensity * enterExitEffectIntensityMultiplier;

        while (timer < enterExitEffectTime)
        {
            _ColorGrading.saturation.value = Mathf.Lerp(saturationIntensity, intensity, timer / enterExitEffectTime);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < enterExitEffectTime)
        {
            _ColorGrading.saturation.value = Mathf.Lerp(intensity, saturationIntensity, timer / enterExitEffectTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    IEnumerator ExitEffect()
    {
        float timer = 0;

        float intensity = saturationIntensity * enterExitEffectIntensityMultiplier;

        while (timer < enterExitEffectTime)
        {
            _ColorGrading.saturation.value = Mathf.Lerp(saturationIntensity, intensity, timer / enterExitEffectTime);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < enterExitEffectTime)
        {
            _ColorGrading.saturation.value = Mathf.Lerp(intensity, 0, timer / enterExitEffectTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    IEnumerator DisableEffect()
    {
        float timer = 0;

        float intensity = saturationIntensity * enterExitEffectIntensityMultiplier;

        while (timer < enterExitEffectTime)
        {
            _ColorGrading.saturation.value = Mathf.Lerp(saturationIntensity, intensity, timer / enterExitEffectTime);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < enterExitEffectTime)
        {
            _ColorGrading.saturation.value = Mathf.Lerp(intensity, 0, timer / enterExitEffectTime);
            processVolume.weight = Mathf.Lerp(1, 0, timer / enterExitEffectTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    IEnumerator ActivateBloom(float amount)
    {
        float timer = 0;

        while (timer < bloomLerpTime)
        {
            _Bloom.intensity.value = Mathf.Lerp(0, amount, timer / bloomLerpTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DeactivateBloom(float amount)
    {
        float timer = 0;

        while (timer < bloomLerpTime)
        {
            _Bloom.intensity.value = Mathf.Lerp(amount, 0, timer / bloomLerpTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
