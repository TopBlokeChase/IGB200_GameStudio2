using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isInteracting;
    public bool isLookingLeft;

    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnimator;

    private bool isGrounded;
    private Rigidbody2D playerRigidbody;
    private float directionInput;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            CheckDirectionInput();
            CheckGrounded();
            CheckPlayerInput();
    }

    // Check player inputs other than 'axis' input. E.g., jump, interact etc.
    private void CheckPlayerInput()
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !isInteracting)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(playerRigidbody.velocity.x, jumpHeight);
            }
        }
    }

    // Check if player is grounded via velocity check
    private void CheckGrounded()
    {
        if (playerRigidbody.velocity.y >= -0.1f && playerRigidbody.velocity.y <= 0.1f)
        {

            isGrounded = true;
            playerAnimator.SetBool("isJumping", false);
        }
        else
        {
            isGrounded = false;
            playerAnimator.SetBool("isJumping", true);
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

        playerRigidbody.velocity = new Vector2(directionInput * movementSpeed, playerRigidbody.velocity.y);

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
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            playerAnimator.SetBool("isRunning", false);
        }
    }
}
