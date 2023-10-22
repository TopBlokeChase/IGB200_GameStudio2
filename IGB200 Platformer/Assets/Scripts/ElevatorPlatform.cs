using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [SerializeField] private AudioSource startStopSoundSource;
    [SerializeField] private AudioSource loopingSoundSource;
    [SerializeField] private AudioSource errorSoundSource;
    [SerializeField] private bool movesAutomatically;
    [SerializeField] private float elevatorSpeed = 5f;
    [SerializeField] private GameObject endPoint;

    private Vector3 startPoint;

    private bool atEndPostion;
    private bool moving;

    private bool hasPlayedInitialSound;

    private Transform playerCurrentParent;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (!atEndPostion)
            {
                if (transform.position != endPoint.transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, endPoint.transform.position, elevatorSpeed * Time.deltaTime);
                }
                else
                {
                    startStopSoundSource.Play();
                    loopingSoundSource.Stop();
                    hasPlayedInitialSound = false;

                    if (!movesAutomatically)
                    {
                        atEndPostion = true;
                        moving = false;
                    }
                    else
                    {
                        atEndPostion = true;                       
                    }
                }
            }
            else
            {
                if (transform.position != startPoint)
                {
                    transform.position = Vector3.MoveTowards(transform.position, startPoint, elevatorSpeed * Time.deltaTime);
                }
                else
                {
                    startStopSoundSource.Play();
                    loopingSoundSource.Stop();
                    hasPlayedInitialSound = false;

                    if (!movesAutomatically)
                    {
                        atEndPostion = false;
                        moving = false;
                    }
                    else
                    {
                        atEndPostion = false;
                    }
                }
            }
        }
    }

    public void PlayerUnderneath()
    {       
        errorSoundSource.Play();
        atEndPostion = false;
        moving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent = playerCurrentParent;
            collision.transform.parent = null;
        }
    }

    public void MovePlatform()
    {
        if (!hasPlayedInitialSound)
        {
            startStopSoundSource.Play();
            loopingSoundSource.Play();
            hasPlayedInitialSound = true;
        }

        moving =  true;
    }

    public void Reset()
    {
        if (!hasPlayedInitialSound)
        {
            startStopSoundSource.Play();
            loopingSoundSource.Play();
            hasPlayedInitialSound = true;
        }

        atEndPostion = true;
        moving = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float labelOffsetY = 1f;
        Vector3 labelEndPos = new Vector3(endPoint.transform.position.x, endPoint.transform.position.y - labelOffsetY, endPoint.transform.position.z);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPoint.transform.position, 1);
        UnityEditor.Handles.Label(labelEndPos, "EndPosition");

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, endPoint.transform.position);
    }
#endif
}
