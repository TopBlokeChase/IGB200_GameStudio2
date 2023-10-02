using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject puzzleCamera;

    private Vector2 triggerSize;
    void Start()
    {
        triggerSize = this.GetComponent<BoxCollider2D>().size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            puzzleCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            puzzleCamera.SetActive(false);
        }
    }
}