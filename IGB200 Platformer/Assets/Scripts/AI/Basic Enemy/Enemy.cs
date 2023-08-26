using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject normalCamera;
    [SerializeField] private GameObject bossCamera;
    [SerializeField] private GameObject bossHealthPanel;

    
    [SerializeField] private Health health;
    [SerializeField] private GameObject linkedNPC;

    [TextArea]
    public string playerSpawnNote = "If this enemy is a boss, use an empty gameobject to determine player spawn on death/lose";
    [SerializeField] private GameObject playerSpawnPoint;


    
    private GameObject player;
    private GameObject bossTrigger;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void Die()
    {
        linkedNPC.GetComponent<MainNPCDialogue>().SetBossDefeated();
        EndBossFight();
        Destroy(this.gameObject);
    }

    private void EndBossFight()
    {
        //reset the camera and anything else needed before destroying this gameobject
        bossCamera.SetActive(false);
        player.GetComponentInChildren<PlayerCombat>().DisablePlayerHealthPanel();
    }

    public void InitiateBossFight(GameObject bossTrigger)
    {
        this.bossTrigger = bossTrigger;
        bossCamera.SetActive(true);
        bossHealthPanel.SetActive(true);

        // get component in children due to combat script being on sprite in player child
        player.GetComponentInChildren<PlayerCombat>().SetCurrentBoss(gameObject);
        player.GetComponentInChildren<PlayerCombat>().EnablePlayerHealthPanel();
    }

    public void ResetBossFight()
    {
        health.ResetHealth();
        bossCamera.SetActive(false);

        //reset boss state back to initial (when implemented)

        bossTrigger.SetActive(true);
        bossHealthPanel.SetActive(false);
        player.GetComponentInChildren<PlayerCombat>().DisablePlayerHealthPanel();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().AddForce(15);
            player.GetComponentInChildren<Health>().DealDamage(1);
        }
    }

    public Transform PlayerSpawnPos()
    {
        return playerSpawnPoint.transform;
    }
}
