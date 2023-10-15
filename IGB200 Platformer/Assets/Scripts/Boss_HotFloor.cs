using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HotFloor : MonoBehaviour
{
    [SerializeField] private float upPositionAmount = 1.5f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private Collider2D allowedBrokenFloorArea;

    private Vector3 startingPos;
    private Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        endPos = new Vector3(startingPos.x, startingPos.y + upPositionAmount, startingPos.z);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Activate()
    {
        particleEffect.Play();
        StartCoroutine(MoveUp());
    }

    public void DeActivate()
    {
        particleEffect.Stop();
        StartCoroutine(MoveDown());
    }

    IEnumerator MoveUp()
    {
        float timer = 0f;

        while (timer < moveSpeed)
        {
            transform.position = Vector2.Lerp(startingPos, endPos, timer / moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MoveDown()
    {
        float timer = 0f;

        while (timer < moveSpeed)
        {
            transform.position = Vector2.Lerp(endPos, startingPos, timer / moveSpeed);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public Collider2D ReturnAllowedArea()
    {
        return allowedBrokenFloorArea;
    }
}
