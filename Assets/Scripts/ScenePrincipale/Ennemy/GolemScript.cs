using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemScript : MonoBehaviour {
    public BoxCollider2D playerCol;
    public BoxCollider2D moveCol;
    public float speed;
    public GameObject slashRight;
    public GameObject slashLeft;
    public Animator anim;
    private Shake shake;
    public GameObject blood;
    public float fireRate;
    private bool attacking;
    Rigidbody2D rb;
    private float nextFire = 0;
    public int Health = 5;
    public Material matWhite;
    private Material matDefault;
    public GameObject gfx;
	private AudioManager audioManager;

    // Start is called before the first frame update
    void Start () {
		audioManager = FindObjectOfType<AudioManager> ();		                
        rb = GetComponent<Rigidbody2D> ();
        shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
        matDefault = gfx.GetComponent<SpriteRenderer> ().material;

    }
    // Update is called once per frame
    void Update () {
        Collider2D[] r = new Collider2D[10];
        ContactFilter2D f = new ContactFilter2D ();
        f.layerMask = LayerMask.GetMask ("Ground");
        int a = moveCol.OverlapCollider (f, r);
        if ((moveCol.IsTouchingLayers (LayerMask.GetMask ("Ground")) == false || a > 2) && !attacking) {
            transform.localScale = new Vector2 (-(Mathf.Sign (rb.velocity.x)), transform.localScale.y);
        }
        if (!attacking) {
            if (IsFacingRight ()) {
                rb.velocity = new Vector2 (speed, rb.velocity.y);
            } else {
                rb.velocity = new Vector2 (-speed, rb.velocity.y);
            }
        }
        if (playerCol.IsTouchingLayers (LayerMask.GetMask ("Player")) && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            StartCoroutine (Attack ());
        }
    }

    private bool IsFacingRight () {
        return transform.localScale.x > Mathf.Epsilon;
    }

    IEnumerator Attack () {
        attacking = true;
        anim.SetTrigger ("Attack");
        yield return new WaitForSeconds (0.5f);
        if (IsFacingRight ())
            Instantiate (slashRight, new Vector3 (transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.identity);
        else
            Instantiate (slashLeft, new Vector3 (transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);

        attacking = false;
    }

    void TakeDamage () {
        gfx.GetComponent<SpriteRenderer> ().material = matWhite;
        Health -= 1;
        if (Health <= 0)
            StartCoroutine (Death ());
        else
            Invoke ("ResetMaterial", .2f);
    }

    void ResetMaterial () {
        gfx.GetComponent<SpriteRenderer> ().material = matDefault;
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
            TakeDamage ();
        }
    }

    void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.layer == 19) {
            TakeDamage ();
        }
    }

    public GameObject splatParticles;

    IEnumerator Death () {
        shake.camShake ();
        // SplatCastRay();
        audioManager.Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														                
        Instantiate (splatParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds (0.1f);
        Destroy (gameObject);
    }

    void OnParticleCollision (GameObject other) {
        GetComponent<Rigidbody2D> ().AddForce (new Vector2 (100 * (transform.position.x - other.transform.position.x), 100 * (transform.position.y - other.transform.position.y)));
    }

}