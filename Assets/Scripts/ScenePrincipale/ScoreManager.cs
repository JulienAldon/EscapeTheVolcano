using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    //ScoreCalculation
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

    //Scoreboard
    private Dictionary<string, Dictionary<string, int> > playerScores;

    void InitScoreboard() {
        if (playerScores != null)
            return;
        playerScores = new Dictionary<string, Dictionary<string, int>>();
    }

    public int GetScore(string username, string scoreType) {
        InitScoreboard();

        if (playerScores.ContainsKey(username) == false) {
            return 0;
        }

        if (playerScores[username].ContainsKey(scoreType) == false) {
            return 0;
        }

        return playerScores[username][scoreType];
    }

    public void SetScore(string username, string scoreType, int value) {
        InitScoreboard();

        if (playerScores.ContainsKey(username) == false) {
            playerScores[username] = new Dictionary<string, int>();
        }

        playerScores[username][scoreType] = value;
    }

    public void ChangeScore(string username, string scoreType, int amount) {
        InitScoreboard();
        int currScore = GetScore(username, scoreType);
        SetScore(username, scoreType, currScore + amount);
    }

    public string[] GetPlayerNames() {
        InitScoreboard();        
        return playerScores.Keys.ToArray();
    }

    public string[] GetSortPlayerNames(string sortingScoreType) {
        InitScoreboard();  

        return playerScores.Keys.OrderBy( n => GetScore(n, sortingScoreType) ).ToArray();
    }

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
        
        Load();
        SetScore(Team.team[0].name, "Time", (int)Timer.endTime);
        SetScore(Team.team[0].name, "Crystals", CharacterStats.nbCrystals);
        SetScore(Team.team[0].name, "LostRobots", (4 - Team.team.Length));
        SetScore(Team.team[0].name, "Score", finalScore);
        Save();
    }
    // PlayerScoreList scoreList;

    // public void ListScore() {
    //     scoreList = GameObject.FindObjectOfType<PlayerScoreList>();
    //     if (scoreList == null) {
    //         Debug.LogError("No ScoreList component in the scene.");
    //     }
    //     scoreList.UpdateList();
    // }

    void Save()
    {
        string filename = "/score.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath+filename);
        // SaveScore data = new SaveScore();
        // data.name = _name;
        // data.crystals = _crystals;
        // data.time = _time;
        // data.score = _score;
        // data.lostRobots = _lostRobots;
        bf.Serialize(file, playerScores); //Use can easily use e.g. a List if you want to store more date
        file.Close();
    }

    bool Load()
    {
        string filename = "/score.dat";
        if (File.Exists(Application.persistentDataPath + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            playerScores=(Dictionary<string, Dictionary<string, int> >) bf.Deserialize(file);

            file.Close();
            return true;
        }
        return false;
    }

    int CalculateScore()
    {
        endTime = Timer.endTime;
        crystals = CharacterStats.nbCrystals;
        float a = 100f / (levelMaxTime - minLevelTime);
        float b = -minLevelTime * a;
        var trans = (int)endTime * a + b;
        var bonusTime = 100 - trans;
        var crystalScore = 150 + ((bonusTime / 100) * 500);
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
            yield return new WaitForSeconds(0.001f);
        }
    }
}
