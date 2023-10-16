using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderDistanceChecker : MonoBehaviour
{
    [SerializeField] private float xDistance;
    [SerializeField] private float yDistance;
    private LadderPlayer_NEW ladderPlayer_NEW;
    private GameObject player;
    private Vector3 placedPosition;

    private void Awake()
    {
        placedPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        ladderPlayer_NEW = player.GetComponent<LadderPlayer_NEW>();
    }

    private void Update()
    {
        if (player.transform.position.x  < placedPosition.x - xDistance || player.transform.position.x > placedPosition.x + xDistance 
            || player.transform.position.y < placedPosition.y - yDistance || player.transform.position.y > placedPosition.y + yDistance)
        {
            ladderPlayer_NEW.RemoveLadder();
        }
    }
}
