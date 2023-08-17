using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class HammerThrowTrail : MonoBehaviour
{
    private GameObject hammer;

    // Update is called once per frame
    void Update()
    {
        if (hammer != null)
        {
            transform.position = hammer.transform.position;
        }
    }

    public void SetHammerToFollow(GameObject hammer)
    {
        this.hammer = hammer;
    }
}
