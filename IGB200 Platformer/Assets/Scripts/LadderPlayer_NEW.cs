using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LadderPlayer_NEW : MonoBehaviour
{
    public GameObject ladderDustParticle;
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
    public GameObject distanceCheckerPrefab;

    private GameObject placedLadder;
    private GameObject distanceChecker;

    private string initialPlacementText;
    private string invalidPlacementText = "Invalid Placement!";

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerSounds playerSounds;

    public void RemoveLadder()
    {
        Destroy(placedLadder);       
        Destroy(distanceChecker);
        hasPlacedLadder = false;
        GameObject particle = Instantiate(ladderDustParticle,
                                new Vector3(placedLadder.transform.position.x, placedLadder.transform.position.y + 1.89f, placedLadder.transform.position.z), Quaternion.identity);
        particle.transform.parent = null;
        playerSounds.PlayLadderDestroy();
    }

    public void StopPlacementMode()
    {
        if (inPlacementMode)
        {
            inPlacementMode = false;
            playerLadderOutline.SetActive(false);
            playerCombat.CannotAttackToggle(true);
            playerSounds.PlayLadderDeactivate();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPlacementText = placementText.text;
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponentInChildren<PlayerCombat>();
        playerSounds = GetComponentInChildren<PlayerSounds>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.canUseTools)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (playerMovement.isInteracting != true)
                {
                    if (!inPlacementMode && !hasPlacedLadder)
                    {
                        inPlacementMode = true;
                        playerLadderOutline.SetActive(true);
                        playerCombat.CannotAttackToggle(false);
                        playerSounds.PlayLadderActivate();
                    }
                    else if (inPlacementMode && !hasPlacedLadder)
                    {
                        inPlacementMode = false;
                        playerLadderOutline.SetActive(false);
                        playerCombat.CannotAttackToggle(true);
                        playerSounds.PlayLadderDeactivate();
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
                    playerCombat.CannotAttackToggle(true);
                    playerLadderOutline.SetActive(false);
                    playerSounds.PlayLadderDeactivate();
                    inPlacementMode = false;
                }

                if (playerMovement.isLookingLeft)
                {
                    playerLadderOutline.transform.rotation = Quaternion.Euler(0, -this.transform.rotation.y, 0);
                }
                else
                {
                    playerLadderOutline.transform.rotation = Quaternion.Euler(Vector3.zero);
                }

                if (collisionChecker.InvalidPlacement() == false && floorCollisionChecker.InvalidFloorPlacement() == false)
                {
                    foreach (Transform child in playerLadderOutline.transform)
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
                        distanceChecker = Instantiate(distanceCheckerPrefab, placedLadder.transform.position, Quaternion.identity);
                        StartCoroutine(DelayAllowAttack());
                        playerSounds.PlayLadderPlace();

                        GameObject particle = Instantiate(ladderDustParticle,
                            new Vector3(placedLadder.transform.position.x, placedLadder.transform.position.y + 1.89f, placedLadder.transform.position.z), Quaternion.identity);
                        particle.transform.parent = null;
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

                    if (!hasPlacedLadder && Input.GetMouseButtonDown(0))
                    {
                        playerSounds.PlayLadderInvalidPlace();
                    }
                }
            }
        }
    }

    public bool ReturnHasPlaced()
    {
        return hasPlacedLadder;
    }

    IEnumerator DelayAllowAttack()
    {
        yield return new WaitForSeconds(1);
        playerCombat.CannotAttackToggle(true);
    }
}
