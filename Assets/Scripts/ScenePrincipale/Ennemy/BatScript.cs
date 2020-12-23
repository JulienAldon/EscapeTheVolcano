using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using System;
public class BatScript : MonoBehaviour {
    private Shake shake;
    public GameObject blood;
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    private Vector2 direction;
    private Rigidbody2D rb;
	private AudioManager audioManager;

    // Start is called before the first frame update
    void Start () {
        Team.monsterNumber += 1;
		audioManager = FindObjectOfType<AudioManager> ();		        
        target = GameObject.FindWithTag ("Player").transform;
        shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
        rb = GetComponent<Rigidbody2D> ();
        direction = new Vector2(1, 1);

    }

    // Update is called once per frame
    void Update () {
        // if (path == null)
        //     return;
        // if (currentWaypoint >= path.vectorPath.Count) {
        //     reachedEndOfPath = true;
        //     return;
        // } else {
        //     reachedEndOfPath = false;
        // }

        // if (Vector2.Distance (transform.position, target.position) < 10f) {
        //     Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        //     Vector2 force = direction * speed * Time.deltaTime;

        //     rb.AddForce (force);

        //     float distance = Vector2.Distance (rb.position, path.vectorPath[currentWaypoint]);

        //     if (distance < nextWaypointDistance) {
        //         currentWaypoint++;
        //     }
        //     if (force.x > 0) {
        //         transform.localRotation = Quaternion.Euler (0, 180, 0);
        //     } else if (force.x < 0) {
        //         transform.localRotation = Quaternion.Euler (0, 0, 0);
        //     }
        // }
        rb.AddForce((-direction) * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.layer == 19) {
            StartCoroutine (Death ());
        } else if (collision.gameObject.layer == 12) {
            Team.batKilled += 1;
			StartCoroutine (Death ());
        } else {
            var x = collision.transform.position.x - transform.position.x;
            var y = collision.transform.position.y - transform.position.y;
            var x2 = Math.Pow(x, 2);
            var y2 =  Math.Pow(y, 2);
            direction = new Vector2((float)(1 / Math.Sqrt(1 +  (x2 / y2) ) ), (float)( x / (y * Math.Sqrt(1 + (x2 / y2) ) ) ) );
        }
    }

    void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.layer == 19) {
            StartCoroutine (Death ());
        } else {
            var x = collision.transform.position.x - transform.position.x;
            var y = collision.transform.position.y - transform.position.y;
            var x2 = Math.Pow(x, 2);
            var y2 =  Math.Pow(y, 2);
            direction = new Vector2((float)(1 / Math.Sqrt(1 +  x2/ y2)), (float)(- x / (y * Math.Sqrt(1 + x2 / y2)) ) );
        }
    }

    public void Damage(Vector3 dir)
    {
        StartCoroutine (Death ());
    }

    public GameObject splatParticles;

    IEnumerator Death () {
        shake.camShake ();
        audioManager.Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														        
        // SplatCastRay();
        Instantiate (splatParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds (0.1f);
        Destroy (gameObject);
    }

    void OnParticleCollision (GameObject other) {
        GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1000 * (transform.position.x - other.transform.position.x), 1000 * (transform.position.y - other.transform.position.y)));
    }

}