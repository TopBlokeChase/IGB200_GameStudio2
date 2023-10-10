using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerMovement : MonoBehaviour
{
    public bool isInteracting;
    public bool isLookingLeft;

    [SerializeField] private LayerMask ground;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] private float addForceUnlockConstraintTime = 3f;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnimator;

    private bool isJumping;
    private bool isGrounded;
    private Rigidbody2D playerRigidbody;
    private float directionInput;

    [SerializeField] private bool hasAddedForce;
    private float forceTimer;

    private float coyoteTimer;

    private float pressedJumpTime = 0.27f;
    private float pressedJumpTimer;

    private PlayerSounds playerSounds;

    private bool isSlowed;

    // Start is called before the first frame update
    void Start()
    {
        playerSounds = GetComponentInChildren<PlayerSounds>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isGrounded);
        CheckAddedForce();
        CheckDirectionInput();
        CheckGrounded();
        CheckPlayerInput();
    }

    // Check player inputs other than 'axis' input. E.g., jump, interact etc.
    private void CheckPlayerInput()
    {
        if (isGrounded && !isJumping)
        {
            if (Input.GetButtonDown("Jump") && !isInteracting)
            {
                isJumping = true;
                playerSounds.PlayJumpGrunt();
                GetComponent<Rigidbody2D>().velocity = new Vector2(playerRigidbody.velocity.x, jumpHeight);
                pressedJumpTimer = 0;
            }
        }
    }

    // Check if player is grounded via raycast
    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, ground);

        if (hit.collider != null)
        {
            isGrounded = true;
            playerAnimator.SetBool("isJumping", false);
            coyoteTimer = 0;
            
        }
        else
        {
            coyoteTimer += Time.deltaTime;

            if (coyoteTimer <= coyoteTime)
            {
                isGrounded = true;
            }
            else
            {
                isJumping = false;
                isGrounded = false;
                playerAnimator.SetBool("isJumping", true);
            }
        }

        if (isGrounded)
        {
            pressedJumpTimer += Time.deltaTime;
            if (pressedJumpTimer <= pressedJumpTime)
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    // Check the players 'axis' input and move them
    private void CheckDirectionInput()
    {
        if (!isInteracting)
        {
            directionInput = Input.GetAxis("Horizontal");
        }
        else
        {
            directionInput = 0;
        }

        if (!hasAddedForce)
        {
            playerRigidbody.velocity = new Vector2(directionInput * movementSpeed, playerRigidbody.velocity.y);
        }

        if (directionInput < 0)
        {
            //playerSprite.flipX = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerAnimator.SetBool("isRunning", true);
            isLookingLeft = true;
        }
        else if (directionInput > 0)
        {
            // playerSprite.flipX = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerAnimator.SetBool("isRunning", true);
            isLookingLeft = false;
        }
        else if (directionInput == 0)
        {
            if (hasAddedForce)
            {
                playerAnimator.SetBool("isRunning", false);
            }
            else
            {
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                playerAnimator.SetBool("isRunning", false);
            }
        }
    }

    // Need to do this check so we can unlock the X Axis movement constraint to apply the force
    private void CheckAddedForce()
    {
        if (hasAddedForce)
        {
            forceTimer += Time.deltaTime;

            if (forceTimer >= addForceUnlockConstraintTime)
            {
                hasAddedForce = false;
                forceTimer = 0;
            }
        }
    }

    public void AddForce(float forceToAdd, GameObject forceObject)
    {
        float forceToAddX;

        if (forceObject.transform.position.x < this.transform.position.x)
        {
            forceToAddX = forceToAdd;
        }
        else
        {           
            forceToAddX = forceToAdd * -1;
        }

        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        hasAddedForce = true;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.velocity += new Vector2(forceToAddX, forceToAdd / 2);

        playerSounds.PlayHurt();
    }

    public void GiveSlowStatusEffect(float time)
    {
        if (!isSlowed)
        {
            StartCoroutine(SlowStatusCoroutine(time));
            isSlowed = true;
        }
    }

    IEnumerator SlowStatusCoroutine(float time)
    {
        float originalMoveSpeed = movementSpeed;
        float originalAnimatorSpeed = playerAnimator.speed;

        movementSpeed = movementSpeed / 2;
        playerAnimator.speed = playerAnimator.speed / 2;

        yield return new WaitForSeconds(time);

        movementSpeed = originalMoveSpeed;
        playerAnimator.speed = originalAnimatorSpeed;
        isSlowed = false;
    }
}
