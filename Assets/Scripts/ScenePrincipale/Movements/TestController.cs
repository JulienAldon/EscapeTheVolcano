using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;

public class TestController : MonoBehaviour
{
	public class GroundState
	{
		private GameObject player;
		private float  width;
		private float height;
		private float length;
		public bool IsWall;

		//GroundState constructor.  Sets offsets for raycasting.
		public GroundState(GameObject playerRef)
		{
			player = playerRef;
			width = player.GetComponent<Collider2D>().bounds.extents.x + 0.2f;
			height = player.GetComponent<Collider2D>().bounds.extents.y + 0.2f;
			length = 0.5f;
		}
 
		//Returns whether or not player is touching wall.
		public bool isWall()
		{
			int LayerIndex = LayerMask.NameToLayer("Ground");
			int layerMask = (1 << LayerIndex);
//			layerMask = ~layerMask;

			bool smth = Physics2D.OverlapCircle(player.transform.position, width, layerMask);

			bool left = Physics2D.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length, layerMask);
//			bool left_down = Physics2D.Raycast(new Vector2(player.transform.position.x - height / 2 - width, player.transform.position.y), -Vector2.right, length, layerMask);
//			bool left_up = Physics2D.Raycast(new Vector2(player.transform.position.x + height / 2 - width, player.transform.position.y), -Vector2.right, length, layerMask);
			bool right = Physics2D.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length, layerMask);
//			bool right_down = Physics2D.Raycast(new Vector2(player.transform.position.x - height / 2 + width, player.transform.position.y), Vector2.right, length, layerMask);
//			bool right_up = Physics2D.Raycast(new Vector2(player.transform.position.x + height / 2 + width, player.transform.position.y), Vector2.right, length, layerMask);
			if(left || right || smth)
				return true;
			else
				return false;
		}
 
		//Returns whether or not player is touching ground.
		public bool isGround()
		{
			int LayerIndex = LayerMask.NameToLayer("Ground");
			int layerMask = (1 << LayerIndex);
//			layerMask = ~layerMask;
			bool bottom1 = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y - height), -Vector2.up, length, layerMask);
