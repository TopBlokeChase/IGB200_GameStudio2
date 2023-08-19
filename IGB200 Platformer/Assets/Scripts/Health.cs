using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [SerializeField] private int maxHealth;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private int health;
    private int numberOfHearts;

    private void Start()
    {
        health = maxHealth;
        numberOfHearts = health;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void ResetHealth()
    {
        health = maxHealth;
        numberOfHearts = health;
    }

    public void SetHealth(int health)
    {
        maxHealth = health;
        health = maxHealth;
        numberOfHearts = health;
    }


    public void DealDamage(int damage)
    {
        if (health <= damage)
        {
            if (this.gameObject.tag == "Player")
            {
                //call the player's reset conditions
            }
            else
            {
                //must be an enemy, so
                this.gameObject.GetComponent<Enemy>().Die();
            }
        }
        else
        {
            health -= damage;
        }
    }
}
