using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
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
        if (isLeftTrigger)
        {
            velModule.xMultiplier = glassShatterVelocity;
        }
        else
        {
            velModule.xMultiplier = -glassShatterVelocity;
        }
        this.gameObject.SetActive(false);
    }
}
