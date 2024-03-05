using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class Move : MonoBehaviourPunCallbacks, IPunObservable
{
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode rightKey;
    public KeyCode leftKey;
    public float speed = 16;
    public GameObject wallPrefab;
    private List<GameObject> wallList = new List<GameObject>();
    private Vector2 moveDirection;
    private float timeBetweenWalls = 0.1f;
    private float timeSinceLastWall = 0f;
    private List<Vector3> wallPositions = new List<Vector3>();

    void Start()
    {
        if (photonView.IsMine)
        {
            moveDirection = Vector2.up;
            GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        // Move o jogador
        HandleMovement();

        // Verifica se é hora de criar uma nova parede
        timeSinceLastWall += Time.deltaTime;
        if (timeSinceLastWall >= timeBetweenWalls)
        {
            CreateWall();
            timeSinceLastWall = 0f;
        }
        
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(upKey) && moveDirection != -Vector2.up)
            moveDirection = Vector2.up;
        else if (Input.GetKeyDown(downKey) && moveDirection != Vector2.up)
            moveDirection = -Vector2.up;
        else if (Input.GetKeyDown(rightKey) && moveDirection != -Vector2.right)
            moveDirection = Vector2.right;
        else if (Input.GetKeyDown(leftKey) && moveDirection != Vector2.right)
            moveDirection = -Vector2.right;

        GetComponent<Rigidbody2D>().velocity = moveDirection * speed;
    }

    private void CreateWall()
    {
        // Cria uma parede na posição atual
        Vector3 wallPosition = transform.position;
        GameObject wallObject = PhotonNetwork.Instantiate(wallPrefab.name, wallPosition, Quaternion.identity);

        // Destroi a parede após 5 segundos
        Destroy(wallObject, 5f);

        // RPC para sincronizar a criação da parede com outros clientes
        photonView.RPC("SpawnWallRPC", RpcTarget.All, wallPosition, moveDirection);
    }

    [PunRPC]
    void SpawnWallRPC(Vector3 startPosition, Vector2 direction)
    {
        // Cria uma parede baseada na direção e na posição inicial
        Vector3 endPosition = startPosition + (Vector3)direction;
        GameObject wallObject = PhotonNetwork.Instantiate(wallPrefab.name, startPosition, Quaternion.identity);
        wallObject.GetComponent<Wall>().SetPositions(startPosition, endPosition);
        wallList.Add(wallObject);
        Destroy(wallObject, 20f);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(wallList.Count);
            foreach (var wall in wallList)
            {
                stream.SendNext(wall.transform.position);
                stream.SendNext(wall.GetComponent<Wall>().GetEndPosition());
            }
        }
        else
        {
            int wallCount = (int)stream.ReceiveNext();
            for (int i = 0; i < wallCount; i++)
            {
                Vector3 wallPosition = (Vector3)stream.ReceiveNext();
                Vector3 wallEndPosition = (Vector3)stream.ReceiveNext();

                bool wallExists = false;
                foreach (var wall in wallList)
                {
                    if (wall.transform.position == wallPosition)
                    {
                        wallExists = true;
                        break;
                    }
                }

                if (!wallExists)
                {
                    GameObject newWall = PhotonNetwork.Instantiate(wallPrefab.name, wallPosition, Quaternion.identity);
                    newWall.GetComponent<Wall>().SetPositions(wallPosition, wallEndPosition);
                    wallList.Add(newWall);
                }
            }
        }
    }
}
