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

    [SerializeField] private float meleeAttackCooldown = 1f;

    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private GameObject hammerThrowPoint;

    [SerializeField] public Animator playerAnimator;
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float attackRadius;

    [SerializeField] private LayerMask enemyLayerMask;

    private PlayerMovement playerMovement;
    private GameObject player;
    private Rigidbody2D playerRB;

    private GameObject currentBoss;

    private bool hasNoteOfCourage;

    private float meleeAttackTimer;

    private PlayerSounds playerSounds;

    private bool canAttack = true;
    private bool hasNoAttackStatus = false;

    private void Start()
    {
        this.gameObject.transform.localScale = new Vector3(0.52f, 0.52f, 0.52f);
        playerSounds = this.gameObject.GetComponent<PlayerSounds>();
        playerMovement = this.gameObject.GetComponentInParent<PlayerMovement>();
        player = playerMovement.gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();
        health.SetInvulnerable(true);
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
        meleeAttackTimer += Time.deltaTime;

        if (canAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (meleeAttackTimer > meleeAttackCooldown)
                {
                    playerAnimator.SetTrigger("Attack");
                    meleeAttackTimer = 0f;
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                HammerThrow();
            }
        }
    }

    private void HammerThrow()
    {
        if (canThrowHammer)
        {
            playerAnimator.SetTrigger("RangedAttack");
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

        playerSounds.PlayAttackGrunt();

        foreach(Collider2D enemy in enemies)
        {
            if (enemy.TryGetComponent<Health>(out  Health health))
            {
                health.DealDamage(attackDamage);
            }
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
        health.SetInvulnerable(false);
        playerHealthPanel.SetActive(true);
    }

    public void DisablePlayerHealthPanel()
    {
        health.SetInvulnerable(true);
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
        health.SetInvulnerable(true);
        //reset boss health & trigger
        currentBoss.GetComponent<Enemy>().ResetBossFight();
    }

    public bool HasNoteOfCourage()
    {
        return hasNoteOfCourage;
    }

    public void SetHasNoteOfCourage(bool hasNoteOfCourage, int shieldAmount = 0)
    {
        this.hasNoteOfCourage = hasNoteOfCourage;
        health.SetShield(shieldAmount);
    }

    public void CannotAttackDuration(float time)
    {
        if (hasNoAttackStatus)
        {
            StartCoroutine(DelayAttackCoroutine(time));
            hasNoAttackStatus = false;
        }
    }

    IEnumerator DelayAttackCoroutine(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
        hasNoAttackStatus = true;
    }
}
