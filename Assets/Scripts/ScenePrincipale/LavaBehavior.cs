using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 localPos;
	public float FireRate = 0.5f;
    float nextFire = 0;
    public Transform PointA;
    public Transform PointB;
    public Transform PointC;
    public Transform PointD;
    public Transform Area1;
    public Transform Area2;
    
    private float lavaTimer = 0f;
    private bool startTimer = false;
    private float bonusTime = 0f;

    public float minTime = 0f;
    public float maxTime = 60f;
    public float minBonusTime = 30f;
    public float maxBonusTime = 90f;
    
    private bool stopped = false;

    public float step = 0.01f;
    public float baseExitTime = 30f;
    
    private float exitTime;
    private bool once = true;
    public ExitScript ExitState;
    void Start()
    {
        localPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position = localPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer) {
            lavaTimer += Time.deltaTime;
        }

        if (ExitScript.GetState() == 0) {
            startTimer = true;
        } else if (once) {
            once = false;
            startTimer = false;
            var a = (maxBonusTime - maxTime) / minBonusTime;
            var b = maxTime;
            bonusTime = a * lavaTimer + b;
        }
        if (Time.time > nextFire && ExitState.GetLavaFlow() && !stopped) {
            nextFire = Time.time + FireRate;
            localPos.y += step;
        }
        transform.position = localPos;

        if (ExitScript.GetState() >= 2)
        {
            FireRate = 0.05f;
            step = 0.5f;
        } 
        
        if (transform.position.y >= PointA.position.y)
        {
            step = 0.1f;
            FireRate = 90f / (baseExitTime + bonusTime);
        }
        // if (transform.position.y >= PointB.position.y)
        // {
        //     // Phase 2
        //     FireRate = 0.05f;
        //     step = 0.1f;
        // }
        // if (transform.position.y >= PointC.position.y)
        // {
        //     // Phase 3
        //     FireRate = 0.05f;
        //     step = 0.2f;
        // }
        // if (transform.position.y >= PointD.position.y)
        // {
        //     stopped = true;
        //     // Phase 4
        //     // TODO: GameOver
        // }
        // int LayerIndex = LayerMask.NameToLayer("Player");
        // int layerMask = (1 << LayerIndex);
        // Collider2D hit = Physics2D.OverlapArea(Area1.position, Area2.position, layerMask);
        // if (hit) 
        // {
        // }
    }

    public void AddSpeed(float speed)
    {
        FireRate = speed;
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.collider.GetType() == typeof(CapsuleCollider2D))
        {
            GameObject.Find("Player").GetComponent<CharacterStats>().LavaDie();
        }
        else if (other.collider.GetType() == typeof(CircleCollider2D))
        {
         // do stuff only for the circle collider
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //
    }
}
