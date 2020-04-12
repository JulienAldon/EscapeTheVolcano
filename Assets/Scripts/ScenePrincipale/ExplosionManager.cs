using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 0.8f);
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("AnimationEnded"))
        {
            Destroy(gameObject);
        }
    }
}
