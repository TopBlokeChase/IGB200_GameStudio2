using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    [SerializeField] private int shieldAmount;

    [SerializeField] private bool isBasicEnemy;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite shieldHeart;
    [SerializeField] private Sprite emptyHeart;

    private int health;
    private int numberOfHearts;

    private void Start()
    {
        health = maxHealth;
        numberOfHearts = health + shieldAmount;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResetHealth()
    {
        health = maxHealth + shieldAmount;
        numberOfHearts = health;
        InitialiseHeartsUI();
    }

    public void SetHealth(int health)
    {
        maxHealth = health + shieldAmount;
        health = maxHealth;
        numberOfHearts = health;
    }

    public void SetShield(int ShieldAmount)
    {
        this.shieldAmount = ShieldAmount;
    }

    public void DealDamage(int damage)
    {
        if (health <= damage)
        {
            if (this.gameObject.tag == "Player")
            {
                this.gameObject.GetComponent<PlayerCombat>().Die();
            }

            if (this.gameObject.tag == "EnemyBasic")
            {
                this.gameObject.GetComponent<EnemyBasicAI>().Die();
            }

            if (this.gameObject.tag == "Enemy")
            {
                //must be an boss enemy, so
                this.gameObject.GetComponent<Enemy>().Die();
            }
        }
        else
        {
            health -= damage;
            SetHeartsUI();
        }
    }

    void InitialiseHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = true;
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        for (int j = health - shieldAmount; j < health; j++)
        {
            hearts[j].sprite = shieldHeart;
        }
    }
void SetHeartsUI()
    {
        if (!isBasicEnemy)
        {
            for (int i = 0; i < numberOfHearts; i++)
            {
                if (i + 1 > health )
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }
    }
}
