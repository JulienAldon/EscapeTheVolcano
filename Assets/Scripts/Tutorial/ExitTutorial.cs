using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTutorial : MonoBehaviour
{
    public GameObject leaveInterface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;        
        LaunchManager.instance.LoadMenu();
    }

    public void Stay()
    {
        Time.timeScale = 1f;
        leaveInterface.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collision)
	{
		leaveInterface.SetActive(true);
        Time.timeScale = 0f;
	}
}
