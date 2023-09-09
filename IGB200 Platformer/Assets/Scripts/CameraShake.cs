using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera vcam;

    private CinemachineBasicMultiChannelPerlin m_channels;

    private float duration;
    private float intensity;
    private float timer;
    private bool shake;
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        m_channels = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            timer += Time.deltaTime;
            if (timer < duration)
            {
                m_channels.m_AmplitudeGain = Mathf.Lerp(intensity, 0, timer / duration);
            }
            else
            {
                m_channels.m_AmplitudeGain = 0;
                shake = false;
            }
        }
    }

    public void ShakeCamera(float duration, float intensity)
    {
        shake = true;
        timer = 0;
        this.duration = duration;
        this.intensity = intensity;
    }
}
