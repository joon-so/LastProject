using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerManager : MonoBehaviour
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    NetworkManager _network;


    public static ServerPlayerManager Instance { get; } = new ServerPlayerManager();

    void Start()
    {
        _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    //Client -> Server
    public void cs_Login_Process(cs_Login packet)
    {
        //로그인 접속
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID == packet.Player_ID)
            {
                Debug.Log(ServerLoginManager.playerList[i].playerID);
                Debug.Log("-----------------------------------------");
                Debug.Log(packet.Player_ID);
            }

            //// 안되는이유 0과  이미 다른 ID도 똑같이 취급 시발
            else if (ServerLoginManager.playerList[i].playerID != packet.Player_ID)
            {
                if (ServerLoginManager.playerList[i].isContainPlayerInfo == false)
                {
                    Debug.Log("새로운 배열에 추가");
                    Debug.Log(ServerLoginManager.playerList[i].playerID);
                    Debug.Log("-----------------------------------------");
                    Debug.Log(packet.Player_ID);
                    ServerLoginManager.playerList[i].playerID = packet.Player_ID;
                    ServerLoginManager.playerList[i].selectMainCharacter = packet.main_charc;
                    ServerLoginManager.playerList[i].selectSubCharacter = packet.sub_charc;
                    ServerLoginManager.playerList[i].isContainPlayerInfo = true;
                    break;
                }
            }
        }
    }
    public void please(cs_PlayerData packet)
    {
        //Debug.Log($"[From Server]Please type: {packet.GetType()} ID : {packet.ID}  floatx: {packet.player_pos_x} floaty: {packet.player_pos_z} Behavior :{packet.behavior_var}");


        cs_PlayerData movePacket = new cs_PlayerData();
        //movePacket.inta = 012;
        //movePacket.floatx = 1234.24f;
        //movePacket.floaty = 771234.24f;
        //movePacket.idtemp = "dnk97";

        _network.Send(movePacket.Write());
    }


    //Server -> Client
    public void sc_playerPosi_DO(sc_PlayerPosi packet)
    {
       //// Debug.Log($" {packet.p1_ID} >> {packet.p2_ID} ");
       // string p2_ID = Player2.instance.p2_ID;
       // string p3_ID = Player3.instance.p3_ID;
       // string p4_ID = Player4.instance.p4_ID;

       // //Player2
       // //Debug.Log($"sc_playerPosi_DO ID: {packet.p2_ID} X : {packet.p2_pos_x} Z : {packet.p2_pos_z} ");
       // if (string.Compare(p2_ID, packet.p1_ID) == 0)
       // {
       //     Player2.instance.behavior_var = packet.p1_behavior;
       //     Player2.instance.pos_x = packet.p1_pos_x;
       //     Player2.instance.pos_z = packet.p1_pos_z;
       //     Player2.instance.rot_y = packet.p1_rot_y;
       // }
       // else if (string.Compare(p2_ID, packet.p2_ID) == 0)
       // {
       //     Player2.instance.behavior_var = packet.p2_behavior;
       //     Player2.instance.pos_x = packet.p2_pos_x;
       //     Player2.instance.pos_z = packet.p2_pos_z;
       //     Player2.instance.rot_y = packet.p2_rot_y;
       // }
       // else if (string.Compare(p2_ID, packet.p3_ID) == 0)
       // {
       //     Player2.instance.behavior_var = packet.p3_behavior;
       //     Player2.instance.pos_x = packet.p3_pos_x;
       //     Player2.instance.pos_z = packet.p3_pos_z;
       //     Player2.instance.rot_y = packet.p3_rot_y;
       // }
       // else if (string.Compare(p2_ID, packet.p4_ID) == 0)
       // {
       //     Player2.instance.behavior_var = packet.p4_behavior;
       //     Player2.instance.pos_x = packet.p4_pos_x;
       //     Player2.instance.pos_z = packet.p4_pos_z;
       //     Player2.instance.rot_y = packet.p4_rot_y;
       // }


       // //Player3
       // if (string.Compare(p3_ID, packet.p1_ID) == 0)
       // {
       //     Player3.instance.behavior_var = packet.p1_behavior;
       //     Player3.instance.pos_x = packet.p1_pos_x;
       //     Player3.instance.pos_z = packet.p1_pos_z;
       //     Player3.instance.rot_y = packet.p1_rot_y;

       // }
       // else if (string.Compare(p3_ID, packet.p2_ID) == 0)
       // {
       //     Player3.instance.behavior_var = packet.p2_behavior;
       //     Player3.instance.pos_x = packet.p2_pos_x;
       //     Player3.instance.pos_z = packet.p2_pos_z;
       //     Player3.instance.rot_y = packet.p2_rot_y;
       // }
       // else if (string.Compare(p3_ID, packet.p3_ID) == 0)
       // {
       //     Player3.instance.behavior_var = packet.p3_behavior;
       //     Player3.instance.pos_x = packet.p3_pos_x;
       //     Player3.instance.pos_z = packet.p3_pos_z;
       //     Player3.instance.rot_y = packet.p3_rot_y;
       // }
       // else if (string.Compare(p3_ID, packet.p4_ID) == 0)
       // {
       //     Player3.instance.behavior_var = packet.p4_behavior;
       //     Player3.instance.pos_x = packet.p4_pos_x;
       //     Player3.instance.pos_z = packet.p4_pos_z;
       //     Player3.instance.rot_y = packet.p4_rot_y;
       // }

       // //Player4
       // if (string.Compare(p4_ID, packet.p1_ID) == 0)
       // {
       //     Player4.instance.behavior_var = packet.p1_behavior;
       //     Player4.instance.pos_x = packet.p1_pos_x;
       //     Player4.instance.pos_z = packet.p1_pos_z;
       //     Player4.instance.rot_y = packet.p1_rot_y;
       // }
       // else if (string.Compare(p4_ID, packet.p2_ID) == 0)
       // {
       //     Player4.instance.behavior_var = packet.p2_behavior;
       //     Player4.instance.pos_x = packet.p2_pos_x;
       //     Player4.instance.pos_z = packet.p2_pos_z;
       //     Player4.instance.rot_y = packet.p2_rot_y;
       // }
       // else if (string.Compare(p4_ID, packet.p3_ID) == 0)
       // {
       //     Player4.instance.behavior_var = packet.p3_behavior;
       //     Player4.instance.pos_x = packet.p3_pos_x;
       //     Player4.instance.pos_z = packet.p3_pos_z;
       //     Player4.instance.rot_y = packet.p3_rot_y;
       // }
       // else if (string.Compare(p4_ID, packet.p4_ID) == 0)
       // {
       //     Player4.instance.behavior_var = packet.p4_behavior;
       //     Player4.instance.pos_x = packet.p4_pos_x;
       //     Player4.instance.pos_z = packet.p4_pos_z;
       //     Player4.instance.rot_y = packet.p4_rot_y;
       // }
    }

}
