using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    //This script is to be used by the animator - hence only public functions here & no start/update
    [Header("Sounds")]
    [SerializeField] private AudioSource footstepSoundSource;
    [SerializeField] private AudioSource meleeAttackSoundSource;
    [SerializeField] private AudioSource hurtSoundSource;
    [SerializeField] private AudioSource jumpGruntSoundSource;
    [SerializeField] private AudioSource nailGunAimSoundSource;
    [SerializeField] private AudioSource nailGunFireSoundSource;
    [SerializeField] private List<AudioClip> footstepSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> meleeAttackSounds = new List<AudioClip>();
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
}
