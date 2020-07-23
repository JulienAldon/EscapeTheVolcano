using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BatScript : MonoBehaviour
{
    private Shake shake;
    public GameObject blood; 
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    
    private Rigidbody2D rb;
    private Path path;
    private Seeker seeker;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;        
        seeker = GetComponent<Seeker>();
		shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        rb = GetComponent<Rigidbody2D>();        
    }
    
    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, new Vector2(target.position.x - 0.5f, target.position.y - 0.5f), OnPathComplete);
    }
    
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        if (Vector2.Distance(transform.position, target.position) < 10f)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);
            
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint ++;
            }
            if (force.x > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            } else if (force.x < 0) {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
            StartCoroutine(Death());
        }
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.layer == 19)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        shake.camShake();
        Instantiate(blood, transform.position, Quaternion.identity);        
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1;        
        Destroy(gameObject);        
    }
}
