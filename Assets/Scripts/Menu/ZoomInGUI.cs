using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomInGUI : MonoBehaviour
{
    public GameObject toZoom;
    [SerializeField]
    float zoomModifierSpeed = 0.1f;

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            if (toZoom.transform.localScale.x < 3)
                toZoom.transform.localScale += new Vector3(zoomModifierSpeed, zoomModifierSpeed, zoomModifierSpeed);
        }
    
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (toZoom.transform.localScale.x > 1.4)
                toZoom.transform.localScale -= new Vector3(zoomModifierSpeed, zoomModifierSpeed, zoomModifierSpeed);
        }
    }
}