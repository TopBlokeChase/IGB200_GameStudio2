using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class FinalCutscene : MonoBehaviour
{
    public bool isIntroSequence = false;
    public TMP_Text dialogueText;
    public GameObject mainDialoguePanel;
    [SerializeField] private GameObject babsCanvas;
    [SerializeField] private GameObject npcCanvas;
    [SerializeField] private GameObject bossCanvas;
    [SerializeField] private GameObject fadeOutCanvas;
    [SerializeField] private GameObject sceneBackgroundMusic;
    [SerializeField] private PlayerMovement playerMovement;

    [System.Serializable]
    public struct dialogueNode
    {
        [TextArea]
        public string dialogue;
        public bool showNpcs;
        public bool showBosses;
        public bool isFinalDialogue;
    }

    [SerializeField] private List<dialogueNode> finalCutsceneDialogue = new List<dialogueNode>();

    private bool readFirstDialogueNode;
    private int dialogueCounter = 0;
    private List<dialogueNode> dialogueToRead;

    private RectTransform choicePanelRectTransform;

    private bool isDelaying;
    private bool needsDelay;
    private bool isInDialogue;
    private bool isRevealingText;

    private bool readyToEnd;

    private bool hasInitiated;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement.isInMenu = true;
        playerMovement.isInteracting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasInitiated)
        {
            InitiateDialogue();
            hasInitiated = true;
            playerMovement.isInteracting = true;

            if (!isIntroSequence)
            {
                sceneBackgroundMusic.SetActive(false);
            }
        }

        if (needsDelay)
        {
            if (!isDelaying)
            {
                StartCoroutine(DelayInput());
            }
        }
        else
        {
            if (isInDialogue)
            {
                CheckInput();
            }
        }
    }

    private void CheckInput()
    {
        if (!readFirstDialogueNode)
        {
            dialogueCounter = 0;
            readFirstDialogueNode = true;
        }

        if (Input.anyKeyDown)
        {
            if (readyToEnd)
            {
                if (!isIntroSequence)
                {
                    fadeOutCanvas.SetActive(true);
                    StartCoroutine(DelayLoadScene());
                }
                else
                {
                    GameObject fadeCanvas = Instantiate(fadeOutCanvas);
                    fadeCanvas.SetActive(true);
                    StartCoroutine(DelayIntroScene());
                }
            }
            else
            {
                if (dialogueText.GetComponent<TeleType>().GetHasFinished())
                {
                    dialogueCounter++;
                    DisplayNewDialogue();
                }
                else
                {
                    dialogueText.GetComponent<TeleType>().RevealAllEarly();
                }
            }
        }
    }

    private void CollectDialogueChain()
    {
        dialogueToRead = finalCutsceneDialogue;
    }

    private void DisplayNewDialogue()
    {
        if (dialogueCounter == dialogueToRead.Count)
        {
            readyToEnd = true;
        }
        else
        {
            for (int i = 0; i < dialogueToRead.Count; i++)
            {
                if (i == dialogueCounter)
                {
                    dialogueText.text = dialogueToRead[i].dialogue;
                    dialogueText.GetComponent<TeleType>().RevealText(dialogueToRead[i].dialogue.Length);

                    if (dialogueToRead[i].showNpcs == true)
                    {
                        babsCanvas.GetComponent<Animator>().SetBool("fadeOut", true);
                        StartCoroutine(DelayAnimation(npcCanvas));
                    }

                    if (dialogueToRead[i].showBosses == true)
                    {
                        npcCanvas.GetComponent<Animator>().SetBool("fadeOut", true);
                        StartCoroutine(DelayAnimation(bossCanvas));
                    }
                }                             
            }
        }
    }

    public void InitiateDialogue()
    {
        isInDialogue = true;
        //needsDelay = true;
        //isDelaying = false;
        CollectDialogueChain();
        DisplayNewDialogue();
    }

    //public void SetBossDefeated()
    //{
    //    hasDefeatedBoss = true;
    //}

    //public bool GetBossDefeated()
    //{
    //    return hasDefeatedBoss;
    //}

    //public string GetSideDialogueToDisplay(bool isFemale)
    //{
    //    if (hasDefeatedBoss)
    //    {
    //        if (isFemale)
    //        {
    //            int randNum = Random.Range(0, sideNPCDialogueAfterBossWOMEN.Count);
    //            return sideNPCDialogueAfterBossWOMEN[randNum];
    //        }
    //        else
    //        {
    //            int randNum = Random.Range(0, sideNPCDialogueAfterBossMEN.Count);
    //            return sideNPCDialogueAfterBossMEN[randNum];
    //        }
    //    }
    //    else
    //    {
    //        if (isFemale)
    //        {
    //            int randNum = Random.Range(0, sideNPCDialogueBeforeBossWOMEN.Count);
    //            return sideNPCDialogueBeforeBossWOMEN[randNum];
    //        }
    //        else
    //        {
    //            int randNum = Random.Range(0, sideNPCDialogueBeforeBossMEN.Count);
    //            return sideNPCDialogueBeforeBossMEN[randNum];
    //        }
    //    }
    //}

    //private void ExitDialogue()
    //{
    //    mainDialoguePanel.SetActive(false);
    //    dialogueCounter = 0;
    //    readFirstDialogueNode = false;
    //    playerMovement.isInteracting = false;
    //    mainNPCInteract.HasStoppedInteracting();
    //    needsDelay = false;
    //    isDelaying = false;
    //    isInDialogue = false;

    //    if (playerCompletedSite)
    //    {
    //        victoryFanfareText.SetActive(true);
    //    }
    //}

    IEnumerator DelayInput()
    {
        isDelaying = true;
        yield return new WaitForSeconds(1);
        needsDelay = false;
        isDelaying = false;
    }

    IEnumerator DelayAnimation(GameObject canvas)
    {
        yield return new WaitForSeconds(1.5f);

        canvas.SetActive(true);
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }

    IEnumerator DelayIntroScene()
    {
        yield return new WaitForSeconds(1f);
        playerMovement.isInteracting = false;
        playerMovement.isInMenu = false;
        this.gameObject.SetActive(false);
    }   
}
