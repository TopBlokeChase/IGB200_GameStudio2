using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] private bool addForce = true;
    [SerializeField] private float forceAmount = 15f;

    private float timer = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer > 1)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (addForce)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().AddForce(forceAmount, this.gameObject);
                }
                collision.gameObject.GetComponentInChildren<Health>().DealDamage(1);
                timer = 0;
            }
        }
    }
}
