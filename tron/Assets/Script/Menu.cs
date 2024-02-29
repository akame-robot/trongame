using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Menu : MonoBehaviourPunCallbacks
{
    public GameObject canvas, canvas2;
    public InputField playerName, enterLobby;

    public Button buttomOff, buttomCreateLobby, buttomEnterLObby;

    public GameObject playerPrefabBlue; // Prefab do jogador azul
    public GameObject playerPrefabPink;

    //PhotonView photon e utilizado para saber que objeto e do meu pc usando photon.ismine  photonetcwork.localplayer.actornumber int number = photonetcwork.localplayer.actornumber

    void Start()
    {
        // Adiciona listeners aos botões
        buttomOff.onClick.AddListener(Playgame);
        buttomCreateLobby.onClick.AddListener(CreateLobby);
        buttomEnterLObby.onClick.AddListener(EnterLobby);


        void Playgame()
        {
            // Verifica se o nome do jogador foi inserido
            if (playerName != null)
            {
                string name = playerName.text;
                PhotonNetwork.NickName = name;
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.ConnectToRegion("sa");
            }
            else
            {
                Debug.Log("Por favor, insira um nome de jogador.");
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao servidor mestre.");
        canvas.SetActive(false); // Esconde o canvas de login após a conexão
        canvas2.SetActive(true); // Mostra o canvas do lobby
    }
    public override void OnConnected() //ele entra aqui na segunda faze
    {
        Debug.Log("connected");
        Debug.Log($"servidor:{PhotonNetwork.CloudRegion} ping {PhotonNetwork.GetPing()}");
    }

    public void CreateLobby()
    {
        // Verifica se o nome do lobby foi inserido
        if (!string.IsNullOrEmpty(enterLobby.text))
        {
            string lobbyName = enterLobby.text;
            PhotonNetwork.CreateRoom(lobbyName, new RoomOptions { MaxPlayers = 4 });
            canvas.SetActive(false);
        }
        else
        {
            Debug.Log("Por favor, insira um nome para o lobby.");
        }
    }

    public void EnterLobby()
    {
        PhotonNetwork.JoinRandomRoom();
        canvas2.SetActive(false);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entrou no lobby.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala.");
        Debug.Log($"Nome da sala: {PhotonNetwork.CurrentRoom.Name}, número de jogadores: {PhotonNetwork.CurrentRoom.PlayerCount}");
        canvas2.SetActive(false);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            // Se já houver dois jogadores na sala, não instanciar mais jogadores
            if (PhotonNetwork.CurrentRoom.PlayerCount > 2)
            {
                Debug.LogWarning("Sala cheia. Não é possível instanciar mais jogadores.");
                return;
            }

            // Calcula a posição de instância do jogador com base no número de jogadores na sala
            Vector3 spawnPosition = new Vector3(2f * PhotonNetwork.CurrentRoom.PlayerCount, 0f, 0f);

            // Prefab do jogador a ser instanciado
            GameObject playerPrefabToInstantiate = null;

            // Define o prefab do jogador de acordo com o número de jogadores na sala
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                playerPrefabToInstantiate = playerPrefabBlue; // Primeiro jogador: cor azul
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                playerPrefabToInstantiate = playerPrefabPink; // Segundo jogador: cor rosa
                //gameStarted = true;
                //ResumeGame(); // Comece o jogo
            }

            // Instancia um novo jogador na posição calculada
            if (playerPrefabToInstantiate != null)
            {
                PhotonNetwork.Instantiate(playerPrefabToInstantiate.name, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Prefab do jogador não atribuído.");
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Pausar o tempo do jogo
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Retomar o tempo do jogo
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

}

