using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization; 
using System;

public class ProgressBar : MonoBehaviour
{

    // void Start()
    // {
    //     localScale = transform.localScale;
    // }
    
    public void SetProgress(float progress)
    {
        transform.localScale = new Vector3(progress, transform.localScale.y, transform.localScale.z);
    }
}
