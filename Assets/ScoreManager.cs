﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI crystalsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI lost;
    private int crystals;
    private float endTime;
    public float levelMaxTime = 200f;
    public int levelScore = 50;
    private int finalScore = 0;
    private int displayScore;
    // Start is called before the first frame update
    void Start()
    {
        displayScore = 0;
        finalScore = CalculateScore();
        //time
        float t = Timer.endTime;
        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timeText.text = minutes + ":" + seconds;

        //crystals
        crystalsText.text = crystals.ToString();

        //lost robots
        lost.text = "0";
        StartCoroutine(ScoreUpdater());   
    }


    int CalculateScore()
    {
        endTime = Timer.endTime;
        crystals = CharacterStats.nbCrystals;
        return (Mathf.Max(0, (int)(levelMaxTime - endTime)) * levelScore) + crystals * levelScore;
    }

    private IEnumerator ScoreUpdater()
    {
        while(true)
        {
            if(displayScore < finalScore)
            {
                displayScore += 10; //Increment the display score by 1
                score.text = displayScore.ToString(); //Write it to the UI
            }
            yield return new WaitForSeconds(0.002f); // I used .2 secs but you can update it as fast as you want
        }
    }
}