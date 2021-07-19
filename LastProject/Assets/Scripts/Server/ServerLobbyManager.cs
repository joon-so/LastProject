using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerLobbyManager : MonoBehaviour
{
    [SerializeField] GameObject player1Obj;
    [SerializeField] GameObject player2Obj;
    [SerializeField] GameObject player3Obj;
    [SerializeField] GameObject player4Obj;

    [SerializeField] Text player1ID;
    [SerializeField] Text player2ID;
    [SerializeField] Text player3ID;
    [SerializeField] Text player4ID;

    void Update()
    {
        // 플레이어 리스트에 정보가 있다면?
        if(ServerLoginManager.playerList[0].isContainPlayerInfo)
        {
            player1Obj.SetActive(true);
            player1ID.text = ServerLoginManager.playerList[0].playerID;
        }
        if (ServerLoginManager.playerList[1].isContainPlayerInfo)
        {
            player2Obj.SetActive(true);
            player2ID.text = ServerLoginManager.playerList[1].playerID;
        }
        if (ServerLoginManager.playerList[2].isContainPlayerInfo)
        {
            player3Obj.SetActive(true);
            player3ID.text = ServerLoginManager.playerList[2].playerID;
        }
        if (ServerLoginManager.playerList[3].isContainPlayerInfo)
        {
            player4Obj.SetActive(true);
            player4ID.text = ServerLoginManager.playerList[3].playerID;
        }
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("ServerStage");
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("ServerLogin");
    }
}
