using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSounds : MonoBehaviour
{
    //This script is to be used by the animator - hence only public functions here & no start/update
    [Header("Sounds")]
    [SerializeField] private AudioSource laughSoundSource;
    [SerializeField] private AudioSource hurtSoundSource;
    [SerializeField] private AudioSource defeatSoundSource;
    [SerializeField] private AudioSource slamAttackSlamSoundSource;
    [SerializeField] private AudioSource slamAttackWhooshSoundSource;
    [SerializeField] private AudioSource slamAttackFlyDownSoundSource;
    [SerializeField] private AudioSource laserBurstSoundSource;
    [SerializeField] private AudioSource laserLoopSoundSource;
    [SerializeField] private AudioSource hotFloorRumbleSoundSource;
    [SerializeField] private AudioSource hotFloorLavaBubbleSoundSource;
    [SerializeField] private AudioSource hotFloorPlatformAppearSoundSource;
    [SerializeField] private AudioSource hotFloorPlatformDisappearSoundSource;
    [SerializeField] private AudioSource wordAttackSoundSource;
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

    public void PlaySlamAttackWhoosh()
    {
        slamAttackWhooshSoundSource.Play();
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

    public void PlayHotFloorRumble()
    {
        hotFloorRumbleSoundSource.Play();
    }

    public void StopHotFloorRumble()
    {
        hotFloorRumbleSoundSource.Stop();
    }

    public void PlayHotFloorBubble()
    {
        hotFloorLavaBubbleSoundSource.Play();
    }

    public void StopHotFloorBubble()
    {
        hotFloorLavaBubbleSoundSource.Stop();
    }

    public void PlayPlatformAppear()
    {
        hotFloorPlatformAppearSoundSource.Play();
    }

    public void PlayPlatformDisappear()
    {
        hotFloorPlatformDisappearSoundSource.Play();
    }

    public void PlayWordAttack()
    {
        wordAttackSoundSource.Play();
    }

    public void PlayDefeat()
    {
        defeatSoundSource.Play();
        laughSoundSource.Stop();
        hotFloorLavaBubbleSoundSource.Stop();
        hotFloorRumbleSoundSource.Stop();
    }

    public void StopAllSounds(bool playDefeat)
    {
        laughSoundSource.Stop();
        hurtSoundSource.Stop();

        if (playDefeat)
        {
            defeatSoundSource.Play();
        }
        else
        {
            defeatSoundSource.Stop();
        }
        slamAttackSlamSoundSource.Stop();
        slamAttackWhooshSoundSource.Stop();
        slamAttackFlyDownSoundSource.Stop();
        laserBurstSoundSource.Stop();
        laserLoopSoundSource.Stop();
        hotFloorRumbleSoundSource.Stop();
        hotFloorLavaBubbleSoundSource.Stop();
        hotFloorPlatformAppearSoundSource.Stop();
        hotFloorPlatformDisappearSoundSource.Stop();
        wordAttackSoundSource.Stop();
        flyingDisSoundSource.Stop();
    }

    private AudioClip PickSound(List<AudioClip> audioClips)
    {
        int randomNumber = Random.Range(0, audioClips.Count);

        return audioClips[randomNumber];
    }
}
