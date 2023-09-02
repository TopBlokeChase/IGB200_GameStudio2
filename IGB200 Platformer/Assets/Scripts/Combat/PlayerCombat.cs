using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [TextArea]
    public string NOTE = "This script was attached to the sprite object " +
        "rather than the parent due to needing access to the methods for " +
        "animation events, which cannot be accessed from the parent.";

    public bool canThrowHammer = true;

    [SerializeField] private Health health;
    [SerializeField] private GameObject playerHealthPanel;
    [SerializeField] private GameObject retryMenuCanvas;

    [SerializeField] private int attackDamage = 1;
    [SerializeField] private int hammerThrowDamage = 1;

    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private GameObject hammerThrowPoint;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float attackRadius;

    [SerializeField] private LayerMask enemyLayerMask;

    private PlayerMovement playerMovement;
    private GameObject player;
    private Rigidbody2D playerRB;

    private GameObject currentBoss;

    private void Start()
    {
        playerMovement = this.gameObject.GetComponentInParent<PlayerMovement>();
        player = playerMovement.gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.isInteracting)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            playerAnimator.SetTrigger("Attack");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            HammerThrow();
        }
    }

    private void HammerThrow()
    {
        if (canThrowHammer)
        {
            canThrowHammer = false;
            GameObject hammer = Instantiate(hammerPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);

            bool playerLookingLeft;
            if (playerMovement.isLookingLeft)
            {
                playerLookingLeft = true;
            }
            else
            {
                playerLookingLeft = false;
            }

            hammer.GetComponent<HammerThrow>().InitialiseData(this.gameObject, this, playerLookingLeft, hammerThrowDamage);
        }
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemyLayerMask);

        foreach(Collider2D enemy in enemies)
        {
            enemy.GetComponent<Health>().DealDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }

    public void SetCurrentBoss(GameObject currentBoss)
    {
        health.ResetHealth();
        this.currentBoss = currentBoss;
    }

    public void EnablePlayerHealthPanel()
    {
        playerHealthPanel.SetActive(true);
    }

    public void DisablePlayerHealthPanel()
    {
        playerHealthPanel.SetActive(false);
    }

    public void Die()
    {
        retryMenuCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void RetryBossFight()
    {
        //reset timescale
        Time.timeScale = 1;
        //disable retryMenu
        retryMenuCanvas.SetActive(false);
        //set player back to spawn point
        playerRB.velocity = Vector2.zero;
        player.transform.position = currentBoss.GetComponent<Enemy>().PlayerSpawnPos().position;
        //reset player health
        health.ResetHealth();
        //reset boss health & trigger
        currentBoss.GetComponent<Enemy>().ResetBossFight();
    }
}
