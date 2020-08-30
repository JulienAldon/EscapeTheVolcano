using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreList : MonoBehaviour
{
    public GameObject PlayerScoreEntryPrefab;
    ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        if (scoreManager == null) {
            Debug.LogError("No scoreManager component in the scene.");
        }
        
    }

    public void UpdateList() {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        if (scoreManager == null) {
            Debug.LogError("No scoreManager component in the scene.");
        }
        string[] names = scoreManager.GetSortPlayerNames("Time");
        while (this.transform.childCount > 0) {
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }
        foreach (var name in names)
        {
            GameObject go = (GameObject)Instantiate(PlayerScoreEntryPrefab);
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(1f, 1f, 1f);
            go.transform.Find("Username").GetComponent<TextMeshProUGUI>().text = name;
            float t = scoreManager.GetScore(name, "Time");
            string minutes = ((int) t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            go.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = minutes + ":" + seconds;
            go.transform.Find("Crystals").GetComponent<TextMeshProUGUI>().text = scoreManager.GetScore(name, "Crystals").ToString();
            go.transform.Find("LostRobots").GetComponent<TextMeshProUGUI>().text = scoreManager.GetScore(name, "LostRobots").ToString();
            go.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = scoreManager.GetScore(name, "Score").ToString();
        }
    }
}
