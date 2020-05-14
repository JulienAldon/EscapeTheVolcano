using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    private string currentScene;
    public static LaunchManager instance;

    private void Awake()
    {
        instance = this;
        SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive); // loadMenu scene
        currentScene = "Menu";
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
   
    public void LoadSelect()
    {
        LoadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1)));
        scenesLoading.Add(SceneManager.LoadSceneAsync("SelectionScene", LoadSceneMode.Additive));
        currentScene = "SelectionScene";
        StartCoroutine(GetSelectSceneLoadProgress());
    }
   
   public void LoadGame()
   {
        LoadingScreen.gameObject.SetActive(true);
        currentScene = "ScenePrincipale";
        scenesLoading.Add(SceneManager.UnloadSceneAsync("SelectionScene"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("ScenePrincipale", LoadSceneMode.Additive));
        StartCoroutine(GetGameSceneLoadProgress());
   }

   public void LoadWin()
   {
        currentScene = "WinScene";
        LoadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("ScenePrincipale"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("WinScene", LoadSceneMode.Additive));
        StartCoroutine(GetSelectSceneLoadProgress());
   }

   public void LoadLose()
   {
        currentScene = "LoseScene";
        LoadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("ScenePrincipale"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("LoseScene", LoadSceneMode.Additive));
        StartCoroutine(GetSelectSceneLoadProgress());
   }

   public void LoadMenu()
   {
        currentScene = "Menu";
        LoadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1)));
        scenesLoading.Add(SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive));
        StartCoroutine(GetSelectSceneLoadProgress());
   }

   public void LoadTutorial()
   {
        currentScene = "Tutorial";
        LoadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync("Menu"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive));
        StartCoroutine(GetSelectSceneLoadProgress());
   }

   public IEnumerator GetGameSceneLoadProgress()
   {
       for (int i=0; i <scenesLoading.Count; i++) {
           while(!scenesLoading[i].isDone)
           {
               yield return null;
           }
       }
              
       int a = 0;
       while(LevelGeneration.current == null || !LevelGeneration.current.isDone)
       {
           if (a > 4) {
               LoadingScreen.transform.GetChild(0).gameObject.SetActive(true);
           } if (a > 8) {
               LoadingScreen.transform.GetChild(1).gameObject.SetActive(true);               
           } if (a > 10) {
               LoadingScreen.transform.GetChild(2).gameObject.SetActive(true);                              
           }
           a++;
           yield return null;
       }
       LoadingScreen.gameObject.SetActive(false);
        LoadingScreen.transform.GetChild(0).gameObject.SetActive(false);
        LoadingScreen.transform.GetChild(1).gameObject.SetActive(false);               
        LoadingScreen.transform.GetChild(2).gameObject.SetActive(false);                              

       
       Timer.instance.started = true;
   }
   
   public IEnumerator GetSelectSceneLoadProgress()
   {
       int a = 0;
       for (int i=0; i <scenesLoading.Count; i++) {
           while(!scenesLoading[i].isDone)
           {
                if (a > 8) {
                    LoadingScreen.transform.GetChild(0).gameObject.SetActive(true);
                } if (a > 12) {
                    LoadingScreen.transform.GetChild(1).gameObject.SetActive(true);               
                } if (a > 15) {
                    LoadingScreen.transform.GetChild(2).gameObject.SetActive(true);                              
                }
                a++;
                yield return null;
           }
       }
       LoadingScreen.gameObject.SetActive(false);
         LoadingScreen.transform.GetChild(0).gameObject.SetActive(false);
        LoadingScreen.transform.GetChild(1).gameObject.SetActive(false);               
        LoadingScreen.transform.GetChild(2).gameObject.SetActive(false);                              
   }

    public void quit()
    {
        Application.Quit();
        Time.timeScale = 1f;
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
