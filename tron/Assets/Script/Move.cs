﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Move : MonoBehaviourPunCallbacks, IPunObservable
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

    private GameObject localWall;
    private GameObject createdWall;


    // Use this for initialization
    void Start()
    {
        // Initial Movement Direction
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        if (photonView.IsMine)
        {
            //spawnWall();
            spawnWallRPC();
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
            //spawnWall();
            spawnWallRPC();
        }
        else if (Input.GetKeyDown(downKey))
        {
            GetComponent<Rigidbody2D>().velocity = -Vector2.up * speed;
            //spawnWall();
            spawnWallRPC();
        }
        else if (Input.GetKeyDown(rightKey))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
            //spawnWall();
            spawnWallRPC();
        }
        else if (Input.GetKeyDown(leftKey))
        {
            GetComponent<Rigidbody2D>().velocity = -Vector2.right * speed;
            //spawnWall();
            spawnWallRPC();
        }

        if (wall != null)
        {
            fitColliderBetween(wall, lastWallEnd, (Vector2)transform.position);
        }
    }

    //[PunRPC]
    //void spawnWallRPC()
    //{
    //    lastWallEnd = (Vector2)transform.position;
    //    GameObject wallObject = PhotonNetwork.Instantiate(wallPrefab.name, transform.position, Quaternion.identity);
    //    Collider2D wallCollider = wallObject.GetComponent<Collider2D>();
    //    fitColliderBetween(wallCollider, lastWallEnd, (Vector2)transform.position);
    //}

    [PunRPC]
    void spawnWallRPC()
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

        //if (co.CompareTag("Wall") && !co.GetComponent<Wall>().IsOwner(gameObject))
        //{
        //    Debug.Log("Player lost: " + photonView.Owner.NickName);
        //    PhotonNetwork.Destroy(gameObject);
        //}
    }

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Write wall data
            Collider2D[] walls = GameObject.FindObjectsOfType<Collider2D>();
            foreach (Collider2D wallCollider in walls)
            {
                if (wallCollider.CompareTag("Wall"))
                {
                    stream.SendNext(wallCollider.transform.position);
                    stream.SendNext(wallCollider.transform.rotation);
                }
            }
        }
        else
        {
            // Read wall data and instantiate if necessary
            int wallCount = stream.Count / 2;
            for (int i = 0; i < wallCount; i++)
            {
                Vector3 wallPosition = (Vector3)stream.ReceiveNext();
                Quaternion wallRotation = (Quaternion)stream.ReceiveNext();

                // Check if wall already exists
                bool wallExists = false;
                Collider2D[] existingWalls = GameObject.FindObjectsOfType<Collider2D>();
                foreach (Collider2D existingWall in existingWalls)
                {
                    if (existingWall.CompareTag("Wall") && existingWall.transform.position == wallPosition)
                    {
                        wallExists = true;
                        break;
                    }
                }

                // Instantiate wall if it doesn't exist
                if (!wallExists)
                {
                    GameObject newWall = Instantiate(wallPrefab, wallPosition, wallRotation);
                    newWall.tag = "Wall";
                }
            }
        }
    }
}