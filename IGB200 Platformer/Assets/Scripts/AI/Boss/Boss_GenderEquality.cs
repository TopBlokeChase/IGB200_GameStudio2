using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_GenderEquality : MonoBehaviour
{
    public enum BossType
    {
        GenderEquality,
        Harassment,
        MentalHealth
    }

    [Header("BossType & References")]
    [SerializeField] private BossType bossType;
    [SerializeField] private MusicHandler musicHandler;
    [SerializeField] private BossSounds bossSounds;
    [SerializeField] private GameObject basicEnemyGroup;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject flyingDisc;
    [SerializeField] private GameObject wordAttackPrefab;
    [SerializeField] private GameObject brokenGround;
    [SerializeField] private GameObject hotFloor;
    [SerializeField] private GameObject slamCollider;
    [SerializeField] private GameObject laserBeam;
    [SerializeField] private GameObject bossCamera;

    [Header("ParticleFX")]
    [SerializeField] private GameObject particleSlamDust;
    [SerializeField] private GameObject particleLaserRadial;
    [SerializeField] private GameObject particleLaserLine;

    [Header("Slam Camera Shake Settings")]
    [SerializeField] private float slamCameraShakeAmount;
    [SerializeField] private float slamCameraShakeDuration;

    [Header("Laser Camera Shake Settings")]
    [SerializeField] private float laserCameraShakeAmount;
    [SerializeField] private float laserCameraShakeDuration;

    [Header("Hot Ground Camera Shake Settings")]
    [SerializeField] private float hotCameraShakeAmount;
    [SerializeField] private float hotCameraShakeDuration;

    [SerializeField] private LayerMask ground;

    //[SerializeField] private float idleBobAmount;
    //[SerializeField] private float idleBobSpeed;
    [Header("Boss Movement Speed")]
    [SerializeField] private float movementSpeed;

    [Header("Boss State Settings")]
    [SerializeField] private float stateChangeTime = 5f;

    [Header("Slam Attack Settings")]
    [SerializeField] private float moveAbovePlayerSpeed;
    [SerializeField] private float slamSizeChangeDuration;
    [SerializeField] private float slamSizeChangeEndMultiplier = 1.26f;
    [SerializeField] private float slamAttackDownDuration;
    [SerializeField] private float slamAttackRestDuration;
    [SerializeField] private float slamAttackReturnDuration;

    [Header("Laser Attack Settings")]
    [SerializeField] private float laserAttackBeamLength;
    [SerializeField] private float laserAttackAngleAmount;
    [SerializeField] private float laserAttackMoveIntoPosSpeed;
    [SerializeField] private float laserAttackBeamStartSpeed;
    [SerializeField] private float laserAttackRotateDelay;
    [SerializeField] private float laserAttackRotateSpeed;
    [SerializeField] private float laserAttackBeamRetractSpeed;
    [SerializeField] private float laserAttackRotateBackSpeed;

    [Header("Hot Ground Attack Settings")]
    [SerializeField] private float bloomAmount = 25f;
    [SerializeField] private float hotSizeChangeDuration;
    [SerializeField] private float hotSizeChangeEndMultiplier = 1.33f;
    [SerializeField] private float hotAttackDownDuration;
    [SerializeField] private float hotAttackRestDuration;
    [SerializeField] private float hotAttackAfterSizeRestDuration;
    [SerializeField] private float hotAttackReturnDuration;
    [SerializeField] private float hotAttackPivotOffset;

    private string state;

    private bool needsToChase;
    private bool isBusy;

    private bool isMovingAbovePlayer;

    private float stateChangeTimer;
    private float laughTimer;
    private float laughTime = 6f;

    private Vector3 originalPosition;
    private float originalHeight;
    private Vector3 spriteOriginalSize;
    private Quaternion originalRotation;

    private BossChaseDistance bossChaseDistance;

    private GameObject player;

    private bool bossFacingLeft;

    private Animator animator;

    private GameObject brokenFloor;

    private Collider2D circleCollider2D;

    //private bool canIdle;

    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        originalPosition = transform.position;
        originalHeight = transform.position.y;
        originalRotation = transform.rotation;
        spriteOriginalSize = sprite.transform.localScale;
        bossChaseDistance = GetComponentInChildren<BossChaseDistance>();

        //canIdle = true;
        isBusy = true;

        animator = sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {      
        if (!isBusy)
        {
            laughTimer += Time.deltaTime;
            if (laughTimer >= laughTime)
            {
                bossSounds.PlayLaugh();
                laughTimer = 0;
            }

            ChasePlayer();
            stateChangeTimer += Time.deltaTime;
            if (stateChangeTimer >= stateChangeTime)
            {
                PickRandomState();
            }
        }

        //if (canIdle)
        //{
        //    IdleAnimation();
        //}
    }

    void PickRandomState()
    {
        int randomNumber = 0;

        if (bossType == BossType.GenderEquality)
        {
            randomNumber = Random.Range(1, 4);
        }

        if(bossType == BossType.Harassment)
        {
            randomNumber = Random.Range(1, 5);
        }

        if (bossType == BossType.MentalHealth)
        {
            randomNumber = Random.Range(1, 6);
        }
        

        // 1 - slamAttack, 2 - shootAttack

        if (randomNumber == 1)
        {
            isBusy = true;
            //canIdle = false;
            SlamAttack();
        }

        if (randomNumber == 2)
        {
            isBusy = true;
            //canIdle = true;
            ShootAttack();
        }

        if (randomNumber == 3)
        {
            isBusy = true;
            //canIdle = false;
            LaserAttack();
        }

        if (randomNumber == 4)
        {
            isBusy = true;
            //canIdle = false;
            HotGroundAttack();
        }

        if (randomNumber == 5)
        {
            isBusy = true;
            //canIdle = false;
            ShootWordAttack();
        }
    }

    //void IdleAnimation()
    //{
    //    float yAxis = Mathf.PingPong(Time.time * idleBobSpeed, 1) * idleBobAmount - idleBobAmount / 2;
    //    transform.position = new Vector3(transform.position.x, transform.position.y + yAxis, transform.position.z);
    //}


    void SlamAttack()
    {
        animator.SetBool("SlamAttack", true);
        StartCoroutine(SlamAttackCoroutine());
    }

    void ShootAttack()
    {
        Instantiate(flyingDisc, transform.position, transform.rotation);
        bossSounds.PlayFlyingDisc();
        animator.SetTrigger("Shoot");
        stateChangeTimer = 0;
        isBusy = false;
    }

    void ShootWordAttack()
    {
        Instantiate(wordAttackPrefab, transform.position, transform.rotation);
        bossSounds.PlayWordAttack();
        animator.SetTrigger("Shoot");
        stateChangeTimer = 0;
        isBusy = false;
    }

    void LaserAttack()
    {
        animator.SetBool("LaserBeam", true);
        StartCoroutine(LaserAttackCoroutine());
    }

    void HotGroundAttack()
    {
        animator.SetBool("HotFloor", true);
        StartCoroutine(HotGroundAttackCoroutine());
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

    public void SetBusy()
    {
        StopAllCoroutines();
        isBusy = true;
        //canIdle = true;
    }

    public void SetNotBusy()
    {
        musicHandler.PlayBossMusic();
        StopAllCoroutines();
        isBusy = false;
        //canIdle = true;
    }

    public void ResetAll()
    {
        player.GetComponent<PlayerMovement>().canUseTools = true;
        musicHandler.PlayNormalMusic();
        StopAllCoroutines();

        isBusy = true;
        slamCollider.GetComponent<BoxCollider2D>().enabled = false;
        laserBeam.SetActive(false);
        particleLaserLine.GetComponent<ParticleSystem>().Stop();
        particleLaserRadial.GetComponent<ParticleSystem>().Stop();

        bossSounds.StopAllSounds(false);

        animator.SetBool("LaserBeam", false);
        animator.SetBool("HotFloor", false);
        animator.SetBool("SlamAttack", false);

        transform.position = originalPosition;
        transform.rotation = originalRotation;
        sprite.transform.localScale = spriteOriginalSize;
        sprite.GetComponent<SpriteRenderer>().flipX = false;

        if (bossType == BossType.Harassment || bossType == BossType.MentalHealth)
        {
            this.gameObject.GetComponent<Enemy>().Bloom(bloomAmount, false);
            hotFloor.GetComponent<Boss_HotFloor>().DeActivate();

            if (brokenFloor != null)
            {
                Destroy(brokenFloor);
            }
        }
        //canIdle = true;
    }

    // Moves boss to original height and idles animation again
    public void EndFight()
    {
        player.GetComponent<PlayerMovement>().canUseTools = true;
        musicHandler.PlayNormalMusic();
        StopAllCoroutines();

        isBusy = true;
        animator.SetBool("LaserBeam", false);
        animator.SetBool("HotFloor", false);
        animator.SetBool("SlamAttack", false);

        transform.rotation = originalRotation;
        slamCollider.GetComponent<BoxCollider2D>().enabled = false;
        laserBeam.SetActive(false);
        particleLaserLine.GetComponent<ParticleSystem>().Stop();
        particleLaserRadial.GetComponent<ParticleSystem>().Stop();

        bossSounds.StopAllSounds(true);

        if (bossType == BossType.Harassment || bossType == BossType.MentalHealth)
        {
            this.gameObject.GetComponent<Enemy>().Bloom(bloomAmount, false);
            hotFloor.GetComponent<Boss_HotFloor>().DeActivate();

            if (brokenFloor != null)
            {
                Destroy(brokenFloor);
            }
        }
    
        //face boss towards player one final time
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

        basicEnemyGroup.SetActive(false);

        StartCoroutine(MoveToOriginalHeight());
    }

    IEnumerator MoveToOriginalHeight()
    {
        float timer = 0;

        Vector3 startingPosition = transform.position;
        Vector3 posToMove = new Vector3(startingPosition.x, originalHeight, startingPosition.z);

        Vector3 startAngle = transform.eulerAngles;
        Vector3 endAngle = Vector3.zero;

        while (timer < laserAttackMoveIntoPosSpeed)
        {
            transform.position = Vector2.Lerp(startingPosition, posToMove, timer / laserAttackMoveIntoPosSpeed);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startAngle), Quaternion.Euler(endAngle), timer / laserAttackMoveIntoPosSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator SlamAttackCoroutine()
    {       
        float timer = 0;
        Vector3 startingPosition = transform.position;
        Vector3 abovePlayerPos = player.transform.position;

        Vector3 startingScale = sprite.transform.localScale;

        bossSounds.PlaySlamAttackWhoosh();

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
            sprite.transform.localScale = Vector2.Lerp(startingScale, new Vector3 (slamSizeChangeEndMultiplier * startingScale.x, slamSizeChangeEndMultiplier * startingScale.x, slamSizeChangeEndMultiplier * startingScale.x), timer / slamSizeChangeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        circleCollider2D.enabled = false;

        timer = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ground);
        Debug.DrawRay(transform.position, Vector2.down, Color.yellow);
       
        slamCollider.GetComponent<BoxCollider2D>().enabled = true;

        Vector3 slamPosition = hit.point + new Vector2(0, GetComponent<Renderer>().bounds.size.y / 2);

        while (timer < slamAttackDownDuration)
        {
            transform.position = Vector2.Lerp(startingPosition, slamPosition, timer / slamAttackDownDuration);
            sprite.transform.localScale = Vector2.Lerp(new Vector3(slamSizeChangeEndMultiplier * startingScale.x, slamSizeChangeEndMultiplier * startingScale.x, slamSizeChangeEndMultiplier * startingScale.x), startingScale, timer / slamAttackDownDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        bossSounds.PlaySlamAttackSlam();

        particleSlamDust.GetComponent<ParticleSystem>().Play();

        timer = 0;
        bossCamera.GetComponent<CameraShake>().ShakeCamera(slamCameraShakeAmount, slamCameraShakeDuration);
        slamCollider.GetComponent<BoxCollider2D>().enabled = false;     

        while (timer < slamAttackRestDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        circleCollider2D.enabled = true;

        timer = 0;

        while (timer < slamAttackReturnDuration)
        {
            transform.position = Vector2.Lerp(slamPosition, new Vector2(startingPosition.x, originalHeight), timer / slamAttackReturnDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        stateChangeTimer = 0;
        isBusy = false;
        //canIdle = true;

        animator.SetBool("SlamAttack", false);
        yield return null;
    }

    IEnumerator LaserAttackCoroutine()
    {       
        float timer = 0;

        Vector3 startingPosition = transform.position;
        Vector3 posToMove = new Vector3(startingPosition.x, originalHeight, startingPosition.z);

        Vector3 startAngle = transform.eulerAngles;
        Vector3 angleToRotateTo;

        if (bossFacingLeft)
        {
            angleToRotateTo = new Vector3(startAngle.x, startAngle.y, startAngle.z + -20);

            particleLaserLine.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            particleLaserRadial.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            angleToRotateTo = new Vector3(startAngle.x, startAngle.y, startAngle.z + 20);

            particleLaserLine.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            particleLaserRadial.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        while (timer < laserAttackMoveIntoPosSpeed)
        {
            transform.position = Vector2.Lerp(startingPosition, posToMove, timer / laserAttackMoveIntoPosSpeed);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startAngle), Quaternion.Euler(angleToRotateTo), timer / laserAttackMoveIntoPosSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        laserBeam.SetActive(true);

        particleLaserRadial.GetComponent<ParticleSystem>().Play();

        bossSounds.PlayLaserBurst();

        Vector3 beamEndPos = beamEndPos = new Vector3(laserAttackBeamLength, 0, 0); 

        if (bossFacingLeft)
        {
            laserBeam.transform.rotation = Quaternion.Euler(0,0, -20);
        }
        else
        {         
            laserBeam.transform.rotation = Quaternion.Euler(0, 180, -20);
        }

        while (timer < laserAttackBeamStartSpeed)
        {
            laserBeam.GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(Vector3.zero, beamEndPos, timer / laserAttackBeamStartSpeed));
            timer += Time.deltaTime;
            yield return null;
        }

        bossSounds.PlayLaserLoop();

        particleLaserLine.GetComponent<ParticleSystem>().Play();

        timer = 0;

        while (timer < laserAttackRotateDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        Vector3 currentAngle = transform.eulerAngles;

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
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(currentAngle), Quaternion.Euler(angleToRotateTo), timer / laserAttackRotateSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        particleLaserLine.GetComponent<ParticleSystem>().Stop();
        particleLaserRadial.GetComponent<ParticleSystem>().Stop();
        

        while (timer < laserAttackBeamRetractSpeed)
        {
            laserBeam.GetComponent<LineRenderer>().SetPosition(1, Vector3.Lerp(beamEndPos, Vector3.zero, timer / laserAttackBeamRetractSpeed));
            timer += Time.deltaTime;
            yield return null;
        }


        bossSounds.StopLaserLoop();
        bossSounds.PlayLaserBurst();

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
        //canIdle = true;

        animator.SetBool("LaserBeam", false);
        yield return null;
    }

    IEnumerator HotGroundAttackCoroutine()
    {
        float timer = 0;
        Vector3 startingPosition = transform.position;

        Vector3 startingScale = sprite.transform.localScale;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ground);
        Debug.DrawRay(transform.position, Vector2.down, Color.yellow);

        Vector3 slamPosition = hit.point + new Vector2(0, GetComponent<Renderer>().bounds.size.y / 2);

        bossCamera.GetComponent<CameraShake>().ShakeCamera(hotCameraShakeAmount, hotCameraShakeDuration);

        bossSounds.PlayHotFloorRumble();     

        while (timer < hotAttackDownDuration)
        {
            transform.position = Vector2.Lerp(startingPosition, slamPosition, timer / hotAttackDownDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        brokenFloor = Instantiate(brokenGround, transform.position, Quaternion.identity);
        foreach (Transform child in brokenFloor.transform)
        {
            child.gameObject.GetComponentInChildren<Platform_BrokenFloor>().CheckEndPositionValid(hotFloor.GetComponent<Boss_HotFloor>().ReturnAllowedArea());
        }

        bossSounds.PlayPlatformAppear();
        
        timer = 0;

        while (timer < hotAttackRestDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        this.gameObject.GetComponent<Enemy>().Bloom(bloomAmount, true);
        hotFloor.GetComponent<Boss_HotFloor>().Activate();

        bossSounds.PlayHotFloorBubble();

        while (timer < hotSizeChangeDuration)
        {
            sprite.transform.localScale = Vector2.Lerp(startingScale, new Vector3(hotSizeChangeEndMultiplier * startingScale.x, hotSizeChangeEndMultiplier * startingScale.x, hotSizeChangeEndMultiplier * startingScale.x), timer / hotSizeChangeDuration);
            transform.position = Vector2.Lerp(slamPosition, new Vector2(slamPosition.x, slamPosition.y + hotAttackPivotOffset), timer / hotSizeChangeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        this.gameObject.GetComponent<Enemy>().Bloom(bloomAmount, false);
        hotFloor.GetComponent<Boss_HotFloor>().DeActivate();


        bossSounds.StopHotFloorBubble();


        while (timer < hotAttackAfterSizeRestDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        foreach(Transform child in brokenFloor.transform)
        {
            child.gameObject.GetComponentInChildren<Platform_BrokenFloor>().Finish();
        }

        bossSounds.PlayPlatformDisappear();
        bossSounds.StopHotFloorRumble();

        Vector3 currentScale = sprite.transform.localScale;
        Vector3 currentPos = transform.position;

        while (timer < hotAttackReturnDuration)
        {
            transform.position = Vector2.Lerp(currentPos, new Vector2(startingPosition.x, originalHeight), timer / hotAttackReturnDuration);
            sprite.transform.localScale = Vector2.Lerp(currentScale, startingScale, timer / hotAttackReturnDuration);
            timer += Time.deltaTime;
            yield return null;
        }       
       
        stateChangeTimer = 0;
        isBusy = false;
        //canIdle = true;

        animator.SetBool("HotFloor", false);
        yield return null;
    }
}
