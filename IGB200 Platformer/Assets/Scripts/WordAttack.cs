using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordAttack : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectIconPrefab;
    [SerializeField] private float playerSlowTime;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<string> attackWords;
    [SerializeField] private TMP_Text attackText;

    private GameObject player;
    private Vector2 dir;
    private float lifeTimer;
    private float lifeTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dir = (player.transform.position - transform.position).normalized;

        int randNum = Random.Range(0, attackWords.Count);
        attackText.text = attackWords[randNum];
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTimer < lifeTime)
        {
            lifeTimer += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Move();
    }

    private void Move()
    {
        transform.Translate(dir * movementSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.GetComponent<PlayerMovement>().IsSlowed() == false)
            {
                GameObject statusEffectIcon = Instantiate(statusEffectIconPrefab);
                statusEffectIcon.GetComponent<StatusEffectIcon>().SetTimeAndAnimate(playerSlowTime, player);

                player.GetComponent<PlayerMovement>().GiveSlowStatusEffect(playerSlowTime);
                player.GetComponentInChildren<PlayerCombat>().CannotAttackDuration(playerSlowTime);
                player.GetComponentInChildren<PlayerSounds>().PlaySlowedBegin(playerSlowTime);

                player.GetComponentInChildren<PlayerStatusParticleEffect>().StartParticles(playerSlowTime);

                //player.GetComponentInChildren<Health>().DealDamage(1);

                Destroy(this.gameObject);
            }
        }
    }
}
