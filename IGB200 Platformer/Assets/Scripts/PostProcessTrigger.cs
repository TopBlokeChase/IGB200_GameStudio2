using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessTrigger : MonoBehaviour
{
    private PostProcessHandler handler;

    private void Start()
    {
        handler = GetComponentInParent<PostProcessHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            handler.StartEnterEffect();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            handler.StartExitEffect();
        }
    }
}
