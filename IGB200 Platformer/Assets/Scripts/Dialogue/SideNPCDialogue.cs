using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class SideNPCDialogue : MonoBehaviour
{
    public enum Level
    {
        One,
        Two,
        Three
    }

    [SerializeField] private Level level;
    [SerializeField] private Material levelOneParticleMaterial;
    [SerializeField] private Material levelTwoParticleMaterial;
    [SerializeField] private Material levelThreeParticleMaterial;
    [SerializeField] private bool activateParticleSwirl;
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

        if (level == Level.One)
        {
            ParticleSystemRenderer rend = sideNPCParticleSwirl.GetComponent<ParticleSystemRenderer>();
            rend.material = levelOneParticleMaterial;
        }

        if (level == Level.Two)
        {
            ParticleSystemRenderer rend = sideNPCParticleSwirl.GetComponent<ParticleSystemRenderer>();
            rend.material = levelTwoParticleMaterial;
        }

        if (level == Level.Three)
        {
            ParticleSystemRenderer rend = sideNPCParticleSwirl.GetComponent<ParticleSystemRenderer>();
            rend.material = levelThreeParticleMaterial;
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

    public Collider2D ReturnBoxCollider()
    {
        return triggerCollider;
    }

    private void GetDialogueToDisplay()
    {
            speechText.text = mainNPCDialogue.GetSideDialogueToDisplay(isFemale);
    }

    public void SetNotEffected()
    {
        spriteObject.GetComponent<SpriteRenderer>().sprite = spriteNotEffected;
        sideNPCParticleSwirl.gameObject.SetActive(false);
    }
}
