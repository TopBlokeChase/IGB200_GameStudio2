using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] AudioSource victorySoundSource;
    [SerializeField] GameObject victoryUI;

    [Header("Only fill this ref if level 3")]
    [SerializeField] GameObject finalCutscenePrefab;
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

        if (ProgressTracker.currentLevel == 3)
        {
            finalCutscenePrefab.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            victoryUI.SetActive(true);
        }
    }
}
