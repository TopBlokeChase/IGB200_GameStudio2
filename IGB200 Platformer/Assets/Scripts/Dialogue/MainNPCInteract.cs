using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNPCInteract : MonoBehaviour
{
    public GameObject interactPanelUI;


    private MainNPCDialogue mainNPCDialogue;
    private bool isReadyToInteract;
    private bool isInteracting;

    private bool isDelaying;
    private bool needsDelay;

    private void Start()
    {
        mainNPCDialogue = this.gameObject.GetComponent<MainNPCDialogue>();
    }

    private void Update()
    {
        Interact();
    }

    private void Interact()
    {
        if (isReadyToInteract && !isInteracting)
        {
            if (needsDelay)
            {
                if (!isDelaying)
                {
                    StartCoroutine(DelayInput());
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))          
                {
                    isInteracting = true;
                    interactPanelUI.SetActive(false);
                    mainNPCDialogue.InitiateDialogue();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactPanelUI.SetActive(true);
            isReadyToInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactPanelUI.SetActive(false);
            isReadyToInteract = false;
        }
    }

    public void HasStoppedInteracting()
    {
        isInteracting = false;
        interactPanelUI.SetActive(true);
        needsDelay = true;
        isDelaying = false;
    }

    IEnumerator DelayInput()
    {
        isDelaying = true;
        yield return new WaitForSeconds(1);
        needsDelay = false;
        isDelaying = false;
    }
}
