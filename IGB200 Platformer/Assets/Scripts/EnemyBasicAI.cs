using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBasicAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float forceOnTouch = 10f;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;

    private GameObject pointToMoveTo;
    private float waitTimer;
    private bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        pointToMoveTo = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPoint();
    }

    private void MoveToPoint()
    {
        if (transform.position != pointToMoveTo.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointToMoveTo.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waitTimer = 0;

                if (pointToMoveTo == pointA)
                {
                    pointToMoveTo = pointB;
                }
                else
                {
                    pointToMoveTo = pointA;

                }       
            }
        }     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("colliding with player!");
            collision.gameObject.GetComponent<PlayerMovement>().AddForce(forceOnTouch);
        }
    }

    private void OnDrawGizmos()
    {
        float labelOffsetY = 1f;
        Vector3 labelAPos = new Vector3(pointA.transform.position.x, pointA.transform.position.y - labelOffsetY, pointA.transform.position.z);
        Vector3 labelBPos = new Vector3(pointB.transform.position.x, pointB.transform.position.y - labelOffsetY, pointB.transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointA.transform.position, 1);
        Handles.Label(labelAPos, "Point A");

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointB.transform.position, 1);
        Handles.Label(labelBPos, "Point B");
    }
}
