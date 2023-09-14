using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LadderPlayer : MonoBehaviour
{
    public GameObject ladderInteract;
    public int ladderHeight = 1;
    public Tile ladder;
    public Tile ladderGreen;
    public Tile ladderRed;
    public Tilemap terrain;
    public Tilemap playerPlacedLadders;

    private Vector3Int currentCell;
    private Vector3Int[] heightCells;
    private Vector3Int[] prevHeight;
    private Vector3Int nextCellHeight;

    private Vector3Int prev;

    private bool hasPlacedLadder;
    private bool inPlacementMode;
    private bool invalidPlacement;
    private bool invalidHeight;

    private float ladderInteractOffset = 0.5f;
    private GameObject playerLadder;



    private void LateUpdate()
    {
        if (inPlacementMode)
        {
            currentCell = playerPlacedLadders.WorldToCell(transform.position);
            currentCell.x += 1;

            Vector3Int belowGround = currentCell;
            belowGround.y += -1;

            if (currentCell != prev)
            {
                if (ladderHeight > 1)
                {
                    for (int i = 1; i < ladderHeight; i++)
                    {
                        Vector3Int currentCellHeight = currentCell;                       
                        currentCellHeight.y += i;
                        heightCells[i] = currentCellHeight;
                        nextCellHeight = heightCells[i];
                        nextCellHeight.y += 1;

                        
                        
                        if (terrain.GetTile(heightCells[i]) != null || terrain.GetTile(nextCellHeight) != null)
                        {
                            invalidHeight = true;
                        }
                        else
                        {
                            invalidHeight = false;
                        }
                    }
                }

                nextCellHeight = currentCell;
                nextCellHeight.y += 1;

                if (terrain.GetTile(currentCell) != null || terrain.GetTile(nextCellHeight) != null || terrain.GetTile(belowGround) == null)
                {
                    invalidPlacement = true;
                }
                else
                {
                    invalidPlacement = false;
                }

                if (invalidPlacement || invalidHeight)
                {
                    playerPlacedLadders.SetTile(currentCell, ladderRed);
                    if (ladderHeight > 1)
                    {
                        for (int i = 1; i < ladderHeight; i++)
                        {
                            playerPlacedLadders.SetTile(heightCells[i], ladderRed);
                        }
                    }
                }
                else
                {
                    playerPlacedLadders.SetTile(currentCell, ladderGreen);
                    if (ladderHeight > 1)
                    {
                        for (int i = 1; i < ladderHeight; i++)
                        {
                            playerPlacedLadders.SetTile(heightCells[i], ladderGreen);
                        }
                    }
                }

                playerPlacedLadders.SetTile(prev, null);

                if (ladderHeight > 1)
                {
                    for (int i = 1; i < ladderHeight; i++)
                    {
                        playerPlacedLadders.SetTile(prevHeight[i], null);
                    }
                }

                prev = currentCell;
                if (ladderHeight > 1)
                {
                    for (int i = 1; i < ladderHeight; i++)
                    {
                        prevHeight[i] = heightCells[i];
                    }
                }
            }
        }
    }

    public void RemoveLadder()
    {
        playerPlacedLadders.ClearAllTiles();
        Destroy(playerLadder);
        hasPlacedLadder = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        heightCells = new Vector3Int[ladderHeight];
        prevHeight = new Vector3Int[ladderHeight];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!inPlacementMode && !hasPlacedLadder)
            {
                inPlacementMode = true;
            }
            else if (inPlacementMode && !hasPlacedLadder)
            {
                inPlacementMode = false;
                playerPlacedLadders.ClearAllTiles();
            }
        }

        if (inPlacementMode)
        {
            if (!hasPlacedLadder && Input.GetMouseButtonDown(0))
            {
                if (!invalidHeight && !invalidPlacement)
                {
                    if (ladderHeight > 1)
                    {
                        for (int i = 1; i < ladderHeight; i++)
                        {
                            playerPlacedLadders.SetTile(heightCells[i], ladder);
                        }
                    }

                    playerPlacedLadders.SetTile(currentCell, ladder);

                    Vector3 cellPos = playerPlacedLadders.GetCellCenterWorld(currentCell);


                    playerLadder = Instantiate(ladderInteract, cellPos, Quaternion.identity);

                    playerLadder.GetComponent<Ladder>().SetPointATriggerPos(cellPos);
                    playerLadder.GetComponent<Ladder>().SetPointBTriggerPos(new Vector3(cellPos.x, cellPos.y + (2f * ladderHeight), cellPos.z));

                    if (ladderHeight > 1)
                    {
                        playerLadder.GetComponent<Ladder>().SetFloorPositionPlayerLadder(new Vector3(cellPos.x, cellPos.y + (2f * ladderHeight) - 1, cellPos.z));
                    }
                    else
                    {
                        playerLadder.GetComponent<Ladder>().SetFloorPositionPlayerLadder(new Vector3(cellPos.x, cellPos.y + (1f * ladderHeight), cellPos.z));
                    }
                    

                    hasPlacedLadder = true;
                    inPlacementMode = false;
                }
            }
        }
    }
}
