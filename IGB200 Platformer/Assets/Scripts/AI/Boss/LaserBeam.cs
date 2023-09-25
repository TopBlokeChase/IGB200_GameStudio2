using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float maxTime;
    private float timer;
    private bool readyToDamage;

    // Update is called once per frame
    void Update()
    {       
        if (!readyToDamage)
        {
            timer += Time.deltaTime;
        }

        if (timer > maxTime)
        {
            readyToDamage = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (readyToDamage)
            {
                collision.gameObject.GetComponent<PlayerMovement>().AddForce(7.5f, this.gameObject);
                collision.gameObject.GetComponentInChildren<Health>().DealDamage(1);
                readyToDamage = false;
                timer = 0;
            }
        }
    }
}
