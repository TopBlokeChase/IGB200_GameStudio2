using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyBasicAI : MonoBehaviour
{
    public enum EnemyType
    {
        Slime,
        Tornado,
        MentalHealth
    }

    [Header("Enemy Type Settings")]
    [SerializeField] EnemyType enemyType;
    [SerializeField] private Sprite slimeSprite;
    [SerializeField] private Sprite tornadoSprite;
    [SerializeField] private Sprite mentalHealthSprite;
    [SerializeField] private Animator animator;

    [Header("Sounds")]
    [SerializeField] private AudioSource deathSoundSource;

    [Header("References & Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float forceOnTouch = 10f;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;

    private GameObject pointToMoveTo;
    private float waitTimer;
    private bool isWaiting;

    private bool hasSetAnimBool;

    // Start is called before the first frame update
    void Start()
    {
        if (enemyType == EnemyType.Slime)
        {
            spriteRenderer.sprite = slimeSprite;
            animator.SetBool("isSlime", true);
        }

        if (enemyType == EnemyType.Tornado)
        {
            spriteRenderer.sprite = tornadoSprite;
            animator.SetBool("isTornado", true);
        }

        if (enemyType == EnemyType.MentalHealth)
        {
            spriteRenderer.sprite = mentalHealthSprite;
            animator.SetBool("isMentalHealth", true);
        }


        pointToMoveTo = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPoint();
    }

    private void MoveToPoint()
    {
        if (transform.position != pointToMoveTo.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointToMoveTo.transform.position, movementSpeed * Time.deltaTime);

            if (!hasSetAnimBool)
            {
                animator.SetBool("isMoving", true);
                hasSetAnimBool = true;
            }
        }
        else
        {
            waitTimer += Time.deltaTime;

            if (hasSetAnimBool)
            {
                animator.SetBool("isMoving", false);
                hasSetAnimBool = false;
            }

            if (waitTimer >= waitTime)
            {
                waitTimer = 0;

                if (pointToMoveTo == pointA)
                {
                    pointToMoveTo = pointB;
                    spriteRenderer.flipX = true;
                }
                else
                {
                    pointToMoveTo = pointA;
                    spriteRenderer.flipX = false;
                }       
            }
        }     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().AddForce(forceOnTouch, this.gameObject);
        }
    }

    public void Die()
    {
        deathSoundSource.Play();

        spriteRenderer.enabled = false;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(Delay(deathSoundSource.clip.length));      
    }

    IEnumerator Delay(float sec)
    {
        yield return new WaitForSeconds(sec);

        this.gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float labelOffsetY = 1f;
        Vector3 labelAPos = new Vector3(pointA.transform.position.x, pointA.transform.position.y - labelOffsetY, pointA.transform.position.z);
        Vector3 labelBPos = new Vector3(pointB.transform.position.x, pointB.transform.position.y - labelOffsetY, pointB.transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointA.transform.position, 1);
        UnityEditor.Handles.Label(labelAPos, "Point A");

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(pointB.transform.position, 1);
        UnityEditor.Handles.Label(labelBPos, "Point B");
    }
#endif
}
