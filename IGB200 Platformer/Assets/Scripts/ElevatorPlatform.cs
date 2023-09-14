using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    [SerializeField] private bool movesAutomatically;
    [SerializeField] private float elevatorSpeed = 5f;
    [SerializeField] private GameObject endPoint;

    private Vector3 startPoint;

    private bool atEndPostion;
    private bool moving;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerCurrentParent = collision.transform.parent;
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent = playerCurrentParent;
        }
    }

    public void MovePlatform()
    {
        moving =  true;
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
