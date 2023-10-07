using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LadderPlayer_CollisionChecker : MonoBehaviour
{
    public bool isFloorChecker;

    private bool invalidPlacement;
    private bool invalidFloorPlacement;

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!isFloorChecker)
        {
            if (collision.tag == "Terrain")
            {
                invalidPlacement = true;
            }
        }
        else
        {
            if (collision.tag == "Terrain")
            {
                invalidFloorPlacement = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isFloorChecker)
        {
            if (collision.tag == "Terrain")
            {
                invalidPlacement = false;
            }
        }
        else
        {
            if (collision.tag == "Terrain")
            {
                invalidFloorPlacement = true;
            }
        }
    }

    public bool InvalidPlacement()
    {
        return invalidPlacement;
    }

    public bool InvalidFloorPlacement()
    {
        return invalidFloorPlacement;
    }
}
