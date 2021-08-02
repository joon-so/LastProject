using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerIngameManager : MonoBehaviour
{
    public List<GameObject> player;


    void Start()
    {
        send_InGame_Start_packet();
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID != null)
            {
                player[i].SetActive(true);
            }
            else
            {
                Destroy(player[i]);
            }
        }
    }

    void send_InGame_Start_packet()
    {
        cs_InGameStart InGameStartPacket = new cs_InGameStart();
        InGameStartPacket.is_Start = true;

        NetworkManager.instance.Send(InGameStartPacket.Write());
    }

}
