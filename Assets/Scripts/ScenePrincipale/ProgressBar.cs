using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    Vector3 localScale;

    void Start()
    {
        localScale = transform.localScale;
    }
    
    public void SetProgress(float progress)
    {
        localScale.x = progress;       
        transform.localScale = localScale;
    }

    void Update(){
//        transform.rotation = Quaternion.identity;
    }
}
