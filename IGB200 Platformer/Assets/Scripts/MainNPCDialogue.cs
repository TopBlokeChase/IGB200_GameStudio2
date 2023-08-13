using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainNPCDialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text choiceText;
    public GameObject dialogueChoicePanel;
    public GameObject mainDialoguePanel;


    [System.Serializable]
    public struct dialogueNode
    {
        [TextArea]
        public string dialogue;
        public bool hasChoice;
        public string choiceText;
    }

    [SerializeField] private bool receivedIntroDialogue;
    [SerializeField] private bool receivedBossDefeatDialogue;

    [SerializeField] private bool hasDefeatedBoss;

    [SerializeField] private List<dialogueNode> introDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> afterIntroDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> bossDefeatDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> afterBossDefeatDialogue = new List<dialogueNode>();

    private bool readFirstDialogueNode;
    private int dialogueCounter = 0;
    private List<dialogueNode> dialogueToRead;

    private MainNPCInteract mainNPCInteract;
    private PlayerMovement playerMovement;

    private RectTransform choicePanelRectTransform;

    private bool isDelaying;
    private bool needsDelay;


    // Start is called before the first frame update
    void Start()
    {
        mainNPCInteract = this.gameObject.GetComponent<MainNPCInteract>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        choicePanelRectTransform = dialogueChoicePanel.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
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
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (readFirstDialogueNode)
            {
                dialogueCounter++;
                DisplayNewDialogue();
            }
            else
            {
                dialogueCounter = 0;
                readFirstDialogueNode = true;
            }
        }
    }

    private void CollectDialogueChain()
    {
        if (receivedIntroDialogue)
        {
            if (receivedBossDefeatDialogue)
            {
                dialogueToRead = afterBossDefeatDialogue;
                receivedBossDefeatDialogue = true;
            }
            else
            {
                if (hasDefeatedBoss)
                {
                    dialogueToRead = bossDefeatDialogue;
                    receivedBossDefeatDialogue = true;
                }
                else
                {
                    dialogueToRead = afterIntroDialogue;
                }
            }
        }
        else
        {
            dialogueToRead = introDialogue;
            receivedIntroDialogue = true;
        }      
    }

    private void DisplayNewDialogue()
    {
        if (dialogueCounter == dialogueToRead.Count)
        {
            ExitDialogue();
        }
        else
        {
            for (int i = 0; i < dialogueToRead.Count; i++)
            {
                if (i == dialogueCounter)
                {
                    dialogueText.text = dialogueToRead[i].dialogue;

                    if (dialogueToRead[i].hasChoice)
                    {
                        choiceText.text = dialogueToRead[i].choiceText;
                        dialogueChoicePanel.SetActive(true);
                        LayoutRebuilder.ForceRebuildLayoutImmediate(choicePanelRectTransform);
                    }
                    else
                    {
                        dialogueChoicePanel.SetActive(false);
                    }
                }
            }
        }
    }

    public void InitiateDialogue()
    {
        needsDelay = true;
        isDelaying = false;
        mainDialoguePanel.SetActive(true);
        playerMovement.isInteracting = true;
        CollectDialogueChain();
        DisplayNewDialogue();
    }

    public void SetBossDefeated()
    {
        hasDefeatedBoss = true;
    }

    private void ExitDialogue()
    {
        mainDialoguePanel.SetActive(false);
        dialogueCounter = 0;
        readFirstDialogueNode = false;
        playerMovement.isInteracting = false;
        mainNPCInteract.HasStoppedInteracting();
        needsDelay = false;
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
