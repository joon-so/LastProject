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

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        send_InGame_Start_packet();
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID != null)
            {
                player[i].SetActive(true);
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
        if(timeStart)
        {
            playTime -= Time.deltaTime;
            curPlayTime.text = string.Format("{0:D2}:{1:D2}", ((int)playTime / 60).ToString(), ((int)playTime % 60).ToString());
        }
    }


    void send_InGame_Start_packet()
    {
        cs_InGameStart InGameStartPacket = new cs_InGameStart();
        InGameStartPacket.is_Start = true;

        NetworkManager.instance.Send(InGameStartPacket.Write());
    }
}