//			bool bottom2 = Physics2D.Raycast(new Vector2(player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
//			bool bottom3 = Physics2D.Raycast(new Vector2(player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
			if(bottom1) //  || bottom2 || bottom3
				return true;
			else
				return false;
		}
 
		//Returns whether or not player is touching wall or ground.
		public bool isTouching()
		{
			if(isGround() || isWall())
				return true;
			else
				return false;
		}
 
		//Returns direction of wall.
		public int wallDirection()
		{
			int LayerIndex = LayerMask.NameToLayer("Ground");
			int layerMask = (1 << LayerIndex);

			bool left = Physics2D.Raycast(new Vector2(player.transform.position.x - width, player.transform.position.y), -Vector2.right, length, layerMask);
			bool right = Physics2D.Raycast(new Vector2(player.transform.position.x + width, player.transform.position.y), Vector2.right, length, layerMask);
			if (left)
				return -1;
			else if (right)
				return 1;
			else
				return 0;
		}
	}
 
	//Feel free to tweak these values in the inspector to perfection.  I prefer them private.
	[Header("Movement")]
	public float    speed = 14f;
	public float    accel = 6f;
	public float airAccel = 3f;
	public float     jump = 14f;  //I could use the "speed" variable, but this is only coincidental in my case.  Replace line 89 if you think otherwise.

	private float jumpTimeCounter;
	public float jumpTime;
	private bool isJumping;
	
	[Header("Shooting")]
	public GameObject BulletLeft;
	public GameObject BulletRight;
	public GameObject Shell;
	public float FireRate = 0.5f;
	float nextFire = 0;
	private bool isFiring = false;

	[Header("Effects")]
	public Animator animator;
	public ParticleSystem dust;
	private GroundState groundState;
	private Shake shake;

	[Header("Runner")]
	public float dashSpeed;
	public float startDashTime;

	private float dashTime;
	private int direction = 0;
	private bool canDash = true;
	
	[Header("Climber")]
	public DistanceJoint2D joint;
	public LineRenderer line;
	public float step = 0.1f;
	private int facing;

	[Header("Tracker")]
	// Tracker
	public GameObject FlagSprite;
	public float FlagRate;
	private float NextFlag;

	[Header("Grenadier")]
	public GameObject Bomb;
	public float BombRate;
	private float nextBomb = 0;
	
	[Header("Tank")]
	public GameObject Shield;
	public float ShieldTime = 15f;
	private float usedTimeShield;
	private bool canShield = true;

	void Start()
	{
		//Create an object to check if player is grounded or touching wall
		groundState = new GroundState(transform.gameObject);
		shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
		//dash
		dashTime = startDashTime;
		//grappling
		joint = GetComponent<DistanceJoint2D>();
		joint.enabled = false;
		line.enabled = false;
		facing = 1;
	}
 
	private Vector2 input;
    Vector2 bulletPos; 
	
	void Update()
	{
		//Handle input
		if(groundState.isTouching() && Input.GetKeyDown(KeyBindScript.keys["Jump"])) {
			isJumping = true;
			jumpTimeCounter = jumpTime;
			if (groundState.isTouching())
				createDust();
			input.y = 1;
			animator.SetTrigger("Jumping");			
		}
		if (Input.GetKey(KeyBindScript.keys["Jump"]) && isJumping == true) {
			if (jumpTimeCounter > 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1 * jump);
				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
			}
		}
		if (Input.GetKeyUp(KeyBindScript.keys["Jump"])) {
			isJumping = false;
		}

		if(Input.GetKey(KeyBindScript.keys["Left"])) {
			input.x = -1;
			if (transform.localRotation.y == 0) {
				if (groundState.isGround())
					createDust();
				if (!isFiring) {
					transform.localRotation = Quaternion.Euler(0, 180, 0);
					facing = -1;
				}
			}
			if (!isJumping)
				animator.SetBool("Running", true);
		} else if(Input.GetKey(KeyBindScript.keys["Right"])) {
			input.x = 1;
			if (transform.localRotation.y == -1) {
				if (groundState.isGround())
					createDust();
				if (!isFiring) {
					transform.localRotation = Quaternion.Euler(0, 0, 0);
					facing = 1;
				}
			}
			if (!isJumping)
				animator.SetBool("Running", true);
		} else {
			input.x = 0;
			animator.SetBool("Running", false);
		}

		if (isJumping) {
			animator.SetBool("Running", false);
		}
		if (isJumping && groundState.isGround())
			isJumping = false;
		// Fire
		if (Input.GetKey(KeyBindScript.keys["Fire"]) && Time.time > nextFire) {
			nextFire = Time.time + FireRate;
			fire();
			//gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0) * -facing * 200f);
			shake.camShake();
		}
		if (Input.GetKeyDown(KeyBindScript.keys["Fire"]))
		{
			isFiring = true;
		}
		if (Input.GetKeyUp(KeyBindScript.keys["Fire"]))
		{
			isFiring = false;
		}

		// Mechanic
		CharacterStats Character = GameObject.Find("Player").GetComponent<CharacterStats>();
		if (Character.ClassType.GetValue() == 1) // Runner
			{
				if (Input.GetKeyDown(KeyBindScript.keys["Action"]) && canDash) {
					Dash(2f, 2f);
					if (direction == 0) {
						if (input.x == -1) {
							direction = 1;
						} else if (input.x == 1) {
							direction = 2;
						}
					}
				}
				if (Input.GetKey(KeyBindScript.keys["Action"])) {
				}
				if (Input.GetKeyUp(KeyBindScript.keys["Action"])) {
				}
			}
			else if (Character.ClassType.GetValue() == 2) //Climber
			{
				if (Input.GetKeyDown(KeyBindScript.keys["Action"])) {
					//create the link (raycast ...)
					int LayerIndex = LayerMask.NameToLayer("Ground");
					int layerMask = (1 << LayerIndex);
					RaycastHit2D hit;

					hit = Physics2D.Raycast(transform.position, new Vector2(facing < 0 ? -0.5f : 0.5f , 1), Mathf.Infinity, layerMask);
					joint.enabled = true;
					joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
					joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
					joint.distance = Vector2.Distance(transform.position, hit.point);

					line.enabled = true;
					line.SetPosition(0, transform.position);
					line.SetPosition(1, hit.point);
					
				}
				if (Input.GetKey(KeyBindScript.keys["Action"])) {
					if (joint.distance > 0.2f ){
						joint.distance -= step;
					} else {
						joint.enabled = false;
						line.enabled = false;
					}
					line.SetPosition(0, transform.position);
				}
				if (Input.GetKeyUp(KeyBindScript.keys["Action"])) {
					// delete the link
					joint.enabled = false;
					line.enabled = false;
				}
			}
			else if (Character.ClassType.GetValue() == 3) // Hacker
			{
				GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
				GameObject turret = GetClosestTurret(turrets);
				if (Input.GetKeyDown(KeyBindScript.keys["Action"])) {
					// Show range
					foreach (GameObject n in turrets) {
						if (!n.transform.GetChild(1).gameObject.GetComponent<turretScript>().getDeactivation())
							n.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
						else
							n.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				if (Input.GetKey(KeyBindScript.keys["Action"])) {
					turret.transform.GetChild(1).gameObject.GetComponent<turretScript>().hack(transform.position);
				}
				if (Input.GetKeyUp(KeyBindScript.keys["Action"])) {
					// Hide range
					foreach (GameObject n in turrets) {
						n.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
						n.transform.GetChild(1).gameObject.GetComponent<turretScript>().resetHack();
					}
				}
			}
			else if (Character.ClassType.GetValue() == 4) // Tracker
			{
				if (Input.GetKeyDown(KeyBindScript.keys["Action"]) && Time.time > NextFlag && groundState.isGround() && Character.nbFlags > 0) {
					NextFlag = Time.time + FlagRate;
					Character.nbFlags -= 1;
					// Spawn a landmark
					Instantiate(FlagSprite, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
				}
				if (Input.GetKey(KeyBindScript.keys["Action"])) {
				}
				if (Input.GetKeyUp(KeyBindScript.keys["Action"])) {
				}	
			}
			else if (Character.ClassType.GetValue() == 5 || true) // Tank
			{
				if (Shield.activeSelf == true) {
					usedTimeShield += Time.deltaTime;
				}
				if (usedTimeShield >= ShieldTime) {
					canShield = false;
					Shield.SetActive(false);					
				}
				if (Input.GetKeyDown(KeyBindScript.keys["Action"])) {
					if (Shield.activeSelf == false && canShield) {
						Shield.SetActive(true);
					} else {
						Shield.SetActive(false);
					}
				}
				if (Input.GetKey(KeyBindScript.keys["Action"])) {
				}
				if (Input.GetKeyUp(KeyBindScript.keys["Action"])) {
				}
			}
			else if (Character.ClassType.GetValue() == 6) // Grenadier
			{
				if (Input.GetKeyDown(KeyBindScript.keys["Action"])) {
					shootBomb();
				}
				if (Input.GetKey(KeyBindScript.keys["Action"])) {
					shootBomb();
				}
				if (Input.GetKeyUp(KeyBindScript.keys["Action"])) {
				}
			}

	}

	private void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
      
        //hasDashed = true;

        //anim.SetTrigger("dash");

        // rb.velocity = Vector2.zero;
        // Vector2 dir = new Vector2(x, y);

        // rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();
        //StartCoroutine(GroundDash());
        // DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        // dashParticle.Play();
        // rb.gravityScale = 0;
        // GetComponent<BetterJumping>().enabled = false;
        // wallJumped = true;
        // isDashing = true;

        yield return new WaitForSeconds(.3f);

        // dashParticle.Stop();
        // rb.gravityScale = 3;
        // GetComponent<BetterJumping>().enabled = true;
        // wallJumped = false;
        // isDashing = false;
    }

	GameObject GetClosestTurret(GameObject[] turrets) 
	{
		GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in turrets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr && !potentialTarget.transform.GetChild(1).gameObject.GetComponent<turretScript>().getDeactivation())
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
	}

	void shootBomb() 
	{
		if (Time.time > nextBomb) {
			nextBomb = Time.time + BombRate;
			//Throw a bomb
			GameObject clone;
			clone = Instantiate(Bomb, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
			clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(600 * facing, 400));
		}
	}

    void fire() 
    {
        bulletPos = transform.position;
		if (transform.localRotation == Quaternion.Euler(0, 0, 0)) {
			bulletPos += new Vector2(+0.6f, -0.05f);
	        Instantiate(BulletRight, bulletPos, Quaternion.identity);
		}
		else {
			bulletPos += new Vector2(-0.6f, -0.05f);
	        Instantiate(BulletLeft, bulletPos, Quaternion.identity);
		}
		GameObject a = Instantiate(Shell, transform.position, Quaternion.identity);
		a.GetComponent<Rigidbody2D>().AddForce(new Vector2(300 * -facing, 200));
    }

	void FixedUpdate()
	{
		if (direction == 0) {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(((input.x * speed) - GetComponent<Rigidbody2D>().velocity.x) * (groundState.isGround() ? accel : airAccel), GetComponent<Rigidbody2D>().velocity.y)); //Move player.
		} else {
			if (dashTime <= 0) {
				direction = 0;
				dashTime = startDashTime;
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			} else {
				dashTime -= Time.deltaTime;
				if (direction == 1) {
					GetComponent<Rigidbody2D>().velocity = Vector2.left * dashSpeed;
					
				} else if (direction == 2) {
					GetComponent<Rigidbody2D>().velocity = Vector2.right * dashSpeed;
				}
				canDash = false;
			}
		}
		if ((groundState.isGround() || groundState.isWall()) && input.x != 0 || input.y != 0) {
			canDash = true;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2((input.x == 0 && groundState.isGround()) ? 0 : GetComponent<Rigidbody2D>().velocity.x, (input.y == 1 && groundState.isTouching()) ? jump : GetComponent<Rigidbody2D>().velocity.y); //Stop player if input.x is 0 (and grounded) and jump if input.y is 1
		if(groundState.isWall() && !groundState.isGround() && input.y == 1) {

			GetComponent<Rigidbody2D>().velocity = new Vector2(-groundState.wallDirection() * speed * 1.3f, GetComponent<Rigidbody2D>().velocity.y); //Add force negative to wall direction (with speed reduction)
		}
		input.y = 0;
	}

	void createDust()
	{
		dust.Play();
	}
}
