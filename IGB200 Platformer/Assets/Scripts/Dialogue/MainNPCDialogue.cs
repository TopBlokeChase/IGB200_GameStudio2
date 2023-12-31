using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class MainNPCDialogue : MonoBehaviour
{
    public enum Level
    {
        One,
        Two,
        Three
    }

    [SerializeField] private Level level;
    [SerializeField] private GameObject victoryFanfareText;
    [SerializeField] private GameObject entryGate;
    public TMP_Text dialogueText;
    public GameObject mainDialoguePanel;

    public GameObject playerImagePanel;
    public GameObject npcImagePanel;

    public TMP_Text playerName;
    public TMP_Text npcName;

    public Image playerImage;
    public Image npcImage;

    [SerializeField] private float frameSizeWhenTalking;
    [SerializeField] private Color frameColorWhenNotTalking;


    [System.Serializable]
    public struct dialogueNode
    {
        [TextArea]
        public string dialogue;
        public bool isPlayerDialogue;
        public bool isLevelFinishedDialogue;
    }

    [SerializeField] private bool receivedIntroDialogue;
    [SerializeField] private bool receivedBossDefeatDialogue;

    [SerializeField] private bool hasDefeatedBoss;

    [SerializeField] private List<dialogueNode> introDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> afterIntroDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> bossDefeatDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> afterBossDefeatDialogue = new List<dialogueNode>();

    [TextArea]
    [SerializeField] private List<string> sideNPCDialogueBeforeBossMEN = new List<string>();
    [TextArea]
    [SerializeField] private List<string> sideNPCDialogueAfterBossMEN = new List<string>();
    [TextArea]
    [SerializeField] private List<string> sideNPCDialogueBeforeBossWOMEN = new List<string>();
    [TextArea]
    [SerializeField] private List<string> sideNPCDialogueAfterBossWOMEN = new List<string>();

    private bool readFirstDialogueNode;
    private int dialogueCounter = 0;
    private List<dialogueNode> dialogueToRead;

    private MainNPCInteract mainNPCInteract;
    private PlayerMovement playerMovement;

    private RectTransform choicePanelRectTransform;

    private bool isDelaying;
    private bool needsDelay;
    private bool isInDialogue;
    private bool isRevealingText;

    private bool playerCompletedSite;

    private Vector3 initialFrameScale = new Vector3(1, 1, 1);
    private Color initialFrameColor = new Color(255, 255, 255, 255);


    // Start is called before the first frame update
    void Start()
    {
        mainNPCInteract = this.gameObject.GetComponent<MainNPCInteract>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
            if (isInDialogue)
            {
                if (playerMovement.isInMenu == false)
                {
                    CheckInput();
                }
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
            entryGate.GetComponent<Gate>().EnableGateTrigger();
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
                    dialogueText.GetComponent<TeleType>().RevealText(dialogueToRead[i].dialogue.Length);

                    if (dialogueToRead[i].isLevelFinishedDialogue)
                    {
                        playerCompletedSite = true;
                    }

                    if (dialogueToRead[i].isPlayerDialogue)
                    {
                        // make player's UI frame bigger/highlighted && decrease NPC frame size
                        playerImagePanel.GetComponent<RectTransform>().localScale = new Vector3(frameSizeWhenTalking, frameSizeWhenTalking, frameSizeWhenTalking);
                        npcImagePanel.GetComponent<RectTransform>().localScale = initialFrameScale;
                        playerImage.color = initialFrameColor;
                        npcImage.color = frameColorWhenNotTalking;
                        playerName.color = Color.white;
                        npcName.color = frameColorWhenNotTalking;
                    }
                    else
                    {
                        // make NPC's UI frame bigger/highlighted && decrease player frame size
                        playerImagePanel.GetComponent<RectTransform>().localScale = initialFrameScale;
                        npcImagePanel.GetComponent<RectTransform>().localScale = new Vector3(frameSizeWhenTalking, frameSizeWhenTalking, frameSizeWhenTalking);
                        playerImage.color = frameColorWhenNotTalking;
                        npcImage.color = initialFrameColor;
                        playerName.color = frameColorWhenNotTalking;
                        npcName.color = Color.white;
                    }
                }              
            }
        }
    }

    public void InitiateDialogue()
    {
        playerMovement.canUseTools = false;

        if(playerMovement.gameObject.TryGetComponent<LadderPlayer_NEW>(out LadderPlayer_NEW ladder))
        {
            ladder.StopPlacementMode();
        }

        playerMovement.gameObject.GetComponent<LadderPlayer_NEW>().StopPlacementMode();
        isInDialogue = true;
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

    public bool GetBossDefeated()
    {
        return hasDefeatedBoss;
    }

    public string GetSideDialogueToDisplay(bool isFemale)
    {
        if (hasDefeatedBoss)
        {
            if (isFemale)
            {
                int randNum = Random.Range(0, sideNPCDialogueAfterBossWOMEN.Count);
                return sideNPCDialogueAfterBossWOMEN[randNum];
            }
            else
            {
                int randNum = Random.Range(0, sideNPCDialogueAfterBossMEN.Count);
                return sideNPCDialogueAfterBossMEN[randNum];
            }
        }
        else
        {
            if (isFemale)
            {
                int randNum = Random.Range(0, sideNPCDialogueBeforeBossWOMEN.Count);
                return sideNPCDialogueBeforeBossWOMEN[randNum];
            }
            else
            {
                int randNum = Random.Range(0, sideNPCDialogueBeforeBossMEN.Count);
                return sideNPCDialogueBeforeBossMEN[randNum];
            }
        }
    }

    private void ExitDialogue()
    {
        playerMovement.canUseTools = true;
        mainDialoguePanel.SetActive(false);
        dialogueCounter = 0;
        readFirstDialogueNode = false;
        playerMovement.isInteracting = false;
        mainNPCInteract.HasStoppedInteracting();
        needsDelay = false;
        isDelaying = false;
        isInDialogue = false;

        if (playerCompletedSite)
        {
            victoryFanfareText.SetActive(true);

            if (level == Level.One)
            {
                ProgressTracker.currentLevel = 1;
                ProgressTracker.hasPassedLevel1 = true;
            }

            if (level == Level.Two)
            {
                ProgressTracker.currentLevel = 2;
                ProgressTracker.hasPassedLevel2 = true;
            }

            if (level == Level.Three)
            {
                ProgressTracker.currentLevel = 3;
                ProgressTracker.hasPassedLevel3 = true;
            }
        }
    }

    IEnumerator DelayInput()
    {
        isDelaying = true;
        yield return new WaitForSeconds(1);
        needsDelay = false;
        isDelaying = false;
    }
}
