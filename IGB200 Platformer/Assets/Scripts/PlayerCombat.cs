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
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private GameObject hammerThrowPoint;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemyLayerMask;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = this.gameObject.GetComponentInParent<PlayerMovement>();
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

            hammer.GetComponent<HammerThrow>().InitialiseData(this.gameObject, this, playerLookingLeft);
        }
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemyLayerMask);

        foreach(Collider2D enemy in enemies)
        {
            enemy.GetComponent<Enemy>().DealDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
