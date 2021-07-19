using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerCharacterSelectManager : MonoBehaviour
{
    [SerializeField] Text ShowIPAdress;
    [SerializeField] Text characterInfoMsgText;
    private bool mainOrSub;

    private bool isMainKarmen;
    private bool isMainJade;
    private bool isMainLeina;
    private bool isMainEva;
            
    private bool isSubKarmen;
    private bool isSubJade;
    private bool isSubLeina;
    private bool isSubEva;

    private int playerNum;

    NetworkManager _network;

    void Start()
    {
        playerNum = ServerLoginManager.curPlayerNum;
        //playerNum = 0;
        ShowIPAdress.text = ServerLoginManager.playerList[playerNum].ipAdress;
        mainOrSub = true;
        _network = GameObject.Find("Network").GetComponent<NetworkManager>();
    }
    public void OnClickSelectKarmen()
    {
        if (mainOrSub)
        {
            isMainKarmen = true;
            isMainJade = false;
            isMainLeina = false;
            isMainEva = false;
        }
        else
        {
            isSubKarmen = true;
            isSubJade = false;
            isSubLeina = false;
            isSubEva = false;
        }
    }
    public void OnClickSelectJade()
    {
        if (mainOrSub)
        {
            isMainKarmen = false;
            isMainJade = true;
            isMainLeina = false;
            isMainEva = false;
        }
        else
        {
            isSubKarmen = false;
            isSubJade = true;
            isSubLeina = false;
            isSubEva = false;
        }
    }
    public void OnClickSelectLeina()
    {
        if (mainOrSub)
        {
            isMainKarmen = false;
            isMainJade = false;
            isMainLeina = true;
            isMainEva = false;
        }
        else
        {
            isSubKarmen = false;
            isSubJade = false;
            isSubLeina = true;
            isSubEva = false;
        }
    }
    public void OnClickSelectEva()
    {
        if (mainOrSub)
        {
            isMainKarmen = false;
            isMainJade = false;
            isMainLeina = false;
            isMainEva = true;
        }
        else
        {
            isSubKarmen = false;
            isSubJade = false;
            isSubLeina = false;
            isSubEva = true;
        }
    }
    public void OnClickSelectButton()
    {
        if (mainOrSub)
        {
            if (isMainKarmen)
                ServerLoginManager.playerList[0].selectMainCharacter = 1;

            if (isMainJade)
                ServerLoginManager.playerList[0].selectMainCharacter = 2;

            if (isMainLeina)
                ServerLoginManager.playerList[0].selectMainCharacter = 3;

            if (isMainEva)
                ServerLoginManager.playerList[0].selectMainCharacter = 4;
        }
        else
        {
            if (isSubKarmen)
                ServerLoginManager.playerList[0].selectSubCharacter = 1;

            if (isSubJade)
                ServerLoginManager.playerList[0].selectSubCharacter = 2;

            if (isSubLeina)
                ServerLoginManager.playerList[0].selectSubCharacter = 3;

            if (isSubEva)
                ServerLoginManager.playerList[0].selectSubCharacter = 4;
        }
        mainOrSub = false;
    }
    void send_Login_packet()
    {
        cs_Login LoginPacket = new cs_Login();
        LoginPacket.Player_ID = ServerLoginManager.playerList[0].playerID;
        //����, ���� �ɸ��� ����
        LoginPacket.main_charc = ServerLoginManager.playerList[0].selectMainCharacter;
        LoginPacket.sub_charc = ServerLoginManager.playerList[0].selectSubCharacter;

        _network.Send(LoginPacket.Write());
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("ServerLobby");
        //�α��� ��Ŷ ����
        send_Login_packet();
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("ServerLogin");
    }

    public void KarmenInfoMsg()
    {
        characterInfoMsgText.text = "ī������ �ٰŸ� ���� ĳ�����Դϴ�.";
    }
    public void JadeInfoMsg()
    {
        characterInfoMsgText.text = "���̵�� ���Ÿ� ���� ĳ�����Դϴ�.";
    }
    public void LeinaInfoMsg()
    {
        characterInfoMsgText.text = "���̳��� ���Ÿ� ���� ĳ�����Դϴ�.";
    }
    public void EvaInfoMsg()
    {
        characterInfoMsgText.text = "���ٴ� �ٰŸ� ���� ĳ�����Դϴ�.";
    }
}