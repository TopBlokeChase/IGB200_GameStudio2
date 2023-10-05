using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource normal;
    [SerializeField] private AudioSource bossFight;

    public void PlayBossMusic()
    {
        normal.Stop();
        bossFight.Play();
    }

    public void PlayNormalMusic()
    {
        normal.Play();
        bossFight.Stop();
    }
}
