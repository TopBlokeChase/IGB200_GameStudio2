using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [TextArea]
    public string note = "If loading a scene, enter the correct scene number below";
    public int sceneToLoad;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);

        //Ensure this is reset at all times when loading diff scenes
        Time.timeScale = 1;
    }
}
