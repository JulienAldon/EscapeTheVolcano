using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
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
	public float    speed = 14f;
	public float    accel = 6f;
	public float airAccel = 3f;
	public float     jump = 14f;  //I could use the "speed" variable, but this is only coincidental in my case.  Replace line 89 if you think otherwise.

	// Timed jumping
	private float jumpTimeCounter;
	public float jumpTime;
	private bool isJumping;
	
	//Shoot
	public GameObject BulletLeft, BulletRight;
	public GameObject Shell;
	public float FireRate = 0.5f;
	public Animator animator;
	float nextFire = 0;

	public ParticleSystem dust;
	private GroundState groundState;
	private Shake shake;

	// Dash (runner)
	public float dashSpeed;
	public float startDashTime;

	private float dashTime;
	private int direction = 0;
	private bool canDash = true;
	
	// Grappling
	public DistanceJoint2D joint;
	public LineRenderer line;
	public float step = 0.1f;
	private int facing;

	// Tracker
	public GameObject FlagSprite;
	public float FlagRate;
	private float NextFlag;

	// Grenadier
	public GameObject Bomb;
	public float BombRate;
	private float nextBomb = 0;

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
		if(groundState.isTouching() && Input.GetKeyDown(KeyCode.Space)) {
			isJumping = true;
			jumpTimeCounter = jumpTime;
			if (groundState.isTouching())
				createDust();
			input.y = 1;
		}
		if (Input.GetKey(KeyCode.Space) && isJumping == true) {
			if (jumpTimeCounter > 0) {
				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 1 * jump);
				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
			}
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			isJumping = false;
		}

		if(Input.GetKey(KeyCode.LeftArrow)) {
			input.x = -1;
			if (transform.localRotation.y == 0) {
				if (groundState.isGround())
					createDust();
				transform.localRotation = Quaternion.Euler(0, 180, 0);
				facing = -1;
			}
			animator.SetBool("Running", true);
		} else if(Input.GetKey(KeyCode.RightArrow)) {
			input.x = 1;
			if (transform.localRotation.y == -1) {
				if (groundState.isGround())
					createDust();
				transform.localRotation = Quaternion.Euler(0, 0, 0);
				facing = 1;
			}
			animator.SetBool("Running", true);
		} else {
			input.x = 0;
			animator.SetBool("Running", false);
		}

		if (groundState.isGround() == false) {
			animator.SetBool("Running", false);
			animator.SetBool("Jumping", true);
		} else {
			animator.SetBool("Jumping", false);
		}

		// Fire
		if (Input.GetKey(KeyCode.E) && Time.time > nextFire) {
			nextFire = Time.time + FireRate;
			fire();
			gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0) * -facing * 200f);
			shake.camShake();
		}

		// Mechanic
		CharacterStats Character = GameObject.Find("Player").GetComponent<CharacterStats>();
		if (Character.ClassType.GetValue() == 1) // Runner
			{
				if (Input.GetKeyDown(KeyCode.A) && canDash) {
					if (direction == 0) {
						if (input.x == -1) {
							direction = 1;
						} else if (input.x == 1) {
							direction = 2;
						}
					}
				}
				if (Input.GetKey(KeyCode.A)) {
				}
				if (Input.GetKeyUp(KeyCode.A)) {
				}
			}
			else if (Character.ClassType.GetValue() == 2) //Climber
			{
				if (Input.GetKeyDown(KeyCode.A)) {
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
				if (Input.GetKey(KeyCode.A)) {
					if (joint.distance > 0.2f ){
						joint.distance -= step;
					} else {
						joint.enabled = false;
						line.enabled = false;
					}
					line.SetPosition(0, transform.position);
				}
				if (Input.GetKeyUp(KeyCode.A)) {
					// delete the link
					joint.enabled = false;
					line.enabled = false;
				}
			}
			else if (Character.ClassType.GetValue() == 3) // Hacker
			{
				GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
				GameObject turret = GetClosestTurret(turrets);
				if (Input.GetKeyDown(KeyCode.A)) {
					// Show range
					foreach (GameObject n in turrets) {
						if (!n.transform.GetChild(1).gameObject.GetComponent<turretScript>().getDeactivation())
							n.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = true;
						else
							n.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				if (Input.GetKey(KeyCode.A)) {
					turret.transform.GetChild(1).gameObject.GetComponent<turretScript>().hack(transform.position);
				}
				if (Input.GetKeyUp(KeyCode.A)) {
					// Hide range
					foreach (GameObject n in turrets) {
						n.transform.GetChild(3).GetComponent<SpriteRenderer>().enabled = false;
						n.transform.GetChild(1).gameObject.GetComponent<turretScript>().resetHack();
					}
				}
			}
			else if (Character.ClassType.GetValue() == 4) // Tracker
			{
				if (Input.GetKeyDown(KeyCode.A) && Time.time > NextFlag && groundState.isGround() && Character.nbFlags > 0) {
					NextFlag = Time.time + FlagRate;
					Character.nbFlags -= 1;
					// Spawn a landmark
					Instantiate(FlagSprite, new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z), Quaternion.identity);
				}
				if (Input.GetKey(KeyCode.A)) {
				}
				if (Input.GetKeyUp(KeyCode.A)) {
				}	
			}
			else if (Character.ClassType.GetValue() == 5) // Tank
			{
				if (Input.GetKeyDown(KeyCode.A)) {
					// shield
				}
				if (Input.GetKey(KeyCode.A)) {
				}
				if (Input.GetKeyUp(KeyCode.A)) {
				}
			}
			else if (Character.ClassType.GetValue() == 6) // Grenadier
			{
				if (Input.GetKeyDown(KeyCode.A)) {
					shootBomb();
				}
				if (Input.GetKey(KeyCode.A)) {
					shootBomb();
				}
				if (Input.GetKeyUp(KeyCode.A)) {
				}
			}

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
			createDust();
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
		if (groundState.isGround() || groundState.isWall()) {
			canDash = true;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2((input.x == 0 && groundState.isGround()) ? 0 : GetComponent<Rigidbody2D>().velocity.x, (input.y == 1 && groundState.isTouching()) ? jump : GetComponent<Rigidbody2D>().velocity.y); //Stop player if input.x is 0 (and grounded) and jump if input.y is 1
		if(groundState.isWall() && !groundState.isGround() && input.y == 1) {

			GetComponent<Rigidbody2D>().velocity = new Vector2(-groundState.wallDirection() * speed * 0.75f, GetComponent<Rigidbody2D>().velocity.y); //Add force negative to wall direction (with speed reduction)
		}
		input.y = 0;
	}

	void createDust()
	{
		dust.Play();
	}
}
