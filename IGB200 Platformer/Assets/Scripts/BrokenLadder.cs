using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BrokenLadder : MonoBehaviour
{
    [SerializeField] private int amountOfNails;
    [SerializeField] private int amountOfHammers;
    [SerializeField] private TMP_Text amountOfNailsTxt;
    [SerializeField] private TMP_Text amountOfHammersTxt;
    [SerializeField] private GameObject nailsTickImg;
    [SerializeField] private GameObject hammerTickImg;
    [SerializeField] private Tilemap ladderLayerTilemap;
    [SerializeField] private Tile ladderHalfRepaired;
    [SerializeField] private Tile ladderFullyRepaired;
    [SerializeField] private GameObject ladderObject;

    private TileBase[] allTiles;
    private BoundsInt bounds;

    private bool nailsComplete;
    private bool hammersComplete;

    private bool isLadderHalfRepaired;
    // Start is called before the first frame update
    void Start()
    {
        bounds = ladderLayerTilemap.cellBounds;
        allTiles = ladderLayerTilemap.GetTilesBlock(bounds);

        amountOfNailsTxt.text = amountOfNails.ToString();
        amountOfHammersTxt.text = amountOfHammers.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLadderHalfRepaired && (nailsComplete || hammersComplete))
        {
            RepairLadder(ladderHalfRepaired);
            isLadderHalfRepaired = true;
        }

        if (nailsComplete && hammersComplete)
        {
            RepairLadder(ladderFullyRepaired);
            ladderObject.transform.parent = null;
            ladderObject.SetActive(true);
            transform.root.gameObject.SetActive(false);
        }
    }

    public void RemoveNailCount()
    {
        amountOfNails -= 1;

        if (amountOfNails <= 0)
        {
            nailsComplete = true;
            amountOfNailsTxt.gameObject.SetActive(false);
            nailsTickImg.gameObject.SetActive(true);
        }
        else
        {
            amountOfNailsTxt.text = amountOfNails.ToString();
        }
    }

    public void RemoveHammerCount()
    {
        amountOfHammers -= 1;

        if (amountOfHammers <= 0)
        {
            hammersComplete = true;
            amountOfHammersTxt.gameObject.SetActive(false);
            hammerTickImg.gameObject.SetActive(true);
        }
        else
        {
            amountOfHammersTxt.text = amountOfHammers.ToString();
        }
    }

    void RepairLadder(Tile tileToChangeTo)
    {
        for (int i = 0; i < bounds.size.x; i++)
        {
            for (int j = 0; j < bounds.size.y; j++)
            {
                TileBase tile = allTiles[i + j * bounds.size.x];

                if (tile != null)
                {
                    Vector3Int gridPos = new Vector3Int(i + bounds.xMin, j + bounds.yMin, bounds.z);
                    ladderLayerTilemap.SetTile(gridPos, tileToChangeTo);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Nail")
        {
            RemoveNailCount();
        }

        if (collision.gameObject.tag == "Hammer")
        {
            RemoveHammerCount();
        }
    }
}
