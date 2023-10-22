using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetElevatorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject elevatorPlatform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            elevatorPlatform.GetComponent<ElevatorPlatform>().Reset();
        }
    }
}

