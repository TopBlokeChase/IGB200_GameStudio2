using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDisc : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float movementSpeed;

    private GameObject player;
    private Vector2 dir;
    private float lifeTimer;
    private float lifeTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dir = (player.transform.position - transform.position).normalized;
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

        Rotate();
        Move();
    }
    private void Rotate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void Move()
    {
        transform.Translate(dir * movementSpeed * Time.deltaTime, Space.World);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerMovement>().AddForce(15, this.gameObject);
            player.GetComponentInChildren<Health>().DealDamage(1);
            player.GetComponentInChildren<PlayerSounds>().PlayHitFlyingDisc();

            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
