using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraClampLevel3 : MonoBehaviour
{
    [SerializeField] private GameObject normalCamera;
    [SerializeField] private GameObject cameraTrigger;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject startPan;
    [SerializeField] private GameObject endPan;

    [SerializeField] private float panSpeed;
    [SerializeField] private float delay;

    private PlayerMovement playerMovement;

    private float timer = 0;
    private bool moving = false;
    private bool atEnd = false;
    private Vector3 position;

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            this.transform.position = position;
            if (timer < delay)
            {
                timer += Time.deltaTime;
            } else
            {
                position.x -= panSpeed * Time.deltaTime;
            }
            if (this.transform.position.x < endPan.transform.position.x)
            {
                moving = false;
                atEnd = true;
                timer = 0;
            }
        }

       if (atEnd)
       {
            if (timer < delay)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Debug.Log("End Pan");
                playerMovement.isInteracting = false;
                cameraTrigger.GetComponent<CameraTriggerLevel3>().EndPan();
            }
       }
    }

    public void StartPan()
    {
        position = new Vector3(startPan.transform.position.x,
                        startPan.transform.position.y,
                        -10);
        playerMovement = player.GetComponent<PlayerMovement>();
        Debug.Log("Start Pan");
        timer = 0;
        moving = true;
        playerMovement.isInteracting = true;
    }
}