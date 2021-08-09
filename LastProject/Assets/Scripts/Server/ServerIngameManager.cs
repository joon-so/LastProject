using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerIngameManager : MonoBehaviour
{
    public static ServerIngameManager instance;

    public List<GameObject> player;
    public List<GameObject> otherPlayerList;

    public float playTime;
    public Text curPlayTime;

    public bool timeStart;

    public List<int> playerRankingIndex;
    public int playerCount;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        playerCount = 0;
        send_InGame_Start_packet();
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID != null)
            {
                player[i].SetActive(true);
                playerCount++;
                if (i >= 1)
                    otherPlayerList.Add(player[i]);
            }
            else
            {
                Destroy(player[i]);
            }
        }

        playTime = 180.0f;
    }

    void Update()
    {
        if (timeStart)
        {
            playTime -= Time.deltaTime;
            curPlayTime.text = string.Format("{0:D2}:{1:D2}", ((int)playTime / 60).ToString(), ((int)playTime % 60).ToString());
        }
        if(playTime > 0)
        {
            CheckRanking();
        }
        else
        {
            Time.timeScale = 0;
            InGameResult();
        }
    }

    void CheckRanking()
    {
        for (int i = 0; i < playerCount; ++i)
        {
 
            if(ServerLoginManager.playerList[i].character1Hp <=0 || ServerLoginManager.playerList[i].character2Hp <= 0)
            {
                // playerListIndex에 차례대로 넣기
                playerRankingIndex.Add(i);

                Debug.Log(playerRankingIndex[0]);
            }
        }

    }

    void InGameResult()
    {
        
    }


    void send_InGame_Start_packet()
    {
        cs_InGameStart InGameStartPacket = new cs_InGameStart();
        InGameStartPacket.is_Start = true;

        NetworkManager.instance.Send(InGameStartPacket.Write());
    }
}
