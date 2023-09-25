using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float bossLeaveSpeed;
    [SerializeField] private GameObject entryGate;
    [SerializeField] private MonoBehaviour bossScript;
    [SerializeField] private GameObject postProcessVolume;
    [SerializeField] private GameObject normalCamera;
    [SerializeField] private GameObject bossCamera;
    [SerializeField] private GameObject bossHealthPanel;

    
    [SerializeField] private Health health;
    [SerializeField] private GameObject linkedNPC;
    [SerializeField] private GameObject linkedSideNPCS;

    [TextArea]
    public string playerSpawnNote = "If this enemy is a boss, use an empty gameobject to determine player spawn on death/lose";
    [SerializeField] private GameObject playerSpawnPoint;


    
    private GameObject player;
    private GameObject bossTrigger;
    private Vector3 bossStartPosition;
    private Vector3 bossLeavePosition;
    private bool bossLeaving;

    // Start is called before the first frame update
    void Start()
    {
        bossStartPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        bossLeavePosition = transform.position + new Vector3(0, 20, 0);
    }

    private void Update()
    {
        if (bossLeaving)
        {
            transform.position = Vector3.MoveTowards(transform.position, bossLeavePosition, bossLeaveSpeed * Time.deltaTime);
        }
    }


    public void Die()
    {
        linkedNPC.GetComponent<MainNPCDialogue>().SetBossDefeated();
        this.gameObject.GetComponent<BossDialogue>().SetBossDefeated();
        this.gameObject.GetComponent<BossDialogue>().InitiateDialogue();
        DisableHealthUI();
        bossScript.Invoke("SetBusy", 0);
    }

    public void DisableHealthUI()
    {
        bossHealthPanel.SetActive(false);
        player.GetComponentInChildren<PlayerCombat>().DisablePlayerHealthPanel();
    }

    public void EndBossFight()
    {
        //reset the camera and anything else needed before destroying this gameobject
        entryGate.GetComponent<Gate>().OpenGate();
        bossCamera.SetActive(false);

        if (player.GetComponentInChildren<PlayerCombat>().HasNoteOfCourage())
        {
            player.GetComponentInChildren<PlayerCombat>().SetHasNoteOfCourage(false);
        }

        foreach (Transform child in linkedSideNPCS.transform)
        {
            if (child.gameObject.GetComponent<SideNPCDialogue>() != null)
            {
                child.gameObject.GetComponent<SideNPCDialogue>().SetNotEffected();
            }
        }

        this.transform.parent.gameObject.SetActive(false);
    }

    public void InitiateBossFight(GameObject bossTrigger)
    {
        entryGate.GetComponent<Gate>().CloseGate();
        bossScript.Invoke("ResetAll", 0);
        this.bossTrigger = bossTrigger;
        bossCamera.SetActive(true);
        health.ResetHealth();
        bossHealthPanel.SetActive(true);

        // get component in children due to combat script being on sprite in player child
        player.GetComponentInChildren<PlayerCombat>().SetCurrentBoss(gameObject);
        player.GetComponentInChildren<PlayerCombat>().EnablePlayerHealthPanel();
    }

    public void ResetBossFight()
    {
        entryGate.GetComponent<Gate>().OpenGate();
        bossScript.Invoke("ResetAll", 0);
        bossScript.enabled = false;
        transform.position = bossStartPosition;
        health.ResetHealth();
        bossCamera.SetActive(false);

        //reset boss state back to initial (when implemented)

        bossTrigger.SetActive(true);
        bossHealthPanel.SetActive(false);
        player.GetComponentInChildren<PlayerCombat>().DisablePlayerHealthPanel();
    }

    public void BossLeave()
    {
        bossLeaving = true;
        postProcessVolume.GetComponent<PostProcessHandler>().StopPostEffectAndDisable();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().AddForce(15, this.gameObject);
            player.GetComponentInChildren<Health>().DealDamage(1);
        }
    }

    public Transform PlayerSpawnPos()
    {
        return playerSpawnPoint.transform;
    }
}
