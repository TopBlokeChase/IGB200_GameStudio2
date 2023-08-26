using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    private bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseMenuCanvas.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
            }
            else
            {
                UnPause();
            }
        }
    }

    public void UnPause()
    {
        pauseMenuCanvas.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }
}
