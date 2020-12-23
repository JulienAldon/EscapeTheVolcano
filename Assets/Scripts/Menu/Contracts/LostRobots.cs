using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LostRobots : MonoBehaviour
{
    public TextMeshProUGUI lostRobots;
    public GameObject[] deadsChar;
    void Start()
    {
        lostRobots.text = "You have lost " + Team.deadCharacters.Count.ToString() + " robots !";
        StartCoroutine(ShowLostRobots());
    }

    IEnumerator ShowLostRobots() {
        int i = 0;
        foreach (var elem in Team.deadCharacters) {
            deadsChar[i].SetActive(true);
            yield return new WaitForSeconds(.5f);
            i += 1;
        }
    }

    void Update()
    {
        
    }
}
