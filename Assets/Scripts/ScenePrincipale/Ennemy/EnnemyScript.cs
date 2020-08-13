using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnnemyScript : MonoBehaviour
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
			width = player.GetComponent<Collider2D>().bounds.extents.x + 0.4f;
			height = player.GetComponent<Collider2D>().bounds.extents.y + 0.5f;
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
			
            int gnd = 1 << LayerMask.NameToLayer("Ground");
            int ally = 1 << LayerMask.NameToLayer("Ennemy");
			int layerMask = gnd | ally;
//			layerMask = ~layerMask;
			bool bottom1 = Physics2D.Raycast(new Vector2(player.transform.position.x, player.transform.position.y - height), -Vector2.up, length, layerMask);
			bool bottom2 = Physics2D.Raycast(new Vector2(player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
			bool bottom3 = Physics2D.Raycast(new Vector2(player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
			if(bottom1 || bottom2 || bottom3)
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

    public GameObject BulletLeft;
	public GameObject BulletRight;
	public GameObject chargeEffect;
	
    private Shake shake;
    public GameObject blood; 
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpspeed = 9f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private GroundState groundState;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Vector2 bulletPos; 

    public float FireRate = 2f;
	float nextFire = 0;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
		groundState = new GroundState(transform.gameObject);
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
		shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();

        InvokeRepeating("UpdatePath", 0f, .5f);
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

    public bool GetReachedEndOfPath()
    {
        return (reachedEndOfPath);
    }

    void FixedUpdate()
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

        if (Vector2.Distance(transform.position, target.position) > 4f && transform.position.x != target.position.x)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            Vector2 velocity = rb.velocity;
            velocity.x = force.x;
            rb.velocity = velocity;

            if (force.x > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            } else if (force.x < 0) {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

            if (direction.y > 0.5 && groundState.isGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, 1 * jumpspeed);
            } else if (direction.y > 0.01 && groundState.isWall()) {
                rb.velocity = new Vector2(rb.velocity.x, jumpspeed); //Add force negative to wall direction (with speed reduction)
            } else if (direction.y < 0 && !groundState.isGround()) {
                rb.velocity = new Vector2(0, -jumpspeed); //Add force negative to wall direction (with speed reduction)
            } else {
                rb.velocity = new Vector2(force.x, rb.velocity.y);
            }
            
            
        } else {
            // fire
            if (Time.time > nextFire) {
				nextFire = Time.time + FireRate;                
                StartCoroutine(Shoot());
            }
        }

        // if (force.y > 0) 
        // {
        //     velocity.y = force.y;
        //     rb.velocity = velocity;
        // }


        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint ++;
        }
    }

    IEnumerator Shoot()
    {
        // Start Shoot anim
        var clone = Instantiate(chargeEffect, transform.position, Quaternion.identity);
        clone.transform.parent = transform;
        yield return new WaitForSeconds(1f);
        
        bulletPos = transform.position;
        Vector2 dir = target.position - transform.position;
        if (dir.x > 0) {
			bulletPos += new Vector2(+0.6f, -0.05f);            
            Instantiate(BulletRight, bulletPos, Quaternion.identity);
        } else if (dir.x < 0) {
			bulletPos += new Vector2(-0.6f, -0.05f);            
	        Instantiate(BulletLeft, bulletPos, Quaternion.identity);            
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

    void OnParticleCollision(GameObject other)
	{
		GetComponent<Rigidbody2D>().AddForce(new Vector2( 100 * (transform.position.x - other.transform.position.x ), 100 * (transform.position.y - other.transform.position.y)));
	}

}
