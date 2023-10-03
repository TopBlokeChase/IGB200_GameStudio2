using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform_PlayerDetection : MonoBehaviour
{
    private ElevatorPlatform _platform;

    private void Start()
    {
        _platform = GetComponentInParent<ElevatorPlatform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _platform.PlayerUnderneath();
        }
    }
}
