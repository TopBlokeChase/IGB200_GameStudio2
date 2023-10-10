using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusParticleEffect : MonoBehaviour
{
    public void StartParticles(float duration)
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particleSystem.main;

        main.duration = duration;
        GetComponent<ParticleSystem>().Play();
    }
}
