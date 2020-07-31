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
    public float minLevelTime = 60f;
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
        lost.text = (4 - Team.team.Length).ToString();
        StartCoroutine(ScoreUpdater());   
    }


    int CalculateScore()
    {
        endTime = Timer.endTime;
        crystals = CharacterStats.nbCrystals;
        float a = 100f / (levelMaxTime - minLevelTime);
        float b = -minLevelTime * a;
        var trans = (int)endTime * a + b;
        var bonusTime = 100 - trans;
        var crystalScore = 1000 + ((bonusTime / 100) * 500);
        return (Mathf.Max(500, (crystals * (int)crystalScore) - ((4 - Team.team.Length) * 1000)));
        // return ((Mathf.Max(0, (int)(levelMaxTime - endTime)) * levelScore) + crystals * levelScore) - ((4 - Team.team.Length) * 1000);
    }

    private IEnumerator ScoreUpdater()
    {
        while(true)
        {
            if(displayScore < finalScore)
            {
                displayScore += 100; //Increment the display score by 1
                score.text = displayScore.ToString(); //Write it to the UI
            }
            yield return new WaitForSeconds(0.002f);
        }
    }
}
