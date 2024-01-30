using UnityEngine;
using Photon.Pun;

public class Move : MonoBehaviourPun
{
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
    void Start()
    {
        // Initial Movement Direction
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        if (photonView.IsMine)
        {
            spawnWall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        // Check for key presses
        if (Input.GetKeyDown(upKey))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(downKey))
        {
            GetComponent<Rigidbody2D>().velocity = -Vector2.up * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(rightKey))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            spawnWall();
        }
        else if (Input.GetKeyDown(leftKey))
        {
            GetComponent<Rigidbody2D>().velocity = -Vector2.right * speed;
            spawnWall();
        }

        if (wall != null)
        {
            fitColliderBetween(wall, lastWallEnd, (Vector2)transform.position);
        }
    }

    void spawnWall()
    {
        // Save last wall's position
        lastWallEnd = (Vector2)transform.position;

        // Spawn a new Lightwall
        GameObject wallObject = PhotonNetwork.Instantiate(wallPrefab.name, transform.position, Quaternion.identity);
        wall = wallObject.GetComponent<Collider2D>();
    }

    void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b)
    {
        // Calculate the Center Position
        co.transform.position = a + (b - a) * 0.5f;

        // Scale it (horizontally or vertically)
        float dist = Vector2.Distance(a, b);
        if (a.x != b.x)
            co.transform.localScale = new Vector2(dist + 1, 1);
        else
            co.transform.localScale = new Vector2(1, dist + 1);
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co != wall)
        {
            if (photonView.IsMine)
            {
                Debug.Log("Player lost: " + photonView.Owner.NickName);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
