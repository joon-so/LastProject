using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerCharacterSelectManager : MonoBehaviour
{
    [SerializeField] GameObject mainKarmen;
    [SerializeField] GameObject subKarmen;
    [SerializeField] GameObject mainJade;
    [SerializeField] GameObject subJade;
    [SerializeField] GameObject mainLeina;
    [SerializeField] GameObject subLeina;
    [SerializeField] GameObject mainEva;
    [SerializeField] GameObject subEva;
    [SerializeField] GameObject selectEffectKarmen;
    [SerializeField] GameObject selectEffectJade;
    [SerializeField] GameObject selectEffectLeina;
    [SerializeField] GameObject selectEffectEva;

    [SerializeField] Text characterInfoMsgText;
    private int mainOrSub;

    private int checkOverLap;

    void Start()
    {
        mainOrSub = 1;
    }
    public void OnClickSelectKarmen()
    {
        if (mainOrSub == 1)
        {
            mainKarmen.SetActive(true);
            mainJade.SetActive(false);
            mainLeina.SetActive(false);
            mainEva.SetActive(false);
        }
        else if (mainOrSub == 2)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
                return;
            subKarmen.SetActive(true);
            subJade.SetActive(false);
            subLeina.SetActive(false);
            subEva.SetActive(false);
        }
    }
    public void OnClickSelectJade()
    {
        if (mainOrSub == 1)
        {
            mainKarmen.SetActive(false);
            mainJade.SetActive(true);
            mainLeina.SetActive(false);
            mainEva.SetActive(false);
        }
        else if (mainOrSub == 2)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
                return;
            subKarmen.SetActive(false);
            subJade.SetActive(true);
            subLeina.SetActive(false);
            subEva.SetActive(false);
        }
    }
    public void OnClickSelectLeina()
    {
        if (mainOrSub == 1)
        {
            mainKarmen.SetActive(false);
            mainJade.SetActive(false);
            mainLeina.SetActive(true);
            mainEva.SetActive(false);
        }
        else if (mainOrSub == 2)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
                return;
            subKarmen.SetActive(false);
            subJade.SetActive(false);
            subLeina.SetActive(true);
            subEva.SetActive(false);
        }
    }
    public void OnClickSelectEva()
    {
        if (mainOrSub == 1)
        {
            mainKarmen.SetActive(false);
            mainJade.SetActive(false);
            mainLeina.SetActive(false);
            mainEva.SetActive(true);
        }
        else if (mainOrSub == 2)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
                return;
            subKarmen.SetActive(false);
            subJade.SetActive(false);
            subLeina.SetActive(false);
            subEva.SetActive(true);
        }
    }
    public void OnClickSelectButton()
    {
        if (mainOrSub == 1)
        {
            if (mainKarmen.activeSelf)
            {
                ServerLoginManager.playerList[0].selectMainCharacter = 1;
                ServerLoginManager.playerList[0].character1Hp = 500;
                ServerLoginManager.playerList[0].character1Ep = 100;
                selectEffectKarmen.SetActive(true);
                mainOrSub = 2;
            }
            else if (mainJade.activeSelf)
            {
                ServerLoginManager.playerList[0].selectMainCharacter = 2;
                ServerLoginManager.playerList[0].character1Hp = 400;
                ServerLoginManager.playerList[0].character1Ep = 200;
                selectEffectJade.SetActive(true);
                mainOrSub = 2;
            }
            else if (mainLeina.activeSelf)
            {
                ServerLoginManager.playerList[0].selectMainCharacter = 3;
                ServerLoginManager.playerList[0].character1Hp = 400;
                ServerLoginManager.playerList[0].character1Ep = 200;
                selectEffectLeina.SetActive(true);
                mainOrSub = 2;
            }
            else if (mainEva.activeSelf)
            {
                ServerLoginManager.playerList[0].selectMainCharacter = 4;
                ServerLoginManager.playerList[0].character1Hp = 500;
                ServerLoginManager.playerList[0].character1Ep = 100;
                selectEffectEva.SetActive(true);
                mainOrSub = 2;
            }
        }
        else if (mainOrSub == 2)
        {
            if (subKarmen.activeSelf)
            {
                ServerLoginManager.playerList[0].selectSubCharacter = 1;
                ServerLoginManager.playerList[0].character2Hp = 500;
                ServerLoginManager.playerList[0].character2Ep = 100;
                selectEffectKarmen.SetActive(true);
                mainOrSub = 0;
            }
            else if (subJade.activeSelf)
            {
                ServerLoginManager.playerList[0].selectSubCharacter = 2;
                ServerLoginManager.playerList[0].character2Hp = 400;
                ServerLoginManager.playerList[0].character2Ep = 200;
                selectEffectJade.SetActive(true);
                mainOrSub = 0;
            }
            else if (subLeina.activeSelf)
            {
                ServerLoginManager.playerList[0].selectSubCharacter = 3;
                ServerLoginManager.playerList[0].character2Hp = 400;
                ServerLoginManager.playerList[0].character2Ep = 200;
                selectEffectLeina.SetActive(true);
                mainOrSub = 0;
            }
            else if (subEva.activeSelf)
            {
                ServerLoginManager.playerList[0].selectSubCharacter = 4;
                ServerLoginManager.playerList[0].character2Hp = 500;
                ServerLoginManager.playerList[0].character2Ep = 100;
                selectEffectEva.SetActive(true);
                mainOrSub = 0;
            }
        }
    }
    void send_Login_packet()
    {
        cs_Login LoginPacket = new cs_Login();
        LoginPacket.Player_ID = ServerLoginManager.playerList[0].playerID;
        //메인, 서브 케릭터 설정
        LoginPacket.main_charc = ServerLoginManager.playerList[0].selectMainCharacter;
        LoginPacket.sub_charc = ServerLoginManager.playerList[0].selectSubCharacter;
        NetworkManager.instance.Send(LoginPacket.Write());
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("ServerLobby");
        //로그인 패킷 전송
        send_Login_packet();
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("ServerLogin");
    }

    public void KarmenInfoMsg()
    {
        characterInfoMsgText.text = "카르멘은 근거리 전투 캐릭터입니다.";
    }
    public void JadeInfoMsg()
    {
        characterInfoMsgText.text = "제이드는 원거리 전투 캐릭터입니다.";
    }
    public void LeinaInfoMsg()
    {
        characterInfoMsgText.text = "레이나는 원거리 전투 캐릭터입니다.";
    }
    public void EvaInfoMsg()
    {
        characterInfoMsgText.text = "에바는 근거리 전투 캐릭터입니다.";
    }
}