using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobScript : MonoBehaviour {
	// Start is called before the first frame update
	public class GroundState {
		private GameObject player;
		private float width;
		private float height;
		private float length;
		public bool IsWall;

		//GroundState constructor.  Sets offsets for raycasting.
		public GroundState (GameObject playerRef) {
			player = playerRef;
			width = player.GetComponent<Collider2D> ().bounds.extents.x + 0.4f;
			height = player.GetComponent<Collider2D> ().bounds.extents.y + 0.5f;
			length = 0.5f;
		}

		//Returns whether or not player is touching wall.
		public bool isWall () {
			int LayerIndex = LayerMask.NameToLayer ("Ground");
			int layerMask = (1 << LayerIndex);
			//			layerMask = ~layerMask;

			bool smth = Physics2D.OverlapCircle (player.transform.position, width, layerMask);

			bool left = Physics2D.Raycast (new Vector2 (player.transform.position.x - width, player.transform.position.y), -Vector2.right, length, layerMask);
			//			bool left_down = Physics2D.Raycast(new Vector2(player.transform.position.x - height / 2 - width, player.transform.position.y), -Vector2.right, length, layerMask);
			//			bool left_up = Physics2D.Raycast(new Vector2(player.transform.position.x + height / 2 - width, player.transform.position.y), -Vector2.right, length, layerMask);
			bool right = Physics2D.Raycast (new Vector2 (player.transform.position.x + width, player.transform.position.y), Vector2.right, length, layerMask);
			//			bool right_down = Physics2D.Raycast(new Vector2(player.transform.position.x - height / 2 + width, player.transform.position.y), Vector2.right, length, layerMask);
			//			bool right_up = Physics2D.Raycast(new Vector2(player.transform.position.x + height / 2 + width, player.transform.position.y), Vector2.right, length, layerMask);
			if (left || right || smth)
				return true;
			else
				return false;
		}

		//Returns whether or not player is touching ground.
		public bool isGround () {

			int gnd = 1 << LayerMask.NameToLayer ("Ground");
			int ally = 1 << LayerMask.NameToLayer ("Ennemy");
			int layerMask = gnd | ally;
			//			layerMask = ~layerMask;
			bool bottom1 = Physics2D.Raycast (new Vector2 (player.transform.position.x, player.transform.position.y - height), -Vector2.up, length, layerMask);
			bool bottom2 = Physics2D.Raycast (new Vector2 (player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
			bool bottom3 = Physics2D.Raycast (new Vector2 (player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
			if (bottom1 || bottom2 || bottom3)
				return true;
			else
				return false;
		}

		//Returns whether or not player is touching wall or ground.
		public bool isTouching () {
			if (isGround () || isWall ())
				return true;
			else
				return false;
		}

		//Returns direction of wall.
		public int wallDirection () {
			int LayerIndex = LayerMask.NameToLayer ("Ground");
			int layerMask = (1 << LayerIndex);

			bool left = Physics2D.Raycast (new Vector2 (player.transform.position.x - width, player.transform.position.y), -Vector2.right, length, layerMask);
			bool right = Physics2D.Raycast (new Vector2 (player.transform.position.x + width, player.transform.position.y), Vector2.right, length, layerMask);
			if (left)
				return -1;
			else if (right)
				return 1;
			else
				return 0;
		}
	}

	public Animator anim;
	public float JumpRate = 2f;
	public GameObject Attack;
	public float speed;
	public BoxCollider2D col;
	public GameObject blood;
	public bool canRespawn = true;
	public Material matWhite;
	public GameObject gfx;

	private GroundState groundState;
	private Rigidbody2D rb;
	private float nextJump = 0;
	private bool isJumping;
	private bool jumping = false;
	private Shake shake;
	private Material matDefault;

	private AudioManager audioManager;

	private bool IsFacingRight () {
		return transform.localScale.x > Mathf.Epsilon;
	}

	void Start () {
		audioManager = FindObjectOfType<AudioManager> ();		        		
		rb = GetComponent<Rigidbody2D> ();
		groundState = new GroundState (transform.gameObject);
		shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
		matDefault = gfx.GetComponent<SpriteRenderer> ().material;
	}

	// Update is called once per frame
	void Update () {
		Collider2D[] r = new Collider2D[10];
		ContactFilter2D f = new ContactFilter2D ();
		f.layerMask = LayerMask.GetMask ("Ground");
		int a = col.OverlapCollider (f, r);
		if (r[0] != null) {
			if ((col.IsTouchingLayers (LayerMask.GetMask ("Ground")) == false || IsInside (r[0], col)) && !jumping) {
				transform.localScale = new Vector2 (-(Mathf.Sign (rb.velocity.x)), transform.localScale.y);
			}
		}
		if (IsFacingRight ()) {
			rb.velocity = new Vector2 (speed, rb.velocity.y);
		} else {
			rb.velocity = new Vector2 (-speed, rb.velocity.y);
		}
		if (groundState.isGround () && Time.time > nextJump) {
			nextJump = Time.time + JumpRate;
			StartCoroutine (Jump ());
		}
	}

	bool IsInside (Collider2D enterableCollider, Collider2D enteringCollider) {
		Bounds enterableBounds = enterableCollider.bounds;
		Bounds enteringBounds = enteringCollider.bounds;

		Vector2 center = enteringBounds.center;
		Vector2 extents = enteringBounds.extents;
		Vector2[] enteringVerticles = new Vector2[2];

		enteringVerticles[0] = new Vector2 (center.x + extents.x, center.y + extents.y);
		enteringVerticles[1] = new Vector2 (center.x - extents.x, center.y + extents.y);
		// enteringVerticles[2] = new Vector2 (center.x + extents.x, center.y - extents.y);
		// enteringVerticles[3] = new Vector2 (center.x - extents.x, center.y - extents.y);

		foreach (Vector2 verticle in enteringVerticles) {
			if (!enterableBounds.Contains (verticle)) {
				return false;
			}
		}
		return true;
	}

	IEnumerator Jump () {
		jumping = true;
		anim.SetTrigger ("Jump");
		rb.velocity = new Vector2 (rb.velocity.x, 7);
		yield return new WaitForSeconds (1f);
		var attack = Instantiate (Attack, new Vector3 (transform.position.x - 0.2f, transform.position.y - 0.8f, transform.position.z), Quaternion.identity);
		attack.transform.parent = transform;
		jumping = false;
	}

	void OnParticleCollision (GameObject other) {
		GetComponent<Rigidbody2D> ().AddForce (new Vector2 (700 * (transform.position.x - other.transform.position.x), 700 * (transform.position.y - other.transform.position.y)));
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
        audioManager.Play ("MonsterDeath", UnityEngine.Random.Range (1f, 3f));														        
		gfx.GetComponent<SpriteRenderer> ().material = matWhite;
		Invoke ("ResetMaterial", .2f);

		// SplatCastRay();
		Instantiate (splatParticles, transform.position, Quaternion.identity);
		// Time.timeScale = 0.1f;
		yield return new WaitForSeconds (0.1f);
		gfx.GetComponent<SpriteRenderer> ().material = matDefault;
		// Time.timeScale = 1;
		if (canRespawn) {
			var Child1 = Instantiate (this.gameObject, transform.position, Quaternion.identity);
			Child1.GetComponent<BlobScript> ().canRespawn = false;
			Child1.transform.localScale = new Vector3 (.5f, .5f, 1);
			var Child2 = Instantiate (this.gameObject, transform.position, Quaternion.identity);
			Child2.transform.localScale = new Vector3 (.5f, .5f, 1);
			Child2.GetComponent<BlobScript> ().canRespawn = false;
		}
		Destroy (gameObject);
	}

	// private void SplatCastRay()
	// {
	//     Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(transform.position));
	//     LayerMask mask = ~LayerMask.GetMask("Ennemy");
	//     RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, mask);
	// 	if (hit.collider != null) 
	// 	{
	// 		GameObject splat = Instantiate(splatPrefab, hit.point, Quaternion.identity) as GameObject;
	//         Splat splatScript = splat.GetComponent<Splat>();
	//         // GameObject a = Instantiate(blood, transform.position, Quaternion.identity);
	//         splatParticles.transform.position = hit.point;
	//         splatParticles.Play();
	//         var main = splatParticles.main; 

	//         if (hit.collider.gameObject.tag == "BG") {
	//             splatScript.Initialize(Splat.SplatLocation.Background);
	//         } else {
	//             splatScript.Initialize(Splat.SplatLocation.Foreground);
	//         }
	// 	}
	// }
}