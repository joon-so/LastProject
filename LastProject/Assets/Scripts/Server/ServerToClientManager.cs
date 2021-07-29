using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerToClientManager : MonoBehaviour
{
    public static ServerToClientManager Instance { get; } = new ServerToClientManager();

    void Start()
    {
    }

    //Client -> Server
    public void cs_Login_Process(cs_Login packet)
    {
        //로그인 접속
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID != packet.Player_ID)
            {
                if (ServerLoginManager.playerList[i].playerID == null)
                {
                    ServerLoginManager.playerList[i].playerID = packet.Player_ID;
                    ServerLoginManager.playerList[i].selectMainCharacter = packet.main_charc;
                    ServerLoginManager.playerList[i].selectSubCharacter = packet.sub_charc;
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
        NetworkManager.instance.Send(movePacket.Write());

        //        NetworkManager.Send(movePacket.Write());
    }

    public void cs_GameStart_Process(cs_GameStart packet)
    {
        SceneManager.LoadScene("ServerStage");
    }


    //Server -> Client
    public void sc_playerPosi_DO(sc_PlayerPosi packet)
    {
        for (int i = 1; i < 4; ++i)
        {
            if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p1_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p1_main_pos_x, 0, packet.p1_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p1_main_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p1_main_behavior;

                //처리해야할 패킷
                /*
                packet.p1_is_main_ch;
                packet.p1_main_hp;
                packet.p1_main_mp;
                packet.p1_sub_behavior;
                packet.p1_sub_pos_x;
                packet.p1_sub_pos_z;
                packet.p1_sub_rot_y;
                packet.p1_sub_hp;
                packet.p1_sub_mp;
                */

                Debug.Log("-------------------------------" + i);
                Debug.Log(ServerLoginManager.playerList[i].playerID);
                Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p2_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p2_main_pos_x, 0, packet.p2_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p2_main_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p2_main_behavior;
                Debug.Log("-------------------------------" + i);
                Debug.Log(ServerLoginManager.playerList[i].playerID);
                Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p3_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p3_main_pos_x, 0, packet.p3_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p3_main_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p3_main_behavior;
                //Debug.Log("-------------------------------" + i);
                //Debug.Log(ServerLoginManager.playerList[i].playerID);
                //Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p4_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p4_main_pos_x, 0, packet.p4_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p4_main_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p4_main_behavior;
                //Debug.Log("-------------------------------" + i);
                //Debug.Log(ServerLoginManager.playerList[i].playerID);
                //Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
        }
    }

    public void sc_playerFirstPosi_DO(sc_First_PlayerPosi packet)
    {

    }
}