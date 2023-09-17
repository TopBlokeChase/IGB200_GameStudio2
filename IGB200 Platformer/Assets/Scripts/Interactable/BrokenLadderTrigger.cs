using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLadderTrigger : MonoBehaviour
{
    [SerializeField] private GameObject brokenLadderUI;
    [SerializeField] private GameObject brokenLadderShootBox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            brokenLadderUI.SetActive(true);
            brokenLadderShootBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            brokenLadderUI.SetActive(false);
            brokenLadderShootBox.SetActive(false);
        }
    }
}
