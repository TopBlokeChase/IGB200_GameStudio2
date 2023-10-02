using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TEST ENTER");
        if (collision.gameObject.tag == "Player")
        {
            tutorialText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("TEST EXIT");
        if (collision.gameObject.tag == "Player")
        {
            tutorialText.SetActive(false);
        }
    }
}