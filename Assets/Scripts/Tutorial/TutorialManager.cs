using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    public GameObject player;

    void Awake()
    {
        Team.team = new Character[6];
        Team.team[0] = new Character("tutoRunner", "Runner" , Random.Range(3,6), 20, new Color32(102,255,80, 255), "Normal", Random.Range(1,3), "Missile");
        Team.team[1] = new Character("tutoClimber", "Climber" , Random.Range(3,6), 20, new Color32(95,25,255, 255), "Normal", Random.Range(1,3), "Gatling");
        Team.team[2] = new Character("tutoHacker", "Hacker" , Random.Range(3,6), 20, new Color32(255,150,0, 255), "Normal", Random.Range(1,3), "EnergyGun");
        Team.team[3] = new Character("tutoTank", "Tank" , Random.Range(3,6), 20, new Color32(75,55,215, 255), "Normal", Random.Range(1,3), "Missile");
        Team.team[4] = new Character("tutoGrenadier", "Grenadier" , Random.Range(3,6), 20, new Color32(60,180,255, 255), "Normal", Random.Range(1,3), "Gatling");
        Team.team[5] = new Character("tutoTracker", "Tracker" , Random.Range(3,6), 20, new Color32(60,180,255, 255), "Normal", Random.Range(1,3), "EnergyGun");
    }
    void Start() 
    {
        //player.GetComponent<TestController>().jump = 0f;
    }

    void Update()
    {
        // for (int i = 0; i < popUps.Length; i++)
        // {
        //     popUps[i].SetActive(i == popUpIndex);
        // }
        if (popUpIndex == 0) { // move
            if (Input.GetKey(KeyBindScript.keys["Left"]) || Input.GetKey(KeyBindScript.keys["Right"])) {
                popUpIndex ++;
            }
        } else if (popUpIndex == 1) { // jump
            player.GetComponent<TestController>().jump = 11f;    
            if (Input.GetKey(KeyBindScript.keys["Jump"])) {
                popUpIndex ++;
            }
        } else if (popUpIndex == 2) { // fire
            if (Input.GetKey(KeyBindScript.keys["Fire"])) {
                popUpIndex++;
            }
        } else if (popUpIndex == 3) { //switch 
            if (Input.GetKey(KeyBindScript.keys["Switch"])) {
                popUpIndex ++;
            }
        }
    }
}
