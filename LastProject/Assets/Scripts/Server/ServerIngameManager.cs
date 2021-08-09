using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRankInfo
{ 
    public string playerID { get; set; }
    public short hp { get; set; }
    public int character1 { get; set; }
    public int character2 { get; set; }

}



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

    [SerializeField] GameObject resultUI;

    List<PlayerRankInfo> diePlayerInfo = new List<PlayerRankInfo>();
    List<PlayerRankInfo> endPlayerInfo = new List<PlayerRankInfo>();

    public List<PlayerRankInfo> resultPlayerInfo = new List<PlayerRankInfo>();

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
            InGameResult();
            Time.timeScale = 0;
        }
    }

    void CheckRanking()
    {
        for (int i = 0; i < playerCount; ++i)
        {
            if (ServerLoginManager.playerList[i].character1Hp <= 0 || ServerLoginManager.playerList[i].character2Hp <= 0)
            {
                // 플레이 타임 때 playerListIndex에 차례대로 넣기
                playerRankingIndex.Add(i);
                diePlayerInfo.Add(new PlayerRankInfo() { playerID = ServerLoginManager.playerList[i].playerID, hp = 0, 
                    character1 = ServerLoginManager.playerList[i].selectMainCharacter, character2 = ServerLoginManager.playerList[i].selectSubCharacter });
            }
        }
    }

    void InGameResult()
    {
        for (int i = 0; i < playerCount; ++i)
        {
            if (ServerLoginManager.playerList[i].character1Hp > 0 && ServerLoginManager.playerList[i].character2Hp > 0)
            {
                if (ServerLoginManager.playerList[i].is_Main_Character == 1)
                {
                    endPlayerInfo.Add(new PlayerRankInfo() { playerID = ServerLoginManager.playerList[i].playerID, hp = ServerLoginManager.playerList[i].character1Hp,
                        character1 = ServerLoginManager.playerList[i].selectMainCharacter, character2 = ServerLoginManager.playerList[i].selectSubCharacter });
                }
                else if (ServerLoginManager.playerList[i].is_Main_Character == 2)
                {
                    endPlayerInfo.Add(new PlayerRankInfo() { playerID = ServerLoginManager.playerList[i].playerID, hp = ServerLoginManager.playerList[i].character2Hp,
                        character1 = ServerLoginManager.playerList[i].selectMainCharacter, character2 = ServerLoginManager.playerList[i].selectSubCharacter });
                }
            }
        }
        endPlayerInfo.Sort((player1, player2) => player1.hp.CompareTo(player2.hp));

        diePlayerInfo.Reverse();
        endPlayerInfo.Reverse();

        resultPlayerInfo.AddRange(endPlayerInfo);
        resultPlayerInfo.AddRange(diePlayerInfo);

        resultUI.SetActive(true);
    }


    void send_InGame_Start_packet()
    {
        cs_InGameStart InGameStartPacket = new cs_InGameStart();
        InGameStartPacket.is_Start = true;

        NetworkManager.instance.Send(InGameStartPacket.Write());
    }
}