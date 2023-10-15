using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform_BrokenFloor : MonoBehaviour
{
    [SerializeField] private GameObject endPosition;
    [SerializeField] private GameObject fallEndPosition;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fallRotationSpeed;
    [SerializeField] private float fallMoveSpeed;
    [SerializeField] private bool flipRotation;
    [SerializeField] private float moveSpeed;

    private PolygonCollider2D polygonCollider;

    public bool finished;

    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponentInChildren<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            if (transform.position != endPosition.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition.transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                polygonCollider.enabled = true;

                if (flipRotation)
                {
                    transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                }
            }
        }
        else 
        {
            if (flipRotation)
            {
                transform.Rotate(0, 0, -fallRotationSpeed * Time.deltaTime);
            }
            else
            {
                transform.Rotate(0, 0, fallRotationSpeed * Time.deltaTime);
            }

            transform.position = Vector3.MoveTowards(transform.position, fallEndPosition.transform.position, fallMoveSpeed * Time.deltaTime);

            if (transform.position == fallEndPosition.transform.position)
            {
                Destroy(transform.parent.parent.gameObject);
            }
        }
    }

    public void Finish()
    {
        finished = true;
        polygonCollider.enabled = false;
    }

    public void CheckEndPositionValid(Collider2D allowedArea)
    {
        if (!allowedArea.bounds.Contains(endPosition.transform.position))
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
