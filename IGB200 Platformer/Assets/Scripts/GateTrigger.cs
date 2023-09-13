using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.GetComponent<Gate>().OpenGate();
        this.gameObject.SetActive(false);
    }
}
