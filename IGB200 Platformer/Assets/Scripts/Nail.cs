using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private bool hasHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHit)
        {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Terrain")
        {
            hasHit = true;       
        }

        if (collision.tag == "MovingPlatform")
        {
            transform.parent = collision.transform.parent;
            collision.gameObject.GetComponentInParent<MovingPlatform>().LockPlatform();
            //transform.parent = collision.transform.root;
            //transform.localScale = Vector3.one;
            hasHit = true;
        }
    }
}
