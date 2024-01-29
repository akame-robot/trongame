using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionScript : MonoBehaviour
{
    public GameObject buttoMenu;
    private string playerNameHere;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        Menu menuScrip = GetComponent<Menu>();
        InputField playerName = menuScrip.playerName;
        playerNameHere = playerName.text;
        if (buttoMenu.activeSelf == false)
        {
            canvas.SetActive(true);
            Login();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("sa");
        PhotonNetwork.NickName = playerNameHere;
    }
}
