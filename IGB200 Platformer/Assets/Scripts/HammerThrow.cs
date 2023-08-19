using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerThrow : MonoBehaviour
{
    [SerializeField] private GameObject hammerTrail;
    [SerializeField] private float hammerRotateSpeed = 5f;
    [SerializeField] private float hammerMoveSpeed = 5f;
    [SerializeField] private float hammerReturnSpeed = 1f;
    [SerializeField] private float hammerThrowMaxSeconds = 1.5f;

    private int damageAmount;
    private float hammerThrowCurrentSeconds = 0;
    private Vector3 endPosition;
    private bool endPositionCollected;
    private float returnTimer = 0f;

    private GameObject player;
    private PlayerCombat playerCombat;
    private bool throwLeft;

    private bool hasHitSomething;
    private bool currentlyReturning;

    // Start is called before the first frame update
    void Start()
    {
        GameObject hammerTrailObject = Instantiate(hammerTrail, transform.position, transform.rotation);
        hammerTrailObject.GetComponent<HammerThrowTrail>().SetHammerToFollow(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
    }

    public void InitialiseData(GameObject player, PlayerCombat playerCombat, bool throwLeft, int damageAmount)
    {
        this.player = player;
        this.playerCombat = playerCombat;
        this.throwLeft = throwLeft;
        this.damageAmount = damageAmount;
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, hammerRotateSpeed * Time.deltaTime);
    }

    private void Move()
    {
        hammerThrowCurrentSeconds += Time.deltaTime;

        if (hasHitSomething || hammerThrowCurrentSeconds > hammerThrowMaxSeconds)
        {
            ReturnHammerToPlayer();
        }

        else
        {
            Vector3 dir;

            // Check if the player's rotation was looking left/right 
            if (throwLeft)
            {
                dir = new Vector3(-1, 0, 0);
            }
            else
            {
                dir = new Vector3(1, 0, 0);
            }

            transform.Translate(dir * hammerMoveSpeed * Time.deltaTime, Space.World);
        }

    }

    private void ReturnHammerToPlayer()
    {
        if (endPositionCollected)
        {
            transform.position = Vector3.Lerp(endPosition, player.transform.position, returnTimer / hammerReturnSpeed);
            returnTimer += Time.deltaTime;

            if (returnTimer >= hammerReturnSpeed)
            {
                //hammers been returned to the player
                playerCombat.canThrowHammer = true;
                Destroy(this.gameObject);
            }
        }
        else
        {
            endPosition = transform.position;
            endPositionCollected = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Debug.Log("enemy hit!");
                collision.gameObject.GetComponent<Health>().DealDamage(damageAmount);
            }
            else
            {
                Debug.Log("something hit!");
            }

            hasHitSomething = true;
        }
    }
}
