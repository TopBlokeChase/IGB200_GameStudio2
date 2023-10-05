using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSounds : MonoBehaviour
{
    //This script is to be used by the animator - hence only public functions here & no start/update
    [Header("Sounds")]
    [SerializeField] private AudioSource laughSoundSource;
    [SerializeField] private AudioSource hurtSoundSource;
    [SerializeField] private AudioSource slamAttackSlamSoundSource;
    [SerializeField] private AudioSource slamAttackFlyDownSoundSource;
    [SerializeField] private AudioSource laserBurstSoundSource;
    [SerializeField] private AudioSource laserLoopSoundSource;
    [SerializeField] private AudioSource flyingDisSoundSource;
    [SerializeField] private List<AudioClip> hurtSounds = new List<AudioClip>();


    public void PlayLaugh()
    {
        laughSoundSource.Play();
    }

    public void PlayHurt()
    {
        hurtSoundSource.clip = (PickSound(hurtSounds));
        hurtSoundSource.Play();
    }

    public void PlaySlamAttackSlam()
    {
        slamAttackSlamSoundSource.Play();
    }

    public void PlaySlamAttackFlyDown()
    {
        slamAttackFlyDownSoundSource.Play();
    }

    public void PlayLaserBurst()
    {
        laserBurstSoundSource.Play();
    }

    public void PlayLaserLoop()
    {
        laserLoopSoundSource.Play();
    }

    public void StopLaserLoop()
    {
        laserLoopSoundSource.Stop();
    }

    public void PlayFlyingDisc()
    {
        flyingDisSoundSource.Play();
    }

    private AudioClip PickSound(List<AudioClip> audioClips)
    {
        int randomNumber = Random.Range(0, audioClips.Count);

        return audioClips[randomNumber];
    }
}
