using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject colliders;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stopTime = 1.5f;
    private Quaternion startRotation;
    private Quaternion currentRotation;
    private bool locked;
    private bool rotatedBackToStart;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void LockPlatform()
    {
        locked = true;
        currentRotation = transform.rotation;
        StartCoroutine(RotateToStart());
        colliders.SetActive(false);
    }


    IEnumerator RotateToStart()
    {
        float timer = 0;
        while (timer < stopTime)
        {
            transform.rotation = Quaternion.Lerp(currentRotation, startRotation, timer / stopTime);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = startRotation;
    }
}
