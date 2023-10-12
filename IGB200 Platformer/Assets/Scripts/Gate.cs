using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private GameObject gateTrigger;
    [SerializeField] private bool bossGate;
    [SerializeField] private float gateOpenSpeed;
    [SerializeField] private float gateCloseSpeed;
    [SerializeField] private float gateOpenHeight;
    [SerializeField] private AudioSource gateOpenAudioSource;

    private bool opening;
    private bool closing;

    private float timer;

    private Vector3 startPosition;
    private Vector3 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x, transform.position.y + gateOpenHeight, transform.position.z);
        if (bossGate)
        {
            transform.position = endPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {          
            if (timer * gateOpenSpeed < 1)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, timer / gateOpenSpeed);
                timer += Time.deltaTime;
            }
            else
            {
                opening = false;
            }
        }

        if (closing)
        {
            if (timer * gateCloseSpeed < 1)
            {
                transform.position = Vector3.Lerp(endPosition, startPosition, timer / gateCloseSpeed);
                timer += Time.deltaTime;
            }
            else
            {
                closing = false;
            }
        }
    }

    public void CloseGate()
    {
        timer = 0;
        closing = true;
    }

    public void OpenGate()
    {
        gateOpenAudioSource.Play();
        timer = 0;
        opening = true;
    }

    public void EnableGateTrigger()
    {
        gateTrigger.SetActive(true);
    }
}
