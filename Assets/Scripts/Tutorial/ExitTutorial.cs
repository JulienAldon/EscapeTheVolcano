using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTutorial : MonoBehaviour
{
    public GameObject leaveInterface;

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
