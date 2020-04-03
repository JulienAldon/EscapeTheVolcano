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
			int LayerIndex = LayerMask.NameToLayer("Ground");
			int layerMask = (1 << LayerIndex);
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

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
		groundState = new GroundState(transform.gameObject);
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        Vector2 velocity = rb.velocity;
        velocity.x = force.x;
        rb.velocity = velocity;

        if (direction.y > 0.01 && groundState.isGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, 1 * jumpspeed);
        } else {
            rb.velocity = new Vector2(force.x, rb.velocity.y);
        }
        if (groundState.isWall() && !groundState.isGround()) {
            rb.velocity = new Vector2(-groundState.wallDirection() * 4f * 0.75f, rb.velocity.y); //Add force negative to wall direction (with speed reduction)
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
    
    public bool isGround()
    {
        int LayerIndex = LayerMask.NameToLayer("Ground");
        int layerMask = (1 << LayerIndex);
//			layerMask = ~layerMask;
        bool bottom1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - GetComponent<Collider2D>().bounds.extents.y), -Vector2.up, 0.5f, layerMask);
//			bool bottom2 = Physics2D.Raycast(new Vector2(player.transform.position.x + (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
//			bool bottom3 = Physics2D.Raycast(new Vector2(player.transform.position.x - (width - 0.2f), player.transform.position.y - height), -Vector2.up, length, layerMask);
        if(bottom1) //  || bottom2 || bottom3
            return true;
        else
            return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 19 || collision.gameObject.layer == 12) {
            Destroy(gameObject);
        }
    }

    // void OnTriggerEnter2D(Collider2D collision) 
    // {
    // }
}
