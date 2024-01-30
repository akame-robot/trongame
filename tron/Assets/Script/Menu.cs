using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Menu : MonoBehaviourPunCallbacks
{
    public GameObject canvas, canvas2;
    public InputField playerName, roomLobbyName, enterLobby;

    public TextMeshProUGUI palyerCount;

    //PhotonView photon e utilizado para saber que objeto e do meu pc usando photon.ismine  photonetcwork.localplayer.actornumber int number = photonetcwork.localplayer.actornumber

    void Start()
    {
        string randomName = "player" + Random.Range(0, 1000);
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
    public override void OnConnectedToMaster() // depois ele entr aaqui na terceira fase e aqui ele busca um apartida o botao vai connetebutommatch tentra fazer entrar no lobby
    {
        Debug.Log("master entrou");
        ConnectButtomMatch();
    }

    public void ConnectButtomMatch() //vem aqui e e tenta conectar no lobby
    {
        PhotonNetwork.JoinLobby();
    }
    public void CreateRoom()
    {
        string lobbyName = enterLobby.text;
        RoomOptions roomOption = new RoomOptions
        {
            MaxPlayers = 4
        };
        PhotonNetwork.JoinOrCreateRoom(lobbyName, roomOption, TypedLobby.Default);
    }


    public override void OnJoinedRoom()//e aqui vai entrar na sala criado no onjoinedramdomfailed 
    {
        Debug.Log("joined de room");
        Debug.Log($"room nanme: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"numero de player: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public override void OnJoinedLobby()//quando cria uma sala aparece esse debug log aqui
    {
        PhotonNetwork.JoinRandomRoom();
    }

}

