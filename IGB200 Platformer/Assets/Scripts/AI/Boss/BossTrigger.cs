using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossGameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bossGameObject.GetComponent<BossDialogue>().InitiateDialogue();
            collision.gameObject.GetComponent<LadderPlayer_NEW>().StopPlacementMode();
            collision.GetComponent<PlayerMovement>().canUseTools = false;
        }
    }

    public void ActivateBossTrigger()
    {
        bossGameObject.GetComponent<Enemy>().InitiateBossFight(gameObject);
        gameObject.SetActive(false);
    }
}
