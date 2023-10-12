using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField] private bool isHoriztonal;
    [SerializeField] private AudioSource glassShatterSoundSource;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject glassShatterParticle;
    [SerializeField] private float glassShatterVelocity = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShatterGlass(bool isLeftTrigger)
    {
        GameObject particleEffectObj = Instantiate(glassShatterParticle, transform.position, transform.rotation);
        ParticleSystem particleEffect = particleEffectObj.GetComponent<ParticleSystem>();
        ParticleSystem.VelocityOverLifetimeModule velModule = particleEffect.velocityOverLifetime;
        ParticleSystem.ForceOverLifetimeModule forceModule = particleEffect.forceOverLifetime;

        if (isLeftTrigger)
        {
            if (!isHoriztonal)
            {
                velModule.xMultiplier = glassShatterVelocity;
            }
            else
            {
                velModule.xMultiplier = glassShatterVelocity;
                forceModule.y = 0f;
                forceModule.x = -10f;
            }
        }
        else
        {
            if (!isHoriztonal)
            {
                velModule.xMultiplier = -glassShatterVelocity;
            }
            else
            {
                velModule.xMultiplier = -glassShatterVelocity;
                forceModule.y = 0f;
                forceModule.x = -10f;
            }
        }
        sprite.SetActive(false);
        glassShatterSoundSource.Play();
    }
}
