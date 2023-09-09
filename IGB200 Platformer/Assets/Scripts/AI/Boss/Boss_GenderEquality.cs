using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boss_GenderEquality : MonoBehaviour
{   
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject flyingDisc;
    [SerializeField] private GameObject slamCollider;
    [SerializeField] private GameObject bossCamera;
    [SerializeField] private float cameraShakeAmount;
    [SerializeField] private float cameraShakeDuration;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float stateChangeTime = 5f;
    [SerializeField] private float slamSizeChangeDuration;
    [SerializeField] private float slamAttackDownDuration;
    [SerializeField] private float slamAttackRestDuration;
    [SerializeField] private float slamAttackReturnDuration;

    private string state;

    private bool needsToChase;
    private bool isBusy;

    private float stateChangeTimer;
    private float originalHeight;
    private BossChaseDistance bossChaseDistance;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        originalHeight = transform.position.y;
        bossChaseDistance = GetComponentInChildren<BossChaseDistance>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBusy)
        {
            ChasePlayer();
            stateChangeTimer += Time.deltaTime;
            if (stateChangeTimer >= stateChangeTime)
            {
                PickRandomState();
            }
        }
    }

    void PickRandomState()
    {
        int randomNumber = Random.Range(1, 3);

        // 1 - slamAttack, 2 - shootAttack

        if (randomNumber == 1)
        {
            isBusy = true;
            SlamAttack();
        }

        if (randomNumber == 2)
        {
            isBusy = true;
            ShootAttack();
        }
    }


    void SlamAttack()
    {
        StartCoroutine(SlamAttackCoroutine());
    }

    void ShootAttack()
    {
        Instantiate(flyingDisc, transform.position, transform.rotation);
        stateChangeTimer = 0;
        isBusy = false;
    }

    void ChasePlayer()
    {
        if (bossChaseDistance.PlayerInRange() == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    IEnumerator SlamAttackCoroutine()
    {
        Vector3 startingPosition = transform.position;
        float timer = 0;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ground);
        Debug.DrawRay(transform.position, Vector2.down, Color.yellow);

        Vector3 slamPosition = hit.point + new Vector2(0, GetComponent<Renderer>().bounds.size.y / 2);

        while (timer < slamSizeChangeDuration)
        {
            sprite.transform.localScale = Vector2.Lerp(Vector3.one, new Vector3 (1.5f, 1.5f, 1.5f), timer / slamSizeChangeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        slamCollider.GetComponent<BoxCollider2D>().enabled = true;

        while (timer < slamAttackDownDuration)
        {
            transform.position = Vector2.Lerp(startingPosition, slamPosition, timer / slamAttackDownDuration);
            sprite.transform.localScale = Vector2.Lerp(new Vector3(1.5f, 1.5f, 1.5f), Vector3.one, timer / slamAttackDownDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        bossCamera.GetComponent<CameraShake>().ShakeCamera(cameraShakeAmount, cameraShakeDuration);
        slamCollider.GetComponent<BoxCollider2D>().enabled = false;



        while (timer < slamAttackRestDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < slamAttackReturnDuration)
        {
            transform.position = Vector2.Lerp(slamPosition, new Vector2(startingPosition.x, originalHeight), timer / slamAttackReturnDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        stateChangeTimer = 0;
        isBusy = false;

        yield return null;
    }
}
