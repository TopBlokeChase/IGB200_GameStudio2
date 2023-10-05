using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject audioObject;
    [SerializeField] private AudioClip audioToPlay;
    public void InstantiateAudio()
    {
        GameObject audio = Instantiate(audioObject);
        audio.transform.parent = null;
        audio.GetComponent<AudioSource>().clip = audioToPlay;
        audio.GetComponent<AudioSource>().Play();
    }
}
