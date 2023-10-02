using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraClamp : MonoBehaviour
{
    [SerializeField] private GameObject normalCamera;
    [SerializeField] private GameObject cameraTrigger;
    [SerializeField] private GameObject player;

    [SerializeField] private bool followX; // If true, will track the player left and right
    [SerializeField] private bool followY; // If true, will track the player up and down

    private Vector2 triggerSize; // Size of the BoxCollider2D on the trigger
    private Vector2 xClamp; // Min and max x position of the camera
    private Vector2 yClamp; // Min and max y position of the camera

    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        triggerSize = cameraTrigger.GetComponent<BoxCollider2D>().size;

        float ortho = this.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        float ratio = 1920f / 1080f;

        xClamp = new Vector2(cameraTrigger.transform.position.x - (ortho*ratio),
                                cameraTrigger.transform.position.x + (ortho * ratio));
        yClamp = new Vector2(cameraTrigger.transform.position.y - (ortho+4),
                                cameraTrigger.transform.position.y + (ortho + 4));
        Debug.Log(xClamp);
        Debug.Log(yClamp);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(cameraTrigger.transform.position.x, cameraTrigger.transform.position.x, -10);
        if (followX)
        {
            position.x = Mathf.Clamp(player.transform.position.x,
                                xClamp.x, xClamp.y);
        }

        if (followY)
        {
            position.y = Mathf.Clamp(player.transform.position.y,
                                yClamp.x, yClamp.y);
        }
        this.transform.position = position;
    }
}