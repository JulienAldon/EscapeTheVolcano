using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanManager : MonoBehaviour {
    public float scanRate = 10f;
    public GameObject AstarPath;

    private float nextScan = 0;
    // Start is called before the first frame update
    void Start () {

    }

    void Update () {
        if (Time.time > nextScan) {
            nextScan = Time.time + scanRate;
            AstarPath.GetComponent<AstarPath> ().Scan ();
        }
    }
}