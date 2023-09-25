using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class SideNPCDialogue : MonoBehaviour
{
    [SerializeField] private bool activateParticleSwirl;
    [SerializeField] private Material swirlingParticleMaterial;
    [SerializeField] private Sprite spriteEffected;
    [SerializeField] private Sprite spriteNotEffected;
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private bool isFemale;
    [SerializeField] private float displayTime;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TMP_Text speechText;
    [SerializeField] private GameObject linkedNPC;
    [SerializeField] private Collider2D triggerCollider;

    private MainNPCDialogue mainNPCDialogue;
    private ParticleSystem sideNPCParticleSwirl;

    private bool readyForAnotherBubble = true;
    private bool isShowing;

    private void Start()
    {
        sideNPCParticleSwirl = GetComponentInChildren<ParticleSystem>();
        mainNPCDialogue = linkedNPC.GetComponent<MainNPCDialogue>();
        spriteObject.GetComponent<SpriteRenderer>().sprite = spriteEffected;

        if (swirlingParticleMaterial != null)
        {
            ParticleSystemRenderer rend = sideNPCParticleSwirl.GetComponent<ParticleSystemRenderer>();
            rend.material = swirlingParticleMaterial;
        }

        if (activateParticleSwirl)
        {
            sideNPCParticleSwirl.gameObject.SetActive(true);
        }
        else
        {
            sideNPCParticleSwirl.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(ShowSpeechBubble());         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        readyForAnotherBubble = true;
    }

    IEnumerator ShowSpeechBubble()
    {
        if (readyForAnotherBubble && !isShowing)
        {
            readyForAnotherBubble = false;
            isShowing = true;
            speechBubble.SetActive(true);
            GetDialogueToDisplay();
            yield return new WaitForSeconds(displayTime);

            speechBubble.SetActive(false);
            isShowing = false;
        }
    }

    private void GetDialogueToDisplay()
    {
            speechText.text = mainNPCDialogue.GetSideDialogueToDisplay(isFemale);
    }

    public void SetNotEffected()
    {
        spriteObject.GetComponent<SpriteRenderer>().sprite = spriteNotEffected;
    }
}
