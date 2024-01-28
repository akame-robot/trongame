using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject inputField, gameName, text,buttom;
    public InputField playerName;

    void Start()
    {
        string randomName = "player" + Random.Range(0,1000);
        playerName.text = randomName;

        Login();
    }

    public void Playgame()
    {
        if(playerName != null)
        {
            inputField.SetActive(false);
            gameName.SetActive(false);
            text.SetActive(false);
            buttom.SetActive(false);
        }
        
    }

    public void Login()
    {

    }
}
