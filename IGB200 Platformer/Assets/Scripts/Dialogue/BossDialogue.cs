using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossDialogue : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject mainDialoguePanel;

    public GameObject playerImagePanel;
    public GameObject bossImagePanel;

    public TMP_Text playerName;
    public TMP_Text bossName;

    public Image playerImage;
    public Image bossImage;

    [SerializeField] private float frameSizeWhenTalking;
    [SerializeField] private Color frameColorWhenNotTalking;


    [System.Serializable]
    public struct dialogueNode
    {
        [TextArea]
        public string dialogue;
        public bool isPlayerDialogue;
        public bool bossLeavesAtThisPoint;
    }

    [SerializeField] private bool hasDefeatedBoss;

    [SerializeField] private List<dialogueNode> introDialogue = new List<dialogueNode>();
    [SerializeField] private List<dialogueNode> bossDefeatDialogue = new List<dialogueNode>();

    private bool readFirstDialogueNode;
    private int dialogueCounter = 0;
    private List<dialogueNode> dialogueToRead;

    private PlayerMovement playerMovement;

    private RectTransform choicePanelRectTransform;

    private bool isDelaying;
    private bool needsDelay;
    private bool isInDialogue;

    private Vector3 initialFrameScale = new Vector3(1, 1, 1);
    private Color initialFrameColor = new Color(255, 255, 255, 255);


    // Start is called before the first frame update
    void Start()
    {
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
        if (hasDefeatedBoss)
        {
            dialogueToRead = bossDefeatDialogue;
        }
        else
        {
            dialogueToRead = introDialogue;
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

                    if (dialogueToRead[i].isPlayerDialogue)
                    {
                        // make player's UI frame bigger/highlighted && decrease NPC frame size
                        playerImagePanel.GetComponent<RectTransform>().localScale = new Vector3(frameSizeWhenTalking, frameSizeWhenTalking, frameSizeWhenTalking);
                        bossImagePanel.GetComponent<RectTransform>().localScale = initialFrameScale;
                        playerImage.color = initialFrameColor;
                        bossImage.color = frameColorWhenNotTalking;
                        playerName.color = Color.white;
                        bossName.color = frameColorWhenNotTalking;
                    }
                    else
                    {
                        // make NPC's UI frame bigger/highlighted && decrease player frame size
                        playerImagePanel.GetComponent<RectTransform>().localScale = initialFrameScale;
                        bossImagePanel.GetComponent<RectTransform>().localScale = new Vector3(frameSizeWhenTalking, frameSizeWhenTalking, frameSizeWhenTalking);
                        playerImage.color = frameColorWhenNotTalking;
                        bossImage.color = initialFrameColor;
                        playerName.color = frameColorWhenNotTalking;
                        bossName.color = Color.white;
                    }

                    if (dialogueToRead[i].bossLeavesAtThisPoint)
                    {
                        gameObject.GetComponent<Enemy>().BossLeave();
                        bossImagePanel.SetActive(false);
                        bossName.transform.parent.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void InitiateDialogue()
    {
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

    public void ResetBossDialogue()
    {

    }

    private void ExitDialogue()
    {
        mainDialoguePanel.SetActive(false);
        dialogueCounter = 0;
        readFirstDialogueNode = false;
        playerMovement.isInteracting = false;
        needsDelay = false;
        isDelaying = false;
        isInDialogue = false;

        if (hasDefeatedBoss)
        {
            // end the fight
            this.gameObject.GetComponent<Enemy>().EndBossFight();
        }
        else
        {
            //initiate the fight
            this.gameObject.transform.parent.GetComponentInChildren<BossTrigger>().ActivateBossTrigger();
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
