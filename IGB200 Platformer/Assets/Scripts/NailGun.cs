using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : MonoBehaviour
{
    [SerializeField] private GameObject nailPrefab;
    [SerializeField] private LayerMask terrain;
    [SerializeField] private float distance = 10f;
    [SerializeField] GameObject player;
    Vector2 mousePosition;
    Vector3 startingPosition;
    float angle;

    bool isAiming;

    PlayerMovement playerMovement;

    LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!playerMovement.isInteracting)
            {
                playerMovement.isInteracting = true;
                isAiming = true;              
            }    
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            playerMovement.isInteracting = false;
            isAiming = false;
            Shoot();
        }

        if (isAiming)
        {
            lr.enabled = true;
            Aim();
        }
        else
        {
            lr.enabled = false;
        }
    }

    private void Aim()
    {
        RotateToCursor();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, distance);

        if (hit.collider != null)
        {
            lr.useWorldSpace = true;
            Debug.Log("Hitting");
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.useWorldSpace = false;
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, new Vector3(distance, 0, 0));
        }
    }

    private void Shoot()
    {
        Instantiate(nailPrefab, transform.position, transform.rotation);
        playerMovement.isInteracting = false;
        isAiming = false;
    }

    private void RotateToCursor()
    {
        mousePosition = Input.mousePosition;
        startingPosition = Camera.main.WorldToScreenPoint(transform.position);

        mousePosition.x -= startingPosition.x;
        mousePosition.y -= startingPosition.y;

        angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
