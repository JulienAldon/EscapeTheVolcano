using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTeam : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterDisplay[] teamList;
    public GameObject[] teamObj;
    public GameObject goButton;
    private bool teamCanGo = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++) {
            if (Team.team[i] != null) {
                teamObj[i].SetActive(true);
                teamList[i].changeText(Team.team[i]);
            }
        }
        if (Team.team[3] != null && Team.currentContract != null) {
            goButton.SetActive(true);
        }
    }
}
