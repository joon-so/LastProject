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

        public short is_Main_Character;

        public short character1Hp;
        public short character2Hp;

        public short character1Ep;
        public short character2Ep;

        // Karmen : 1 Eva : 4
        public int selectMainCharacter;
        public int selectSubCharacter;
        public int mainCharacterBehavior;
        public int subCharacterBehavior;

        public Vector3 mainCharacterPos;
        public Vector3 subCharacterPos;
        public Quaternion mainCharacterRot;
        public Quaternion subCharacterRot;
    }

    public static ServerPlayer[] playerList = new ServerPlayer[4];
    public InputField inputID;

    private void Start()
    {
        playerList[0].playerID = LoginManager.playerID;
        inputID.text = LoginManager.playerID;
    }

    public void GetIPAdress(InputField ip)
    {
        if (ip.text == "0")
        {
            ip.text = "127.0.0.1";
        }
        playerList[0].ipAdress = ip.text;
    }

    public void ClickJoin()
    {
        SceneManager.LoadScene("ServerCharacterSelect");
    }

    public void ClickExit()
    {
        SceneManager.LoadScene("Main");
    }
}