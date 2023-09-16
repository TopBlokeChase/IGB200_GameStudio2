using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LadderTrigger : MonoBehaviour
{
    [SerializeField] private bool isPlayerPlacedLadder;
    private Ladder ladder;
    private bool readyToClimb;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ladder = GetComponentInParent<Ladder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ladder.EnableInteractUI(this.gameObject.transform);
            readyToClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ladder.DisableInteractUI();
            readyToClimb = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToClimb && Input.GetKeyDown(KeyCode.E))
        {
            ladder.SetPoint(this.gameObject.transform);
        }

        if (isPlayerPlacedLadder)
        {
            if (readyToClimb && Input.GetKeyDown(KeyCode.F))
            {
                player.GetComponent<LadderPlayer>().RemoveLadder();
            }
        }
    }
}
