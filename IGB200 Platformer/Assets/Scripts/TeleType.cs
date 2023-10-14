using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//Created with the help of "Text Mesh Pro - Text Reveal Effect" tutorial on Youtube @ https://www.youtube.com/watch?v=U85gbZY6oo8
public class TeleType : MonoBehaviour
{
    [SerializeField] private float textSpeed = 0.5f; 
    [SerializeField] private TMP_Text textMeshPro;

    private bool hasFinished;

    IEnumerator RevelTextCoroutine(int textCount)
    {
        GetComponent<AudioSource>().Play();
        textMeshPro.ForceMeshUpdate();

        hasFinished = false;

        int totalVisCharacters = textCount;
        int counter = 0;
        int visibleCount = 0;
        
        while (!hasFinished)
        {
            visibleCount = counter % (totalVisCharacters + 1);
            textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisCharacters)
            {
                hasFinished = true;
                GetComponent<AudioSource>().Stop();
            }

            counter += 1;

            yield return new WaitForSeconds(textSpeed);
        }
    }

    public bool GetHasFinished()
    {
        return hasFinished;
    }

    public void RevealText(int textCount)
    {
        StartCoroutine(RevelTextCoroutine(textCount));
    }

    public void RevealAllEarly()
    {
        hasFinished = true;
        StopAllCoroutines();
        textMeshPro.maxVisibleCharacters = textMeshPro.textInfo.characterCount;
        GetComponent<AudioSource>().Stop();
    }
}
