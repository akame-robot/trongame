using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Connect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }
    public void Login() //quando ele entra aqui ele ele conectar o player dando o nome kaiki
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("sa");
        PhotonNetwork.NickName = "kaiki";
    }

    public void ButtomCreateRoom()
    {
        string roomName = "nome temporario";
        RoomOptions roomOption = new RoomOptions()
        {
            MaxPlayers = 4
        };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOption, TypedLobby.Default);
    }

    public void ConnectButtomMatch() //vem aqui e e tenta conectar no lobby
    {
        PhotonNetwork.JoinLobby();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public override void OnConnected() //ele entra aqui na segunda faze
    {
        Debug.Log("connected");
        Debug.Log($"servidor:{PhotonNetwork.CloudRegion} ping {PhotonNetwork.GetPing()}");
    }

    public override void OnConnectedToMaster() // depois ele entr aaqui na terceira fase e aqui ele busca um apartida o botao vai connetebutommatch tentra fazer entrar no lobby
    {
        Debug.Log("master connected");
        ConnectButtomMatch();
    }

    public override void OnJoinedLobby()//quando cria uma sala aparece esse debug log aqui
    {
        Debug.Log("created lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // ele entra aqui caso ele falha conectar uma sala e no final cria uma sala
    {
        Debug.Log("failed and i gonna create a lobby");
        string roomName = "room " + Random.Range(0, 1000);
        PhotonNetwork.CreateRoom(roomName);
    }
    public override void OnJoinedRoom()//e aqui vai entrar na sala criado no onjoinedramdomfailed 
    {
        Debug.Log("joined de room");
        Debug.Log($"room nanme: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"numero de player: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
}
