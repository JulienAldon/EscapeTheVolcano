using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
	public static LevelGeneration current;
	public bool isDone = false;
	public Transform[] startingPositions;
	public GameObject[] rooms; // index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRBT
	public GameObject spawn;

	public GameObject AstarPath;
	public GameObject wayOut;
	public GameObject Player;
	public float moveAmount;
	public float startTimeBtwRoom = 0.25f;

	public float minX;
	public float maxX;
	public float minY;

	public Tilemap tilemap;
	public TileBase wall;

	public LayerMask room;
	public bool stopGeneration;

	private int downCounter;
	private int direction;
	private float timeBtwRoom;
	private bool once = true;
	private Vector2 start;
	// Start is called before the first frame update
	void Awake()
	{
		current = this;
	}

	void Start()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName("ScenePrincipale"));
		int randStartingPos = Random.Range(0, startingPositions.Length);
		transform.position = startingPositions[randStartingPos].position;
		Level.FirstRoom = Instantiate(rooms[0], transform.position, Quaternion.identity);
		start = transform.position;
		direction = Random.Range(1, 6);
	}

	private void Move()
	{
		if (direction == 1 | direction == 2) { // Move Right
			if (transform.position.x < maxX) {
				downCounter = 0;
				Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
				transform.position = newPos;

				int rand = Random.Range(0, rooms.Length);
				Level.path.Add(Instantiate(rooms[rand], transform.position, Quaternion.identity));

				direction = Random.Range(1, 6);
				if (direction == 3) {
					direction = 2;
				} else if (direction == 4) {
					direction = 5;
				}
			} else {
				direction = 5;
			}
		} else if (direction == 3 | direction == 4) { // Move Left
			if (transform.position.x > minX) {
				downCounter = 0;
				Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
				transform.position = newPos;

				int rand = Random.Range(0, rooms.Length);
				Level.path.Add(Instantiate(rooms[rand], transform.position, Quaternion.identity));

				direction = Random.Range(3, 6);
			} else {
				direction = 5;
			}
		} else if (direction == 5) { // Move Down
			downCounter++;
			if (transform.position.y > minY) {
				Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
				if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3) {
					if (downCounter >= 2) {
						roomDetection.GetComponent<RoomType>().RoomDestruction();
						Level.path.Add(Instantiate(rooms[3], transform.position, Quaternion.identity));
					} else {
						roomDetection.GetComponent<RoomType>().RoomDestruction();

						int randBottomRoom = Random.Range(1, 4);
						if (randBottomRoom == 2) {
							randBottomRoom = 1;
						}
						Level.path.Add(Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity));
					}
				}
				Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
				transform.position = newPos;

				int rand = Random.Range(2, 4);
				Level.path.Add(Instantiate(rooms[rand], transform.position, Quaternion.identity));

				direction = Random.Range(1, 6);
			} else {
				// Stop level Gen
				Instantiate(wayOut, new Vector2(transform.position.x, transform.position.y - 2.0f), Quaternion.identity);
				stopGeneration = true;
			}
		}
	}
	// Update is called once per frame
	void Update()
	{
		if (timeBtwRoom <= 0 && stopGeneration == false) {
			Move();
			timeBtwRoom = startTimeBtwRoom;
		} else {
			timeBtwRoom -= Time.deltaTime;
		}
		if (stopGeneration == true && once == true)
		{
			once = false;
			StartCoroutine(SpawnPlayer());
		}
		AstarPath.GetComponent<AstarPath>().Scan();
	}

	IEnumerator SpawnPlayer()
	{
		int LayerIndex1 = 1 << LayerMask.NameToLayer("Ground");
		int LayerIndex2 = 1 << LayerMask.NameToLayer("Ennemy");
		int LayerIndex3 = 1 << LayerMask.NameToLayer("Turret");
		int layerMask = LayerIndex1 | LayerIndex2 | LayerIndex3;
		// Collider2D[] res = new Collider2D[20];
		// ContactFilter2D f = new ContactFilter2D();
		// f.layerMask = layerMask;
		var res = Physics2D.OverlapCircleAll(start, 3f, layerMask);
		if (res.Length > 0) {
			foreach (var item in res)
			{
				if(item) {
					print(item.gameObject);
					Destroy(item.gameObject);
				}
			}
		}
		Instantiate(spawn, start, Quaternion.identity);
		GameObject playerStart = GameObject.FindGameObjectsWithTag("SpawnPlayer")[0];
		Player.transform.position = playerStart.transform.position;
		Level.path.RemoveAll(item => item == null);

		yield return new WaitForSeconds(1f);
		
		var Ground = GameObject.FindGameObjectsWithTag("Ground");
		foreach (var obj in Ground) {
			tilemap.SetTile(new Vector3Int((int)(obj.transform.position.x), (int)(Mathf.Round(obj.transform.position.y- 0.5f) ), (int)(obj.transform.position.z)), wall);
		}
		
		isDone = true;
	}
}
