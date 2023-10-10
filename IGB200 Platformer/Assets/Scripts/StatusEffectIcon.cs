using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIcon : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float yOffset = 2.61f;
    [SerializeField] private Image circularImage;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
    }

    public void SetTimeAndAnimate(float time, GameObject player)
    {
        this.player = player;
        this.time = time;
        StartCoroutine(ClockCountdown());
    }

    IEnumerator ClockCountdown()
    {
        float timer = 0f;

        while (timer <  time)
        {
            timer += Time.deltaTime;
            circularImage.fillAmount = Mathf.Lerp(1, 0, timer / time);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
