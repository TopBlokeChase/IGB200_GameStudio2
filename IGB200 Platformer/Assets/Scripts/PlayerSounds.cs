using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    //This script is to be used by the animator - hence only public functions here & no start/update
    [Header("Sounds")]
    [SerializeField] private AudioSource footstepSoundSource;
    [SerializeField] private AudioSource meleeAttackSoundSource;
    [SerializeField] private AudioSource meleeAttackWhooshSoundSource;
    [SerializeField] private AudioSource meleeAttackHitGroundSoundSource;
    [SerializeField] private AudioSource hurtSoundSource;
    [SerializeField] private AudioSource hitFlyingDiscSoundSource;
    [SerializeField] private AudioSource slowedSoundSource;
    [SerializeField] private AudioSource slowedEndSoundSource;
    [SerializeField] private AudioSource jumpGruntSoundSource;
    [SerializeField] private AudioSource nailGunAimSoundSource;
    [SerializeField] private AudioSource nailGunFireSoundSource;
    [SerializeField] private List<AudioClip> footstepSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> meleeAttackSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> meleeAttackHitGroundSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> hurtSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> jumpGruntSounds = new List<AudioClip>();

    public void PlayFootstep()
    {
        footstepSoundSource.clip = (PickSound(footstepSounds));
        footstepSoundSource.Play();
    }

    public void PlayHurt()
    {
        hurtSoundSource.clip = (PickSound(hurtSounds));
        hurtSoundSource.Play();
    }

    public void PlayHitFlyingDisc()
    {
        hitFlyingDiscSoundSource.Play();
    }

    public void PlaySlowedBegin(float time)
    {
        slowedSoundSource.Play();
        StartCoroutine(PlaySoundAfterDelay(time, slowedEndSoundSource));
    }

    public void PlaySlowedEnd()
    {
    }

    public void PlayJumpGrunt()
    {
        jumpGruntSoundSource.clip = (PickSound(jumpGruntSounds));
        jumpGruntSoundSource.Play();
    }

    public void PlayAttackGrunt()
    {
        meleeAttackSoundSource.clip = (PickSound(meleeAttackSounds));
        meleeAttackSoundSource.Play();
    }

    public void PlayAttackWhoosh()
    {
        meleeAttackWhooshSoundSource.Play();
    }

    public void PlayAttackHitGround()
    {
        meleeAttackHitGroundSoundSource.clip = (PickSound(meleeAttackHitGroundSounds));
        meleeAttackHitGroundSoundSource.Play();
    }

    public void PlayNailGunAim()
    {
        nailGunAimSoundSource.Play();
    }

    public void PlayNailGunFire()
    {
        nailGunFireSoundSource.Play();
    }

    private AudioClip PickSound(List<AudioClip> audioClips)
    {
        int randomNumber = Random.Range(0, audioClips.Count);

        return audioClips[randomNumber];
    }

    IEnumerator PlaySoundAfterDelay(float time, AudioSource source)
    {
        yield return new WaitForSeconds(time);
        source.Play();
    }
}
