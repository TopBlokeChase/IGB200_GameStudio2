using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    [TextArea]
    public string NOTE = "This script was attached to the sprite object " +
        "rather than the parent due to needing access to the methods for " +
        "animation events, which cannot be accessed from the parent.";

    public bool canThrowHammer = true;
    [SerializeField] bool canUseMeleeRepair;

    [SerializeField] private GameObject particleCourageNote;

    [SerializeField] private Health health;
    [SerializeField] private float deathScreenDelayTime = 2.5f;
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

    private bool enemyExists = false;

    private bool playerIsDead;
    private bool hasSetDeadStatus;

    

    private void Start()
    {
        this.gameObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        playerSounds = this.gameObject.GetComponent<PlayerSounds>();
        playerMovement = this.gameObject.GetComponentInParent<PlayerMovement>();
        player = playerMovement.gameObject;
        playerRB = player.GetComponent<Rigidbody2D>();
        health.SetInvulnerable(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("playerisDead " + playerIsDead);
        Debug.Log("Dead status "+hasSetDeadStatus);
        if (!playerMovement.isInteracting && !playerIsDead)
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

        if (canUseMeleeRepair)
        {
            Collider2D ladder = Physics2D.OverlapCircle(attackPoint.transform.position, attackRadius);

            if (ladder != null)
            {
                if(ladder.gameObject.tag == "BrokenLadder")
                {
                        ladder.GetComponent<BrokenLadder>().RemoveHammerCount();
                }
            }
        }

        playerSounds.PlayAttackGrunt();

        foreach(Collider2D enemy in enemies)
        {
            if (enemy.TryGetComponent<Health>(out  Health health))
            {
                if (health != null)
                {
                    health.DealDamage(attackDamage);            
                }
            }
        } 
        
        if (enemies.Length == 0)
        {
            enemyExists = false;
        }
        else
        {
            enemyExists = true;
        }
    }

    public void PlayHammerHitGroundIfNoTarget()
    {
        if (playerMovement.IsGrounded() && !enemyExists)
        {
            playerSounds.PlayAttackHitGround();
        }
    }

    public void SetPlayerDead(bool value)
    {
        playerMovement.isInteracting = value;
        playerIsDead = value;
        playerAnimator.SetBool("isDead", value);
    }

    public bool ReturnPlayerDead()
    {
        return playerIsDead;
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
        if (!hasSetDeadStatus)
        {
            StartCoroutine(DelayDeathScreen(deathScreenDelayTime));
            hasSetDeadStatus = true;
        }
    }

    public void RetryBossFight()
    {
        SetPlayerDead(false);
        hasSetDeadStatus = false;
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

    public void PlayNotePickupParticle()
    {
        particleCourageNote.GetComponent<ParticleSystem>().Play();
    }

    public void CannotAttackDuration(float time)
    {
        if (!hasNoAttackStatus)
        {
            hasNoAttackStatus = true;
            StartCoroutine(DelayAttackCoroutine(time));
        }
    }

    public void CannotAttackToggle(bool value)
    {
        canAttack = value;
    }

    IEnumerator DelayAttackCoroutine(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
        hasNoAttackStatus = false;
    }

    IEnumerator DelayDeathScreen(float time)
    {
        SetPlayerDead(true);
        MusicHandler musicHandler = GameObject.FindGameObjectWithTag("MusicHandler").GetComponent<MusicHandler>();
        musicHandler.PlayGameOver();
        yield return new WaitForSeconds(time);
        retryMenuCanvas.SetActive(true);
        Time.timeScale = 0;       
    }
}
