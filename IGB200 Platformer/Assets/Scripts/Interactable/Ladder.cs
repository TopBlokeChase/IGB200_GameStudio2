using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("--ONLY TO BE USED IF PLAYER LADDER--")]
    public int ladderHeight = 1;
    [SerializeField] private float ladderPieceOffset;
    [SerializeField] private float floorOffset;
    [SerializeField] private GameObject ladderPieceSprite;
    [SerializeField] private BoxCollider2D ladderCollisionBody;

    [Header("References & Settings")]
    [SerializeField] private bool isPlayerPlacedLadder;
    [SerializeField] private GameObject playerLadderFloor;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    //[SerializeField] private GameObject pointC;
    [SerializeField] private GameObject UIInteractCanvas;
    [SerializeField] private float footStepSpeed = 0.1f;
    [SerializeField] private AudioSource ladderClimbFoot1;
    [SerializeField] private AudioSource ladderClimbFoot2;

    private GameObject pointToMoveTo;
    private GameObject player;
    private Rigidbody2D playerRB;

    private bool hasSnappedToInitialPoint;
    private bool hasReachedPointB;
    private bool isClimbing;

    private float footStepTimer;
    private bool playedFootstep1;

    //vars only used if player ladder
    
    private float initialLadderPiecePosY;
    private float initialLadderFloorPosY;

    private float floorHeight;

    // Start is called before the first frame update
    void Start()
    {
        if (isPlayerPlacedLadder)
        {
            initialLadderPiecePosY = ladderPieceSprite.transform.position.y;
            initialLadderFloorPosY = playerLadderFloor.transform.position.y;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();

        if (isPlayerPlacedLadder)
        {
            if (ladderHeight != 1)
            {
                floorHeight = 1 + ((ladderHeight - 1) * floorOffset);
                playerLadderFloor.transform.localPosition = new Vector3(0, floorHeight, 0);

                pointB.transform.localPosition = new Vector3(0, 2.16f * ladderHeight, 0);

                ladderCollisionBody.size = new Vector2(ladderCollisionBody.size.x, 2.73f * ladderHeight);

                ladderCollisionBody.gameObject.transform.localPosition = new Vector3(0, 1.33f * (ladderHeight - 1) + 0.44f);


                for (int i = 1; i < ladderHeight; i++)
                {
                    float height = i * ladderPieceOffset;
                    

                    GameObject newLadderPiece = Instantiate(ladderPieceSprite, Vector3.zero, ladderPieceSprite.transform.rotation, this.gameObject.transform);
                    newLadderPiece.transform.localPosition = new Vector3(0, height, 0);
                }                                           
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayerAlongLadder();
    }

    private void MovePlayerAlongLadder()
    {
        if (isClimbing)
        {
            footStepTimer += Time.deltaTime;

            if (footStepTimer >= footStepSpeed)
            {
                if (!playedFootstep1)
                {
                    ladderClimbFoot1.Play();
                    playedFootstep1 = true;
                    footStepTimer = 0;
                }
                else
                {
                    ladderClimbFoot2.Play();
                    playedFootstep1 = false;
                    footStepTimer = 0;
                }
            }

            if (pointToMoveTo == pointA)
            {
                Vector3 dir = (pointToMoveTo.transform.position - player.transform.position).normalized;
                //no need to move to point c, just move to b
                if (!hasSnappedToInitialPoint)
                {
                    player.transform.position = pointB.transform.position;
                    hasSnappedToInitialPoint = true;
                    playerRB.bodyType = RigidbodyType2D.Static;
                    player.GetComponent<PlayerMovement>().isInteracting = true;
                }

                if (player.transform.position != pointToMoveTo.transform.position)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, pointToMoveTo.transform.position, climbSpeed * Time.deltaTime);
                }
                else
                {
                    isClimbing = false;
                    hasSnappedToInitialPoint = false;
                    //enable player input
                    playerRB.bodyType = RigidbodyType2D.Dynamic;
                    player.GetComponent<PlayerMovement>().isInteracting = false;
                }
            }

            if (pointToMoveTo == pointB)
            {
                //need to move to point a and then c afterwards
                Vector3 dir = (pointToMoveTo.transform.position - player.transform.position).normalized;
                //no need to move to point c, just move to b
                if (!hasSnappedToInitialPoint)
                {
                    player.transform.position = pointA.transform.position;
                    hasSnappedToInitialPoint = true;
                    playerRB.bodyType = RigidbodyType2D.Static;
                    player.GetComponent<PlayerMovement>().isInteracting = true;
                }

                if (player.transform.position != pointToMoveTo.transform.position)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, pointToMoveTo.transform.position, climbSpeed * Time.deltaTime);
                }
                else
                {
                    isClimbing = false;
                    hasSnappedToInitialPoint = false;
                    //enable player input
                    playerRB.bodyType = RigidbodyType2D.Dynamic;
                    player.GetComponent<PlayerMovement>().isInteracting = false;
                }
            }

            //if (pointToMoveTo == pointC)
            //{
            //    //need to move to point a and then c afterwards
            //    Vector3 dir = (pointToMoveTo.transform.position - player.transform.position).normalized;

            //    if (player.transform.position != pointToMoveTo.transform.position)
            //    {
            //        player.transform.position = Vector3.MoveTowards(player.transform.position, pointToMoveTo.transform.position, climbSpeed * Time.deltaTime);
            //    }
            //    else
            //    {
            //        isClimbing = false;
            //        hasSnappedToInitialPoint = false;
            //        //enable player input
            //        playerRB.bodyType = RigidbodyType2D.Dynamic;
            //        player.GetComponent<PlayerMovement>().isInteracting = false;
            //    }
            //}
        }
    }

    public void SetPoint(Transform currentPoint)
    {
        if (currentPoint.position == pointA.transform.position)
        {
            pointToMoveTo = pointB;
        }

        if (currentPoint.position == pointB.transform.position)
        {
            pointToMoveTo = pointA;
        }

        footStepTimer = footStepSpeed;
        isClimbing = true;
    }

    public void EnableInteractUI(Transform pointToSetPosTo)
    {
        UIInteractCanvas.SetActive(true);
        UIInteractCanvas.transform.position = new Vector3(pointToSetPosTo.position.x, pointToSetPosTo.position.y + 1f, pointToSetPosTo.position.z);
    }

    public void DisableInteractUI()
    {
        UIInteractCanvas.SetActive(false);
    }

    public void SetPointATriggerPos(Vector3 pointToSetPosTo)
    {
        pointA.transform.position = pointToSetPosTo;
    }

    public void SetPointBTriggerPos(Vector3 pointToSetPosTo)
    {
        pointB.transform.position = pointToSetPosTo;
    }

    public void SetFloorPositionPlayerLadder(Vector3 pointToSetPosTo)
    {
        playerLadderFloor.transform.position = pointToSetPosTo;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!isPlayerPlacedLadder)
        {
            float labelOffsetY = 1f;
            Vector3 labelAPos = new Vector3(pointA.transform.position.x, pointA.transform.position.y - labelOffsetY, pointA.transform.position.z);
            Vector3 labelBPos = new Vector3(pointB.transform.position.x, pointB.transform.position.y - labelOffsetY, pointB.transform.position.z);
            //Vector3 labelCPos = new Vector3(pointC.transform.position.x, pointC.transform.position.y - labelOffsetY, pointC.transform.position.z);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointA.transform.position, 1);
            UnityEditor.Handles.Label(labelAPos, "Point A");

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointB.transform.position, 1);
            UnityEditor.Handles.Label(labelBPos, "Point B");

            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(pointC.transform.position, 1);
            //UnityEditor.Handles.Label(labelCPos, "Point C");
        }
    }
#endif
}
