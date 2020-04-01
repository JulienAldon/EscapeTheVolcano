using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    private GameObject Player;
    public GameObject lavaSpawner;
    // Start is called before the first frame update
    public static int state = 0;

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
        print(state);
        if (state == 0) {
            Player.transform.position = GameObject.Find("SpawnTreasure").transform.position;
        } else if (state == 1)
        {
            Player.transform.position = GameObject.Find("WayOut(Clone)").transform.position;
            Level.canWin = true;
            Instantiate(lavaSpawner);
        }
        state += 1;
    }
}
