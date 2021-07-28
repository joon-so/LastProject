using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerLobbyManager : MonoBehaviour
{
    public List<GameObject> playerObj;
    public List<Text> playerTextID;
    NetworkManager _network;

    void Start()
    {
        _network = GameObject.Find("Network").GetComponent<NetworkManager>();
    }

    void Update()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID != null)
            {
                playerObj[i].SetActive(true);
                playerTextID[i].text = ServerLoginManager.playerList[i].playerID;
            }
        }
    }

    void send_Game_Start_packet()
    {
        cs_GameStart GameStartPacket = new cs_GameStart();
        GameStartPacket.is_Start = true;

        _network.Send(GameStartPacket.Write());
    }

    public void OnClickStartButton()
    {
        send_Game_Start_packet();
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("ServerLogin");
    }
}
