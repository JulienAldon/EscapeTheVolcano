using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCondition : MonoBehaviour
{
    private LoadingLevel load;
    private Timer timer;
    private AudioManager audio;

    void Start()
    {
        load = GameObject.Find("LevelLoader").GetComponent<LoadingLevel>();
        timer = GameObject.Find("TimeManager").GetComponent<Timer>();
		audio = FindObjectOfType<AudioManager> ();        
        
    } 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Level.canWin == true)
        {
            audio.Play ("win");														                    
            timer.Finnish();
            load.LoadWinScene();
            
        }
    }
}
