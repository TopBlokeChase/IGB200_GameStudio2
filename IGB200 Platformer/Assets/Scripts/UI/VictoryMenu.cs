using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] AudioSource victorySoundSource;
    [SerializeField] GameObject victoryUI;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMenu(victorySoundSource.clip.length));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTimescale()
    {
        Time.timeScale = 1;
    }

    IEnumerator DelayMenu(float delay)
    {
        yield return new WaitForSeconds(delay);

        Time.timeScale = 0;
        victoryUI.SetActive(true);
    }
}
