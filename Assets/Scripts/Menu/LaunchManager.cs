using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchManager : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
    }

    public void play()
    {
        SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
