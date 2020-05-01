using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public Text timerText;
    public bool started = false;
    private float startTime;
    private bool finnish = false;
    public static float endTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startTime = Time.time;    
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
            return;
        if (finnish)
            return;
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
    }

    public void Finnish()
    {
        finnish = true;
        endTime = Time.time - startTime;
    }
}
