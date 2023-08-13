using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject linkedNPC;

    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(int damage)
    {
        if (currentHealth <= damage)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            currentHealth -= damage;
        }
    }

    private void Die()
    {
        linkedNPC.GetComponent<MainNPCDialogue>().SetBossDefeated();
        Destroy(this.gameObject);
    }
}
