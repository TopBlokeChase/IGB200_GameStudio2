using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Boss_GenderEquality : MonoBehaviour
{   
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject flyingDisc;
    [SerializeField] private GameObject slamCollider;
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject bossCamera;

    [SerializeField] private float slamCameraShakeAmount;
    [SerializeField] private float slamCameraShakeDuration;

    [SerializeField] private float laserCameraShakeAmount;
    [SerializeField] private float laserCameraShakeDuration;

    [SerializeField] private LayerMask ground;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float moveAbovePlayerSpeed;

    [SerializeField] private float stateChangeTime = 5f;

    [SerializeField] private float slamSizeChangeDuration;
    [SerializeField] private float slamAttackDownDuration;
    [SerializeField] private float slamAttackRestDuration;
    [SerializeField] private float slamAttackReturnDuration;

    [SerializeField] private float laserAttackBeamLength;
    [SerializeField] private float laserAttackAngleAmount;
    [SerializeField] private float laserAttackMoveIntoPosSpeed;
    [SerializeField] private float laserAttackBeamStartSpeed;
    [SerializeField] private float laserAttackRotateDelay;
    [SerializeField] private float laserAttackRotateSpeed;
    [SerializeField] private float laserAttackBeamRetractSpeed;
    [SerializeField] private float laserAttackRotateBackSpeed;

    private string state;

    private bool needsToChase;
    private bool isBusy;

    private bool isMovingAbovePlayer;

    private float stateChangeTimer;
    private float originalHeight;
    private BossChaseDistance bossChaseDistance;

    private GameObject player;

    private bool bossFacingLeft;
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
        int randomNumber = Random.Range(1, 4);

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

        if (randomNumber == 3)
        {
            isBusy = true;
            LaserAttack();
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

    void LaserAttack()
    {
        StartCoroutine(LaserAttackCoroutine());
    }

    void ChasePlayer()
    {
        if (player.transform.position.x > transform.position.x)
        {
            bossFacingLeft = false;
            sprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            bossFacingLeft = true;
            sprite.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (bossChaseDistance.PlayerInRange() == false && !isMovingAbovePlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
    }

    public void ResetAll()
    {
        StopAllCoroutines();
        isBusy = false;
        slamCollider.GetComponent<BoxCollider2D>().enabled = false;
    }

    IEnumerator SlamAttackCoroutine()
    {
        float timer = 0;
        Vector3 startingPosition = transform.position;
        Vector3 abovePlayerPos = player.transform.position;

        while (timer < moveAbovePlayerSpeed)
        {
            transform.position = Vector2.Lerp(startingPosition, new Vector3(abovePlayerPos.x, originalHeight, abovePlayerPos.z), timer / moveAbovePlayerSpeed);
            timer += Time.deltaTime;
            yield return null;           
        }


        timer = 0;
        startingPosition = transform.position;

        while (timer < slamSizeChangeDuration)
        {
            sprite.transform.localScale = Vector2.Lerp(Vector3.one, new Vector3 (1.5f, 1.5f, 1.5f), timer / slamSizeChangeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ground);
        Debug.DrawRay(transform.position, Vector2.down, Color.yellow);
       
        slamCollider.GetComponent<BoxCollider2D>().enabled = true;

        Vector3 slamPosition = hit.point + new Vector2(0, GetComponent<Renderer>().bounds.size.y / 2);

        while (timer < slamAttackDownDuration)
        {
            transform.position = Vector2.Lerp(startingPosition, slamPosition, timer / slamAttackDownDuration);
            sprite.transform.localScale = Vector2.Lerp(new Vector3(1.5f, 1.5f, 1.5f), Vector3.one, timer / slamAttackDownDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        bossCamera.GetComponent<CameraShake>().ShakeCamera(slamCameraShakeAmount, slamCameraShakeDuration);
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

    IEnumerator LaserAttackCoroutine()
    {
        float timer = 0;

        Vector3 startingPosition = transform.position;
        Vector3 posToMove = new Vector3(startingPosition.x, originalHeight, startingPosition.z);

        while (timer < laserAttackMoveIntoPosSpeed)
        {
            transform.position = Vector2.Lerp(startingPosition, posToMove, timer / laserAttackMoveIntoPosSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        laserBeam.SetActive(true);

        Vector3 beamEndPos = beamEndPos = new Vector3(laserAttackBeamLength, 0, 0); 

        if (bossFacingLeft)
        {
            laserBeam.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {         
            laserBeam.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        while (timer < laserAttackBeamStartSpeed)
        {
            laserBeam.GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(Vector3.zero, beamEndPos, timer / laserAttackBeamStartSpeed));
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < laserAttackRotateDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        Vector3 startAngle = transform.eulerAngles;
        Vector3 angleToRotateTo;

        if (bossFacingLeft)
        {
            angleToRotateTo = new Vector3(startAngle.x, startAngle.y, laserAttackAngleAmount);
        }
        else
        {
            angleToRotateTo = new Vector3(startAngle.x, startAngle.y, laserAttackAngleAmount * -1);
        }

        bossCamera.GetComponent<CameraShake>().ShakeCamera(laserCameraShakeAmount, laserCameraShakeDuration);

        while (timer < laserAttackRotateSpeed)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startAngle), Quaternion.Euler(angleToRotateTo), timer / laserAttackRotateSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        while (timer < laserAttackBeamRetractSpeed)
        {
            laserBeam.GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(beamEndPos, Vector3.zero, timer / laserAttackBeamRetractSpeed));
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        laserBeam.SetActive(false);

        while (timer < laserAttackRotateBackSpeed)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(angleToRotateTo), Quaternion.Euler(startAngle), timer / laserAttackRotateBackSpeed);          
            timer += Time.deltaTime;
            yield return null;
        }
        
        stateChangeTimer = 0;
        isBusy = false;

        yield return null;
    }
}
