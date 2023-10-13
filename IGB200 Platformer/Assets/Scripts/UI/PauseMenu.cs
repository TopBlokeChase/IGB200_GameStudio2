using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;

    private bool isPaused;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                pauseMenuCanvas.SetActive(true);
                player.GetComponent<PlayerMovement>().isInMenu = true;
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
        player.GetComponent<PlayerMovement>().isInMenu = false;
        isPaused = false;
        Time.timeScale = 1;
    }
}
