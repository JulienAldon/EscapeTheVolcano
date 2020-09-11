using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscToCloseWindow : MonoBehaviour
{
    public GameObject windowToClose;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (windowToClose.transform.localScale.x > 0)
                windowToClose.GetComponent<Animator>().SetTrigger("close");
            // windowToClose.SetActive(false);
        }
    }
}
