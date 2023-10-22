using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerLevel3 : MonoBehaviour
{
    [SerializeField] private GameObject puzzleCamera;
    private CameraClampLevel3 cameraScript;
    private bool triggerDone = false;

    void Start()
    {
        cameraScript = puzzleCamera.GetComponent<CameraClampLevel3>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !triggerDone)
        {
            puzzleCamera.SetActive(true);
            cameraScript.StartPan();
        }
    }

    public void EndPan()
    {
        puzzleCamera.SetActive(false);
        triggerDone = true;
    }
}