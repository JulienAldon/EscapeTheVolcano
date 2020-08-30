using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeAnim : MonoBehaviour
{
	private Shake shake;    
    public GameObject escape;
    bool once = true;

    void Start()
    {
		shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
    }
    // Update is called once per frame
    void Update()
    {
        if (Level.state == 0 && once) {
            StartCoroutine( Escape()); 
            once = false;     
        }
    }

    IEnumerator Escape()
    {
        escape.SetActive(true);
        shake.camShakeBig();
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 1f;        
        escape.SetActive(false);
    }
    
}
