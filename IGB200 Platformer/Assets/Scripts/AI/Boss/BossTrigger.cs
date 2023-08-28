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
            bossGameObject.GetComponent<Enemy>().InitiateBossFight(gameObject);
            gameObject.SetActive(false);
        }
    }
}
