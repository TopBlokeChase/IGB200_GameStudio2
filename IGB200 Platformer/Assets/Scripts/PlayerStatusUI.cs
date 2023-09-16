using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private float displayTime = 4f;
    [SerializeField] private TMP_Text buffText;
    [SerializeField] private TMP_Text shieldAmount;

    private float timer;

    private GameObject player;
    private float yOffset;

    private Vector3 UIposition;

    // Update is called once per frame
    void Update()
    {
        UIposition = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
        timer += Time.deltaTime;

        if (timer >= displayTime)
        {
            Destroy(this.gameObject);
        }

        transform.position = UIposition;
    }

    public void InitiateText(string buffName, int shieldAmount)
    {
        buffText.text = buffName;
        this.shieldAmount.text = "+" + shieldAmount.ToString();
    }

    public void InitialisePosition(GameObject player, float yOffset)
    {
        this.player = player;
        this.yOffset = yOffset;
    }
}
