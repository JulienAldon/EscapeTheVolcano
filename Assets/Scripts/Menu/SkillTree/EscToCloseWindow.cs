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
            windowToClose.SetActive(false);
        }
    }
}
