using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BatScript : MonoBehaviour {
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
	private AudioManager audio;

    // Start is called before the first frame update
    void Start () {
		audio = FindObjectOfType<AudioManager> ();		        
        target = GameObject.FindWithTag ("Player").transform;
        seeker = GetComponent<Seeker> ();
        shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
        InvokeRepeating ("UpdatePath", 0f, .5f);
        rb = GetComponent<Rigidbody2D> ();
    }

    void UpdatePath () {
        if (seeker.IsDone ())
            seeker.StartPath (rb.position, new Vector2 (target.position.x - 0.5f, target.position.y - 0.5f), OnPathComplete);
    }

    public bool GetReachedEndOfPath () {
        return (reachedEndOfPath);
    }

    void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void Update () {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        if (Vector2.Distance (transform.position, target.position) < 10f) {
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce (force);

            float distance = Vector2.Distance (rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) {
                currentWaypoint++;
            }
            if (force.x > 0) {
                transform.localRotation = Quaternion.Euler (0, 180, 0);
            } else if (force.x < 0) {
                transform.localRotation = Quaternion.Euler (0, 0, 0);
            }
        }
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
            StartCoroutine (Death ());
        }
    }

    void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.layer == 19) {
            StartCoroutine (Death ());
        }
    }

    public GameObject splatParticles;

    IEnumerator Death () {
        shake.camShake ();
        audio.Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														        
        // SplatCastRay();
        Instantiate (splatParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds (0.1f);
        Destroy (gameObject);
    }

    void OnParticleCollision (GameObject other) {
        GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1000 * (transform.position.x - other.transform.position.x), 1000 * (transform.position.y - other.transform.position.y)));
    }

}