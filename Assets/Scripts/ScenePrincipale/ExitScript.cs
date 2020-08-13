using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private GameObject Player;
    // Start is called before the first frame update
    // public static int state = -1;

    void Start()
    {
        Player = GameObject.Find("Player");   
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Level.state < 2) {
            Level.state += 1;
            if (Level.state == 0) { 
                // phase treasure
                Player.transform.position = GameObject.Find("SpawnTreasure").transform.position;
            } else if (Level.state == 1)
            {
                // phase go back to spawn
                Player.transform.position = GameObject.Find("WayOut(Clone)").transform.position;
                Level.canWin = true;
            }
        }
    }

    static public int GetState()
    {
        return Level.state;
    }

    public bool GetLavaFlow()
    {
        if (Level.state >= 0)
            return true;
        return false;
    }
}
