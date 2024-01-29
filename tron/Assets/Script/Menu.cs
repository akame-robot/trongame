using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    public GameObject canvas, canvas2;
    public InputField playerName, roomLobbyName;

    void Start()
    {
        string randomName = "player" + Random.Range(0, 1000);
        playerName.text = randomName;
    }

    public void Playgame()
    {
        if (playerName != null)
        {
            canvas.SetActive(false);
            canvas2.SetActive(true);

            Login();
        }

    }
    public void Login()
    {
        string name = playerName.text;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("sa");
        PhotonNetwork.NickName = name;
    }

    public override void OnConnected() //ele entra aqui na segunda faze
    {
        Debug.Log("connected");
        Debug.Log($"servidor:{PhotonNetwork.CloudRegion} ping {PhotonNetwork.GetPing()}");
    }
    public void ConnectButtomMatch() //vem aqui e e tenta conectar no lobby
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster() // depois ele entr aaqui na terceira fase e aqui ele busca um apartida o botao vai connetebutommatch tentra fazer entrar no lobby
    {
        Debug.Log("master entrou");
        string roomName = "room" + Random.Range(0, 10);
        roomLobbyName.text = roomName;
        RoomOptions roomOption = new RoomOptions()
        {
            MaxPlayers = 4
        };
        PhotonNetwork.CreateRoom(roomName, roomOption, TypedLobby.Default);
        Debug.Log(roomName);
        ConnectButtomMatch();
    }
    public override void OnJoinedRoom()//e aqui vai entrar na sala criado no onjoinedramdomfailed 
    {
        Debug.Log("joined de room");
        Debug.Log($"room nanme: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"numero de player: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public void JoinLObbyButtom()
    {
        string lobbyName = roomLobbyName.text;
        PhotonNetwork.JoinLobby(new TypedLobby(lobbyName, LobbyType.Default));
    }
}
    
