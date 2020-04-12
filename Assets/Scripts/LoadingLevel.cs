using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevel : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 3f;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadGameOverScene()
    {
        reset();
        StartCoroutine(LoadLevel(3));
    }

    public void LoadMenuScene()
    {
        reset();
        StartCoroutine(LoadLevel(0));
    }

    public void LoadWinScene()
    {
        // reset();
        StartCoroutine(LoadLevel(4));
    }

    public void LoadSelectScene()
    {
        reset();
        StartCoroutine(LoadLevel(1));
    }

    public void reset()
    {
        Time.timeScale = 1f;
        Level.canWin = false;        
        Level.state = -1;
        Team.team = new Character[4];     
        Team.nbSelected = 0;
        PlayerAnimation.count = 0;
        CharacterStats.nbCrystals = 0;
        Timer.endTime = 0;
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
