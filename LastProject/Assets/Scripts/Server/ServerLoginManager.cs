using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerLoginManager : MonoBehaviour
{
    public struct ServerPlayer
    {
        public string ipAdress;
        public string playerID;

        // Karmen : 1 Eva : 4
        public int selectMainCharacter;
        public int selectSubCharacter;

        public bool isContainPlayerInfo;
    }

    public static ServerPlayer[] playerList = new ServerPlayer[4];
    public static int curPlayerNum;

    private void Start()
    {
        curPlayerNum = 0;
    }

    public void GetIPAdress(InputField ip)
    {
        if (ip.text == "0")
        {
            ip.text = "127.0.0.1";
        }
        playerList[0].ipAdress = ip.text;
    }

    public void GetPlayerID(InputField id)
    {
        playerList[0].playerID = id.text;
    }

    public void ClickJoin()
    {
        playerList[0].isContainPlayerInfo = true;
        SceneManager.LoadScene("ServerCharacterSelect");

    }

    public void ClickExit()
    {
        //SceneManager.LoadScene("Main");
    }
}