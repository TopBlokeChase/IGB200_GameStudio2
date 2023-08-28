using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LadderTrigger : MonoBehaviour
{
    private Ladder ladder;
    private bool readyToClimb;

    // Start is called before the first frame update
    void Start()
    {
        ladder = GetComponentInParent<Ladder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ladder.EnableInteractUI(this.gameObject.transform);
        readyToClimb = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ladder.DisableInteractUI();
        readyToClimb = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToClimb && Input.GetKeyDown(KeyCode.E))
        {
            ladder.SetPoint(this.gameObject.transform);
        }
    }
}
