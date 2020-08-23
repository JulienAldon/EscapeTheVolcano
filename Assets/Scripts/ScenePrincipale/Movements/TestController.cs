using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TestController : MonoBehaviour {
	public class GroundState {
		private GameObject player;
		private float width;
		private float height;
		private float length;
		public bool IsWall;

		//GroundState constructor.  Sets offsets for raycasting.
		public GroundState (GameObject playerRef) {
			player = playerRef;
			width = player.GetComponent<Collider2D> ().bounds.extents.x + 0.2f;
			height = player.GetComponent<Collider2D> ().bounds.extents.y + 0.2f;
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
			int LayerIndex = LayerMask.NameToLayer ("Ground");
			int layerMask = (1 << LayerIndex);
			//			layerMask = ~layerMask;
			bool bottom1 = Physics2D.Raycast (new Vector2 (player.transform.position.x, player.transform.position.y - height), -Vector2.up, length, layerMask);
			bool bottom2 = Physics2D.Raycast(new Vector2(player.transform.position.x + (width), player.transform.position.y - height), -Vector2.up, length, layerMask);
			bool bottom3 = Physics2D.Raycast(new Vector2(player.transform.position.x - (width), player.transform.position.y - height), -Vector2.up, length, layerMask);
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

	//Feel free to tweak these values in the inspector to perfection.  I prefer them private.
	[Header ("Movement")]
	public float speed = 14f;
	public float accel = 6f;
	public float airAccel = 3f;
	public float jump = 14f; //I could use the "speed" variable, but this is only coincidental in my case.  Replace line 89 if you think otherwise.

	private float jumpTimeCounter;
	public float jumpTime;
	private bool isJumping;
	private bool jumped;

	[Header ("Shooting")]
	public GameObject BulletLeft;
	public GameObject BulletRight;
	public GameObject Shell;
	float nextFire = 0;
	private bool isFiring = false;
	public int nbFire = 3;

	[Header ("Effects")]
	public Animator animator;
	public ParticleSystem dust;
	public ParticleSystem dustWeapon;
	private GroundState groundState;
	private Shake shake;

	[Header ("Runner")]
	public float dashSpeed;
	public GameObject dashParticle;
	public float startDashTime;

	private float nextDash;
	private float nextRefreshRunner = 0;
	private Vector2 lastInput;
	private float dashTime;
	private int direction = 0;
	private bool canDash = true;

	[Header ("Climber")]
	private float nextClimb;
	public DistanceJoint2D joint;
	public LineRenderer line;
	public float step = 0.1f;
	private int facing;
	private float nextRefreshClimber = 0;

	[Header ("Tracker")]
	// Tracker
	public GameObject FlagSprite;
	public float FlagRate;
	private float NextFlag;

	[Header ("Grenadier")]
	public GameObject Bomb;
	public float BombRate;
	private float nextBomb = 0;

	[Header ("Tank")]
	public GameObject Shield;
	public float ShieldTime = 15f;
	private float usedTimeShield;
	private bool canShield = true;

	private AudioManager audio;
	void Start () {
		//Create an object to check if player is grounded or touching wall
		groundState = new GroundState (transform.gameObject);
		dashTime = startDashTime;
		//grappling
		joint = GetComponent<DistanceJoint2D> ();
		joint.enabled = false;
		line.enabled = false;
		facing = 1;
		audio = FindObjectOfType<AudioManager> ();
		shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
		//dash
	}

	private Vector2 input;
	Vector2 bulletPos;
	private float rawY;
	private float speedTimer;

	void Update () {
		speedTimer -= Time.deltaTime;
		if (speedTimer <= 0) {
			speed = 5 + GetComponent<CharacterStats> ().Speed.GetValue ();
			if (speed <= 5) {
				speed = 10f;
			}
		}
		if (Input.GetKey (KeyBindScript.keys["Up"])) {
			rawY = 1f;
		} else if (Input.GetKey (KeyBindScript.keys["Down"])) {
			rawY = -1f;
		} else {
			rawY = 0f;
		}
		CharacterStats Character = GetComponent<CharacterStats> ();
		//Handle input
		if (groundState.isTouching () && Input.GetKeyDown (KeyBindScript.keys["Jump"])) {
			isJumping = true;
			jumpTimeCounter = jumpTime;
			if (groundState.isTouching ()) {
				createDust ();
			}
			input.y = 1;
			audio.Play ("PlayerJump");
			animator.SetTrigger ("Jumping");
			animator.SetBool ("Jump", true);
		}

		if (Input.GetKey (KeyBindScript.keys["Jump"]) && isJumping == true) {
			if (jumpTimeCounter > 0) {
				// GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1 * jump);

				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
			}
		}
		if (Input.GetKeyUp (KeyBindScript.keys["Jump"])) {
			isJumping = false;
		}

		if (Input.GetKey (KeyBindScript.keys["Left"])) {
			input.x = -1;
			lastInput.x = -1;
			if (transform.localRotation.y == 0) {
				if (groundState.isGround ())
					createDust ();
				if (!isFiring) {
					transform.localRotation = Quaternion.Euler (0, 180, 0);
					facing = -1;
				}
			}
			if (!isJumping)
				animator.SetBool ("Running", true);
		} else if (Input.GetKey (KeyBindScript.keys["Right"])) {
			input.x = 1;
			lastInput.x = 1;
			if (transform.localRotation.y == -1) {
				if (groundState.isGround ())
					createDust ();
				if (!isFiring) {
					transform.localRotation = Quaternion.Euler (0, 0, 0);
					facing = 1;
				}
			}
			if (!isJumping)
				animator.SetBool ("Running", true);
		} else {
			input.x = 0;
			animator.SetBool ("Running", false);
		}
		if (isJumping) {
			animator.SetBool ("Running", false);
		}
		if (isJumping && groundState.isGround ()) {
			isJumping = false;
		}
		if (GetComponent<Rigidbody2D> ().velocity.y < 0 && groundState.isTouching () == false) {
			jumped = true;
		}
		// Fire
		if (Character.currentAffliction != "Pacifist") {
			if (Input.GetKey (KeyBindScript.keys["Fire"]) && Time.time > nextFire) {
				nextFire = Time.time + Team.team[GetComponent<CharacterStats> ().currentChar].fireRate;
				fire ();

				//gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0) * -facing * 200f);
				// shake.camShake();
			}
			if (Input.GetKeyDown (KeyBindScript.keys["Fire"])) {
				isFiring = true;
			}
			if (Input.GetKeyUp (KeyBindScript.keys["Fire"])) {
				isFiring = false;
			}
		}
		// Mechanic
		if (Character.ClassType.GetValue () == 1) // Runner
		{
			if (Input.GetKeyDown (KeyBindScript.keys["Action"]) && canDash && Time.time > nextDash) {
				Dash (input.x, rawY);
				Team.team[Character.currentChar].runner_state = 0;
				nextDash = Time.time + Character.runner_CDR;
			} else if (Time.time > nextRefreshRunner) {
				nextRefreshRunner = Time.time + 0.1f;
				if (Team.team[Character.currentChar].runner_state + Team.team[Character.currentChar].runner_step > 20) {
					Team.team[Character.currentChar].runner_state = 20;
				} else {
					Team.team[Character.currentChar].runner_state += Team.team[Character.currentChar].runner_step;
				}
			}
			if (Input.GetKey (KeyBindScript.keys["Action"])) { }
			if (Input.GetKeyUp (KeyBindScript.keys["Action"])) { }
		} else if (Character.ClassType.GetValue () == 2) //Climber
		{
			if (Input.GetKeyDown (KeyBindScript.keys["Action"]) && Time.time > nextClimb) {
				//create the link (raycast ...)
				int LayerIndex = LayerMask.NameToLayer ("Ground");
				int layerMask = (1 << LayerIndex);
				RaycastHit2D hit;

				hit = Physics2D.Raycast (transform.position, new Vector2 (facing < 0 ? -0.5f : 0.5f, 1), Mathf.Infinity, layerMask);
				joint.enabled = true;
				joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D> ();
				joint.connectedAnchor = hit.point - new Vector2 (hit.collider.transform.position.x, hit.collider.transform.position.y);
				joint.distance = Vector2.Distance (transform.position, hit.point);
				line.enabled = true;
				line.SetPosition (0, transform.position);
				line.SetPosition (1, hit.point);
				nextClimb = Time.time + Character.climber_CDR;
				if (Team.team[Character.currentChar].climber_state == 20)
					Team.team[Character.currentChar].climber_state = 0;
			} else if (Time.time > nextRefreshClimber) {
				nextRefreshClimber = Time.time + 0.1f;
				if (Team.team[Character.currentChar].climber_state + Team.team[Character.currentChar].climber_step > 20) {
					Team.team[Character.currentChar].climber_state = 20;
				} else {
					Team.team[Character.currentChar].climber_state += Team.team[Character.currentChar].climber_step;
				}
			}
			if (Input.GetKey (KeyBindScript.keys["Action"])) {
				if (joint.distance > 0.2f) {
					joint.distance -= step;
				} else {
					joint.enabled = false;
					line.enabled = false;
				}
				line.SetPosition (0, transform.position);
			} else if (Time.time > nextRefreshClimber) {
				nextRefreshClimber = Time.time + 0.1f;
				if (Team.team[Character.currentChar].climber_state + Team.team[Character.currentChar].climber_step > 20) {
					Team.team[Character.currentChar].climber_state = 20;
				} else {
					Team.team[Character.currentChar].climber_state += Team.team[Character.currentChar].climber_step;
				}
			}
			if (Input.GetKeyUp (KeyBindScript.keys["Action"])) {
				// delete the link

				joint.enabled = false;
				line.enabled = false;
			}
		} else if (Character.ClassType.GetValue () == 3) // Hacker
		{
			GameObject[] turrets = GameObject.FindGameObjectsWithTag ("Turret");
			GameObject turret = GetClosestTurret (turrets);
			if (Input.GetKeyDown (KeyBindScript.keys["Action"])) {
				// Show range
				foreach (GameObject n in turrets) {
					if (!n.transform.GetChild (1).gameObject.GetComponent<turretScript> ().getDeactivation ())
						n.transform.GetChild (3).GetComponent<SpriteRenderer> ().enabled = true;
					else
						n.transform.GetChild (3).GetComponent<SpriteRenderer> ().enabled = false;
				}
			}
			if (Input.GetKey (KeyBindScript.keys["Action"])) {
				if (turret) {
					turret.transform.GetChild (1).gameObject.GetComponent<turretScript> ().hack (transform.position);
				}
			}
			if (Input.GetKeyUp (KeyBindScript.keys["Action"])) {
				// Hide range
				foreach (GameObject n in turrets) {
					n.transform.GetChild (3).GetComponent<SpriteRenderer> ().enabled = false;
					n.transform.GetChild (1).gameObject.GetComponent<turretScript> ().resetHack ();
				}
			}
		} else if (Character.ClassType.GetValue () == 4) // Tracker
		{
			if (Input.GetKeyDown (KeyBindScript.keys["Action"]) && Time.time > NextFlag && groundState.isGround () && Team.team[GetComponent<CharacterStats> ().currentChar].nbFlags > 0) {
				NextFlag = Time.time + FlagRate;
				Team.team[GetComponent<CharacterStats> ().currentChar].nbFlags -= 1;
				GetComponent<CharacterStats> ().UpdatePower ();
				// Spawn a landmark
				Instantiate (FlagSprite, new Vector3 (transform.position.x, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
			} else if (Team.team[GetComponent<CharacterStats> ().currentChar].nbFlags <= 0) {
				// display error msg for 2 sec
			}
			if (Input.GetKey (KeyBindScript.keys["Action"])) { }
			if (Input.GetKeyUp (KeyBindScript.keys["Action"])) { }
		} else if (Character.ClassType.GetValue () == 5) // Tank
		{
			if (Shield.activeSelf == true) {
				Team.team[GetComponent<CharacterStats> ().currentChar].used_tank_shield += Time.deltaTime;
				Team.team[GetComponent<CharacterStats> ().currentChar].tank_state = (int) (Team.team[GetComponent<CharacterStats> ().currentChar].tank_shield - Team.team[GetComponent<CharacterStats> ().currentChar].used_tank_shield);
				GetComponent<CharacterStats> ().UpdatePower ();
			}
			if (Team.team[GetComponent<CharacterStats> ().currentChar].used_tank_shield >= Character.tank_shield) {
				canShield = false;
				Shield.SetActive (false);
			}
			if (Input.GetKeyDown (KeyBindScript.keys["Action"])) {
				if (Shield.activeSelf == false && canShield) {
					Shield.SetActive (true);
				} else {
					Shield.SetActive (false);
				}
			}
			if (Input.GetKey (KeyBindScript.keys["Action"])) { }
			if (Input.GetKeyUp (KeyBindScript.keys["Action"])) { }
		} else if (Character.ClassType.GetValue () == 6) // Grenadier
		{
			if (Input.GetKeyDown (KeyBindScript.keys["Action"])) {
				shootBomb ();
			}
			if (Input.GetKey (KeyBindScript.keys["Action"])) {
				shootBomb ();
			}
			if (Input.GetKeyUp (KeyBindScript.keys["Action"])) { }
		}

	}

	float AngleBetweenPoints (Vector2 a, Vector2 b) {
		return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

	private void Dash (float x, float y) {
		Vector2 dashDir = new Vector2 (lastInput.x, rawY);
		Vector3 newDashDir = new Vector3 (dashDir.x, dashDir.y, 0);
		float angle = AngleBetweenPoints (transform.position + newDashDir, transform.position);
		var a = Instantiate (dashParticle, transform.position, Quaternion.Euler (0, 0, angle));
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (dashDir.normalized.x * dashSpeed * 2, dashDir.normalized.y * dashSpeed);
		shake.camShake ();
		canDash = false;

		StartCoroutine (DashWait ());
	}

	IEnumerator DashWait () {
		FindObjectOfType<GhostTrail> ().ShowGhost ();

		yield return new WaitForSeconds (.3f);

		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
	}

	GameObject GetClosestTurret (GameObject[] turrets) {
		GameObject bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach (GameObject potentialTarget in turrets) {
			Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if (dSqrToTarget < closestDistanceSqr && !potentialTarget.transform.GetChild (1).gameObject.GetComponent<turretScript> ().getDeactivation ()) {
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}
		return bestTarget;
	}

	void shootBomb () {
		if (Time.time > nextBomb && Team.team[GetComponent<CharacterStats> ().currentChar].grenadier_bombs > 0) {
			nextBomb = Time.time + BombRate;
			//Throw a bomb
			Team.team[GetComponent<CharacterStats> ().currentChar].grenadier_bombs -= 1;
			GetComponent<CharacterStats> ().UpdatePower ();
			GameObject clone;
			clone = Instantiate (Bomb, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
			clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (300 * facing, 400));
		}
	}

	public GameObject BulletLeftGatling;
	public GameObject BulletRightGatling;

	private int spreadRange = 20;
	public int maxSpread = 20;
	public int minSpread = 10;
	void fire () {
		dustWeapon.Play ();
		bulletPos = transform.position;
		GameObject clone;
		CharacterStats Character = GetComponent<CharacterStats> ();
		if (Character.currentAffliction == "Partygoer") {
			if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
				bulletPos += new Vector2 (+0.6f, -0.05f);
				clone = Instantiate (BulletRight, bulletPos, Quaternion.identity);
				// clone.GetComponent<SplatParticles>().splatParticles.Play();
			} else {
				bulletPos += new Vector2 (-0.6f, -0.05f);
				clone = Instantiate (BulletLeft, bulletPos, Quaternion.identity);
				// clone.GetComponent<SplatParticles>().splatParticles.Play();
			}
			GameObject b = Instantiate (Shell, transform.position, Quaternion.identity);
			b.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (300 * -facing, 200));
			return;
		}
		// 3 type of weapon
		// Missile launcher
		// gatling
		if (Team.team[Character.currentChar].weaponType == "Missile") { // missile
			if (Team.team[Character.currentChar].currentFire > 2)
				Team.team[Character.currentChar].currentFire = 0;
			if (Team.team[Character.currentChar].currentFire == 0) {
				if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
					bulletPos += new Vector2 (+0.6f, -0.05f);
					var clone1 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone2 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone3 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					clone1.transform.Rotate (0, 0, 10f);
					clone2.transform.Rotate (0, 0, 0f);
					clone3.transform.Rotate (0, 0, -10f);
					clone1.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 10f) * new Vector2 (300, 0));
					clone2.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 0f) * new Vector2 (300, 0));
					clone3.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -10f) * new Vector2 (300, 0));
				} else {
					bulletPos += new Vector2 (-0.6f, -0.05f);
					var clone1 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone2 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone3 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					clone1.transform.Rotate (0, 0, 10f);
					clone2.transform.Rotate (0, 0, 0f);
					clone3.transform.Rotate (0, 0, -10f);
					clone1.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 10f) * new Vector2 (-300, 0));
					clone2.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 0f) * new Vector2 (-300, 0));
					clone3.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -10f) * new Vector2 (-300, 0));
				}
			} else if (Team.team[Character.currentChar].currentFire == 1) {
				if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
					bulletPos += new Vector2 (+0.6f, -0.05f);
					var clone1 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone2 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone3 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone4 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					clone1.transform.Rotate (0, 0, 10f);
					clone2.transform.Rotate (0, 0, 0f);
					clone3.transform.Rotate (0, 0, -10f);
					clone4.transform.Rotate (0, 0, -10f);
					clone1.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 20f) * new Vector2 (300, 0));
					clone2.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 10f) * new Vector2 (300, 0));
					clone3.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -10f) * new Vector2 (300, 0));
					clone4.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -20f) * new Vector2 (300, 0));
				} else {
					bulletPos += new Vector2 (-0.6f, -0.05f);
					var clone1 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone2 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone3 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone4 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					clone1.transform.Rotate (0, 0, 10f);
					clone2.transform.Rotate (0, 0, 0f);
					clone3.transform.Rotate (0, 0, -10f);
					clone4.transform.Rotate (0, 0, -10f);
					clone1.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 20f) * new Vector2 (-300, 0));
					clone2.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 10f) * new Vector2 (-300, 0));
					clone3.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -10f) * new Vector2 (-300, 0));
					clone4.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -20f) * new Vector2 (-300, 0));
				}
			} else if (Team.team[Character.currentChar].currentFire == 2) {
				if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
					bulletPos += new Vector2 (+0.6f, -0.05f);
					var clone1 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone2 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone3 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone4 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					var clone5 = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
					clone1.transform.Rotate (0, 0, 10f);
					clone2.transform.Rotate (0, 0, 0f);
					clone3.transform.Rotate (0, 0, -10f);
					clone4.transform.Rotate (0, 0, -10f);
					clone5.transform.Rotate (0, 0, -10f);
					clone1.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 20f) * new Vector2 (300, 0));
					clone2.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 10f) * new Vector2 (300, 0));
					clone3.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 0) * new Vector2 (300, 0));
					clone4.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -10f) * new Vector2 (300, 0));
					clone5.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -20f) * new Vector2 (300, 0));
				} else {
					bulletPos += new Vector2 (-0.6f, -0.05f);
					var clone1 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone2 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone3 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone4 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					var clone5 = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
					clone1.transform.Rotate (0, 0, 10f);
					clone2.transform.Rotate (0, 0, 0f);
					clone3.transform.Rotate (0, 0, -10f);
					clone4.transform.Rotate (0, 0, -10f);
					clone5.transform.Rotate (0, 0, -10f);
					clone1.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 20f) * new Vector2 (-300, 0));
					clone2.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 10f) * new Vector2 (-300, 0));
					clone3.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, 0f) * new Vector2 (-300, 0));
					clone4.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -10f) * new Vector2 (-300, 0));
					clone5.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, -20f) * new Vector2 (-300, 0));
				}
			}
			Team.team[Character.currentChar].currentFire += 1;
		} else if (Team.team[Character.currentChar].weaponType == "Gatling") { // gatling
			if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
				bulletPos += new Vector2 (+0.6f, -0.05f);
				clone = Instantiate (BulletRightGatling, bulletPos, Quaternion.identity);
				int angle = UnityEngine.Random.Range (-(minSpread + spreadRange), minSpread + spreadRange);
				clone.transform.Rotate (0, 0, angle);
				clone.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, angle) * new Vector2 (1000, 0));
			} else {
				bulletPos += new Vector2 (-0.6f, -0.05f);
				clone = Instantiate (BulletLeftGatling, bulletPos, Quaternion.identity);
				int angle = UnityEngine.Random.Range (-(minSpread + spreadRange), minSpread + spreadRange);
				clone.transform.Rotate (0, 0, angle);
				clone.GetComponent<Rigidbody2D> ().AddForce (Quaternion.Euler (0, 0, angle) * new Vector2 (-1000, 0));

			}
		} else if (Team.team[Character.currentChar].weaponType == "EnergyGun") { // energyBall
			if (Team.team[Character.currentChar].currentFire > 2)
				Team.team[Character.currentChar].currentFire = 0;
			if (Team.team[Character.currentChar].currentFire == 0) {
				audio.Play ("PlayerFire1", UnityEngine.Random.Range (1, 3));
				if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
					bulletPos += new Vector2 (+0.6f, -0.05f);
					clone = Instantiate (BulletRight, bulletPos, Quaternion.identity);
					clone.GetComponent<BulletScript> ().maxBounce = 0;
					clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1000, 250));
				} else {
					bulletPos += new Vector2 (-0.6f, -0.05f);
					clone = Instantiate (BulletLeft, bulletPos, Quaternion.identity);
					clone.GetComponent<BulletScript> ().maxBounce = 0;
					clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-1000, 250));

				}
			} else if (Team.team[Character.currentChar].currentFire == 1) {
				audio.Play ("PlayerFire2", UnityEngine.Random.Range (1, 3));
				if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
					bulletPos += new Vector2 (+0.6f, -0.05f);
					clone = Instantiate (BulletRight, bulletPos, Quaternion.identity);
					clone.GetComponent<BulletScript> ().maxBounce = 3;
					clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (1500, -100));
				} else {
					bulletPos += new Vector2 (-0.6f, -0.05f);
					clone = Instantiate (BulletLeft, bulletPos, Quaternion.identity);
					clone.GetComponent<BulletScript> ().maxBounce = 3;
					clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-1500, -100));

				}
			} else if (Team.team[Character.currentChar].currentFire == 2) {
				audio.Play ("PlayerFire3", UnityEngine.Random.Range (1, 3));
				if (transform.localRotation == Quaternion.Euler (0, 0, 0)) {
					bulletPos += new Vector2 (+0.6f, -0.05f);
					clone = Instantiate (BulletRight, bulletPos, Quaternion.identity);
					clone.GetComponent<BulletScript> ().maxBounce = 1;
					clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (2000, 50));
					GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-facing * 700, 0));
				} else {
					bulletPos += new Vector2 (-0.6f, -0.05f);
					clone = Instantiate (BulletLeft, bulletPos, Quaternion.identity);
					clone.GetComponent<BulletScript> ().maxBounce = 1;
					clone.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-2000, 50));
					GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-facing * 700, 0));
				}
			}
			Team.team[Character.currentChar].currentFire += 1;
		}

		GameObject a = Instantiate (Shell, transform.position, Quaternion.identity);
		a.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (300 * -facing, 200));
	}

	void FixedUpdate () {
		if (direction == 0) {
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (((input.x * speed) - GetComponent<Rigidbody2D> ().velocity.x) * (groundState.isGround () ? accel : airAccel), GetComponent<Rigidbody2D> ().velocity.y)); //Move player.
		}
		if ((groundState.isGround () || groundState.isWall ()) && input.x != 0 || input.y != 0) {
			canDash = true;
		}
		GetComponent<Rigidbody2D> ().velocity = new Vector2 ((input.x == 0 && groundState.isGround ()) ? 0 : GetComponent<Rigidbody2D> ().velocity.x, (input.y == 1 && groundState.isTouching ()) ? jump : GetComponent<Rigidbody2D> ().velocity.y); //Stop player if input.x is 0 (and grounded) and jump if input.y is 1
		if (groundState.isWall () && !groundState.isGround () && input.y == 1) {

			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-groundState.wallDirection () * speed * 1.3f, GetComponent<Rigidbody2D> ().velocity.y); //Add force negative to wall direction (with speed reduction)
		}
		input.y = 0;
	}

	void createDust () {
		dust.Play ();
	}

	public GameObject insult;
	public GameObject poop;
	public GameObject ripple;
	public ParticleSystem landingDust;
	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.layer == 8 && jumped) {
			audio.Play ("PlayerLand", UnityEngine.Random.Range (0.1f, 3f));
			jumped = false;
			landingDust.Play();
			animator.SetBool ("Jump", false);
		}
		if (collision.otherCollider.GetType () == typeof (CapsuleCollider2D)) {
			if (collision.gameObject.layer == 17 || collision.gameObject.layer == 18) {
				CharacterStats Character = GetComponent<CharacterStats> ();
				// Hitback
				Character.TakeDamage (1, (transform.position - collision.gameObject.transform.position).normalized);
				shake.camShake ();
				if (Character.currentAffliction == "Coprolalia") {
					StartCoroutine (Insult ());
				} else if (Character.currentAffliction == "I.B.S") {
					Instantiate (poop, transform.position, Quaternion.identity);
				} else if (Character.currentAffliction == "Sissy") {
					Character.CharacterSwitch ();
				}
			}
		} else if (collision.otherCollider.GetType () == typeof (CircleCollider2D)) {
			// do stuff only for the circle collider
		}

		//    anim.SetTrigger("isHit");
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.layer == 17 || collision.gameObject.layer == 18) { }
		if (collision.gameObject.layer == 24) {
			// tracker
			speed = 15;
			speedTimer = 5;
		}
		//    anim.SetTrigger("isHit");
	}

	IEnumerator Insult () {
		// var clone = Instantiate(insult, transform.position, Quaternion.identity);
		insult.SetActive (true);
		insult.GetComponent<InsultScript> ().changeInsult ();
		ripple.SetActive (true);
		// clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0));
		yield return new WaitForSeconds (1.5f);
		// insult.SetActive((false);
		insult.SetActive (false);
		ripple.SetActive (false);
	}
}