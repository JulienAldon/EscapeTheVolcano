using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string[] sentences; 
    private int current;
    // Start is called before the first frame update
    void Start()
    {
        text.text = sentences[0];
        
    }

    public void nextSentence()
    {
        current+=1;
        text.text = sentences[current];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            nextSentence();
        }
    }
}
