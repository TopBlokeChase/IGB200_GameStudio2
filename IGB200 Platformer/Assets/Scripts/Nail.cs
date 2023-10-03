using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private GameObject sprite;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float deathTimer = 10f;

    private float timer;

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
        else
        {
            timer += Time.deltaTime;

            if (timer >= deathTimer)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Terrain")
        {
            hitSound.Play();
            hasHit = true;       
        }

        if (collision.tag == "MovingPlatform")
        {
            transform.parent = collision.transform.parent;
            collision.gameObject.GetComponentInParent<MovingPlatform>().LockPlatform();
            //transform.parent = collision.transform.root;
            //transform.localScale = Vector3.one;
            hitSound.Play();
            hasHit = true;
        }

        if (collision.tag == "Button")
        {
            collision.transform.parent.GetComponentInChildren<ElevatorPlatform>().MovePlatform();
            hitSound.Play();
            hasHit = true;
        }

        if (collision.tag == "Glass")
        {
            collision.gameObject.GetComponent<GlassTrigger>().GlassShatter();
            hitSound.Play();
            StartCoroutine(Delay(hitSound.clip.length));
        }

        if (collision.tag == "BrokenLadder")
        {
            collision.gameObject.GetComponent<BrokenLadder>().RemoveNailCount();
            hitSound.Play();
            StartCoroutine(Delay(hitSound.clip.length));
        }
    }

    IEnumerator Delay(float sec)
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        sprite.SetActive(false);
        yield return new WaitForSeconds(sec);

        Destroy(this.gameObject);
    }
}
