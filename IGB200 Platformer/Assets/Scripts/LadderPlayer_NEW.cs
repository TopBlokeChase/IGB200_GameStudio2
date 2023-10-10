using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LadderPlayer_NEW : MonoBehaviour
{
    public GameObject ladderInteract;
    public GameObject playerLadderOutline;
    public GameObject playerLadderPrefab;
    public TMP_Text placementText;

    private bool hasPlacedLadder;
    private bool inPlacementMode;
    private bool invalidPlacement;
    private bool invalidHeight;

    public LadderPlayer_CollisionChecker collisionChecker;
    public LadderPlayer_CollisionChecker floorCollisionChecker;

    private GameObject placedLadder;

    private string initialPlacementText;
    private string invalidPlacementText = "Invalid Placement!";

    private PlayerMovement playerMovement;

    public void RemoveLadder()
    {
        Destroy(placedLadder);
        hasPlacedLadder = false;
    }

    public void StopPlacementMode()
    {
        if (inPlacementMode)
        {
            inPlacementMode = false;
            playerLadderOutline.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPlacementText = placementText.text;
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerMovement.isInteracting != true)
            {
                if (!inPlacementMode && !hasPlacedLadder)
                {
                    inPlacementMode = true;
                    playerLadderOutline.SetActive(true);
                }
                else if (inPlacementMode && !hasPlacedLadder)
                {
                    inPlacementMode = false;
                    playerLadderOutline.SetActive(false);
                }
                else if (!inPlacementMode && hasPlacedLadder)
                {
                    if (playerMovement.isInteracting != true)
                    {
                        RemoveLadder();
                    }
                }
            }
        }

        if (inPlacementMode)
        {
            if (playerMovement.isInteracting == true)
            {
                inPlacementMode = false;
                playerLadderOutline.SetActive(false);
            }

            if (playerMovement.isLookingLeft)
            {
                playerLadderOutline.transform.rotation = Quaternion.Euler(0, -this.transform.rotation.y, 0);
            }
            else
            {
                playerLadderOutline.transform.rotation = Quaternion.Euler(Vector3.zero);
            }

            Debug.Log("Ladder placement Invalid? = " + collisionChecker.InvalidPlacement());
            Debug.Log("Ladder floor Invalid? = " + collisionChecker.InvalidFloorPlacement());
            if (collisionChecker.InvalidPlacement() == false && floorCollisionChecker.InvalidFloorPlacement() == false)
            {
                foreach(Transform child in playerLadderOutline.transform)
                {
                    SpriteRenderer renderer;
                    child.TryGetComponent<SpriteRenderer>(out renderer);

                    if (renderer != null)
                    {
                        renderer.color = Color.green;
                    }
                }

                placementText.text = initialPlacementText;

                if (!hasPlacedLadder && Input.GetMouseButtonDown(0))
                {
                    hasPlacedLadder = true;
                    inPlacementMode = false;
                    playerLadderOutline.SetActive(false);
                    placedLadder = Instantiate(playerLadderPrefab, playerLadderOutline.transform.position, Quaternion.Euler(Vector3.zero));
                }
            }
            else
            {
                foreach (Transform child in playerLadderOutline.transform)
                {
                    SpriteRenderer renderer;
                    child.TryGetComponent<SpriteRenderer>(out renderer);

                    if (renderer != null)
                    {
                        renderer.color = Color.red;
                    }                   
                }

                placementText.text = invalidPlacementText;
            }
        }
    }
}
