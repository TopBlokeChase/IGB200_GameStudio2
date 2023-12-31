using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HammerThrow : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private GameObject hitSoundSource;
    [SerializeField] private GameObject throwSoundSource;
    [SerializeField] private List<AudioClip> terrainHitSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> throwSounds = new List<AudioClip>();

    [Header("References & Settings")]
    [SerializeField] private GameObject hammerTrail;
    [SerializeField] private GameObject hammer;
    [SerializeField] private float hammerArcHeight = 1f;
    [SerializeField] private GameObject endThrowPoint;
    [SerializeField] private float hammerRotateSpeed = 5f;
    [SerializeField] private float hammerThrowTime = 5f;
    [SerializeField] private float hammerReturnTime = 1f;
    [SerializeField] private float hammerThrowMaxSeconds = 1.5f;

    private int damageAmount;
    private float hammerThrowCurrentSeconds = 0;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool endPositionCollected;

    private float throwTimer = 0f;
    private float returnTimer = 0f;

    private GameObject player;
    private PlayerCombat playerCombat;
    private bool throwLeft;

    private bool hasHitSomething;
    private bool currentlyReturning;

    private bool reachedMidPoint;

    bool hasPlayedReturnAnim;

    // Start is called before the first frame update
    void Start()
    {
        GameObject hammerTrailObject = Instantiate(hammerTrail, transform.position, transform.rotation);
        hammerTrailObject.GetComponent<HammerThrowTrail>().SetHammerToFollow(this.gameObject);
        startPosition = this.gameObject.transform.position;
        endPosition = endThrowPoint.transform.position;

        throwSoundSource.GetComponent<AudioSource>().clip = (PickSound(throwSounds));
        throwSoundSource.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        ReturnHammerToPlayer();
    }

    public void InitialiseData(GameObject player, PlayerCombat playerCombat, bool throwLeft, int damageAmount)
    {
        this.player = player;
        this.playerCombat = playerCombat;
        this.throwLeft = throwLeft;
        this.damageAmount = damageAmount;
    }

    private void Rotate()
    {
        hammer.transform.Rotate(0, 0, hammerRotateSpeed * Time.deltaTime);
    }

    private void Move()
    {
        throwTimer += Time.deltaTime;
        if (!endPositionCollected)
        {
            if (hasHitSomething)
            {
                endPositionCollected = true;
                endPosition = transform.position;
            }
            else 
            if (transform.position == endPosition)
            {
                endPositionCollected = true;
            }
            else
            {
                Vector3 dir;

                // Check if the player's rotation was looking left/right 
                if (throwLeft)
                {
                    dir = new Vector3(-1, 0, 0);
                }
                else
                {
                    dir = new Vector3(1, 0, 0);
                }

                //transform.Translate(dir * hammerMoveSpeed * Time.deltaTime, Space.World);

                Vector3 midpoint = (startPosition + endPosition) / 2;
                midpoint -= new Vector3(0, hammerArcHeight, 0);

                Vector3 startArc = startPosition - midpoint;
                Vector3 endArc = endPosition - midpoint;

                transform.position = Vector3.Slerp(startArc, endArc, hammerThrowTime * throwTimer);
                transform.position += midpoint;

                //if (transform.position.x == (startPosition.x + endPosition.x) / 2)
                //{
                //    reachedMidPoint = true;
                //}

                //if (!reachedMidPoint)
                //{
                //    transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, transform.position.y + hammerArcHeight, transform.position.z), hammerThrowTime / 2 * throwTimer);
                //}
                //else
                //{
                //    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y  hammerArcHeight, transform.position.z), hammerThrowTime / 2 * throwTimer);
                //}
                
            }
        }
    }

    private void ReturnHammerToPlayer()
    {    
        if (endPositionCollected)
        {
            currentlyReturning = true;
            Vector3 midpoint = (endPosition + startPosition) / 2;
            midpoint -= new Vector3(0, hammerArcHeight, 0);

            Vector3 startArc = endPosition - midpoint;
            Vector3 endArc = player.transform.position - midpoint;

            transform.position = Vector3.Slerp(startArc, endArc, returnTimer / hammerReturnTime);
            transform.position += midpoint;

            //transform.position = Vector3.Lerp(endPosition, player.transform.position, returnTimer / hammerReturnTime);
            returnTimer += Time.deltaTime;

            //if (!hasPlayedReturnAnim)
            //{
            //    if (returnTimer / hammerReturnTime >= 0.8f)
            //    {
            //        playerCombat.playerAnimator.SetTrigger("RangedReturn");
            //        hasPlayedReturnAnim = true;
            //    }
            //}

            if (returnTimer >= hammerReturnTime)
            {
                //hammers been returned to the player
                playerCombat.canThrowHammer = true;              
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBasic")
            {
                collision.gameObject.GetComponent<Health>().DealDamage(damageAmount);
                hasHitSomething = true;
            }
            else
            if (collision.gameObject.tag == "NPC")
            {
                return;
            }
            else
            if (collision.gameObject.tag == "BrokenLadder")
            {
                collision.gameObject.GetComponent<BrokenLadder>().RemoveHammerCount();
                hasHitSomething = true;
            }
            else
            if (collision.gameObject.tag == null)
            {
                return;
            }
            else
            {
                // must have hit terrain
                hasHitSomething = true;
                if (!currentlyReturning)
                {
                    hitSoundSource.GetComponent<AudioSource>().clip = (PickSound(terrainHitSounds));
                    hitSoundSource.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    private AudioClip PickSound(List<AudioClip> audioClips)
    {
        int randomNumber = Random.Range(0, audioClips.Count);

        return audioClips[randomNumber];

    }
}
