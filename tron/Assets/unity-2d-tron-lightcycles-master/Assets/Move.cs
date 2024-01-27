using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	// Movement keys (customizable in inspector)
	public KeyCode upKey;
	public KeyCode downKey;
	public KeyCode rightKey;
	public KeyCode leftKey;

	// Movement Speed
	public float speed = 16;

	// Wall Prefab
	public GameObject wallPrefab;

	// Current Wall
	Collider2D wall;

	// Last Wall's End
	Vector2 lastWallEnd;

	// Use this for initialization
	void Start () {
		// Initial Movement Direction
		GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
		spawnWall ();
	}
	
	// Update is called once per frame
	void Update () {
		// Check for key presses
		if (Input.GetKeyDown (upKey)) {
			GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
			spawnWall ();
		} else if (Input.GetKeyDown (downKey)) {
			GetComponent<Rigidbody2D>().velocity = -Vector2.up * speed;
			spawnWall ();
		} else if (Input.GetKeyDown (rightKey)) {
			GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
			spawnWall ();
		} else if (Input.GetKeyDown (leftKey)) {
			GetComponent<Rigidbody2D>().velocity = -Vector2.right * speed;
			spawnWall ();
		}

		fitColliderBetween (wall, lastWallEnd, transform.position);
	}

	void spawnWall() {
		// Save last wall's position
		lastWallEnd = transform.position;

		// Spawn a new Lightwall
		GameObject g = (GameObject)Instantiate (wallPrefab, transform.position, Quaternion.identity);
		wall = g.GetComponent<Collider2D>();
	}

	void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b) {
		// Calculate the Center Position
		co.transform.position = a + (b - a) * 0.5f;

		// Scale it (horizontally or vertically)
		float dist = Vector2.Distance (a, b);
		if (a.x != b.x)
			co.transform.localScale = new Vector2 (dist + 1, 1);
		else
			co.transform.localScale = new Vector2 (1, dist + 1);
	}

	void OnTriggerEnter2D(Collider2D co) {
		if (co != wall) {
			print ("Player lost:" + name);
			Destroy (gameObject);
		}
	}
}
