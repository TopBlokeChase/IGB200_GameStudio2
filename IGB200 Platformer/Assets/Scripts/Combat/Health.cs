using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject damageParticlePrefab;
    [SerializeField] private int maxHealth;
    [SerializeField] private int shieldAmount;

    [SerializeField] private bool isBasicEnemy;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite shieldHeart;
    [SerializeField] private Sprite emptyHeart;

    private int health;
    private int numberOfHearts;

    private bool isInvulnerable;

    private BossSounds bossSounds;

    private float bossIFramesTime = 0.5f;
    private float bossIFrameTimer = 0.5f;

    private void Start()
    {
        bossSounds = transform.parent.GetComponentInChildren<BossSounds>();
        health = maxHealth;
        numberOfHearts = health + shieldAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.tag == "Enemy")
        {
            bossIFrameTimer += Time.deltaTime;
        }
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
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
        this.shieldAmount += ShieldAmount;
    }

    public void DealDamage(int damage)
    {
        if (this.gameObject.tag == "Player")
        {
            if (this.gameObject.GetComponent<PlayerCombat>().ReturnPlayerDead() == false)
            {
                damageParticlePrefab.GetComponent<ParticleSystem>().Play();
            }
        }
        else if (this.gameObject.tag == "EnemyBasic")
        {
            GameObject damageParticle = Instantiate(damageParticlePrefab, transform.position, Quaternion.identity);
            damageParticle.transform.parent = null;
        }

        if (isInvulnerable)
        {
            return;
        }

        if (health <= damage)
        {
            if (this.gameObject.tag == "Player")
            {
                if (this.gameObject.GetComponent<PlayerCombat>().ReturnPlayerDead() == false)
                {
                    this.gameObject.GetComponent<PlayerCombat>().Die();
                    health -= damage;
                    SetHeartsUI();
                }
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
            //is boss
            if (this.gameObject.tag == "Player")
            {
                if (this.gameObject.GetComponent<PlayerCombat>().ReturnPlayerDead() == false)
                {
                    health -= damage;
                    SetHeartsUI();
                }
            }
            else
            {
                if (this.gameObject.tag == "Enemy")
                {
                    if (bossIFrameTimer >= bossIFramesTime)
                    {
                        bossIFrameTimer = 0f;
                        GameObject damageParticle = Instantiate(damageParticlePrefab, transform.position, Quaternion.identity);
                        damageParticle.transform.parent = null;
                        bossSounds.PlayHurt();
                        health -= damage;
                        SetHeartsUI();
                    }
                }
                else
                {
                    health -= damage;
                    SetHeartsUI();
                }
            }
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
