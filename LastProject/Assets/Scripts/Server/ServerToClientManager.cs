using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerToClientManager : ServerIngameManager
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
        SceneManager.LoadScene("Network");
    }


    //Server -> Client
    public void sc_playerPosi_DO(sc_PlayerPosi packet)
    {
        for (int i = 1; i < 4; ++i)
        {
            if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p1_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p1_pos_x, 0, packet.p1_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p1_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p1_behavior;
                //Debug.Log("-------------------------------" + i);
                //Debug.Log(ServerLoginManager.playerList[i].playerID);
                //Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p2_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p2_pos_x, 0, packet.p2_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p2_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p2_behavior;
                //Debug.Log("-------------------------------" + i);
                //Debug.Log(ServerLoginManager.playerList[i].playerID);
                //Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p3_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p3_pos_x, 0, packet.p3_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p3_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p3_behavior;
                //Debug.Log("-------------------------------" + i);
                //Debug.Log(ServerLoginManager.playerList[i].playerID);
                //Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p4_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p4_pos_x, 0, packet.p4_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p4_rot_y, 0);
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p4_behavior;
                //Debug.Log("-------------------------------" + i);
                //Debug.Log(ServerLoginManager.playerList[i].playerID);
                //Debug.Log(ServerLoginManager.playerList[i].mainCharacterPos);
            }
        }


        //for (int i = 0; i < 4; ++i)
        //{
        //    if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p1_ID) == 0)
        //    {
        //        ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p1_pos_x, 0, packet.p1_pos_z);
        //        ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p1_rot_y, 0);
        //        ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p1_behavior;
        //    }
        //    else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p2_ID) == 0)
        //    {
        //        ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p2_pos_x, 0, packet.p2_pos_z);
        //        ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p2_rot_y, 0);
        //        ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p2_behavior;
        //    }
        //    else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p3_ID) == 0)
        //    {
        //        ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p3_pos_x, 0, packet.p3_pos_z);
        //        ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p3_rot_y, 0);
        //        ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p3_behavior;
        //    }
        //    else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p4_ID) == 0)
        //    {
        //        ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p4_pos_x, 0, packet.p4_pos_z);
        //        ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p4_rot_y, 0);
        //        ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p4_behavior;
        //    }
        //}
        // Debug.Log($" {packet.p1_ID} >> {packet.p2_ID} ");
        //string p2_ID = ServerLoginManager.playerList[1].playerID;
        //string p3_ID = Player3.instance.p3_ID;
        //string p4_ID = Player4.instance.p4_ID;

        ////Player2
        ////Debug.Log($"sc_playerPosi_DO ID: {packet.p2_ID} X : {packet.p2_pos_x} Z : {packet.p2_pos_z} ");
        //if (string.Compare(p2_ID, packet.p1_ID) == 0)
        //{
        //    Player2.instance.behavior_var = packet.p1_behavior;
        //    Player2.instance.pos_x = packet.p1_pos_x;
        //    Player2.instance.pos_z = packet.p1_pos_z;
        //    Player2.instance.rot_y = packet.p1_rot_y;
        //}
        //else if (string.Compare(p2_ID, packet.p2_ID) == 0)
        //{
        //    Player2.instance.behavior_var = packet.p2_behavior;
        //    Player2.instance.pos_x = packet.p2_pos_x;
        //    Player2.instance.pos_z = packet.p2_pos_z;
        //    Player2.instance.rot_y = packet.p2_rot_y;
        //}
        //else if (string.Compare(p2_ID, packet.p3_ID) == 0)
        //{
        //    Player2.instance.behavior_var = packet.p3_behavior;
        //    Player2.instance.pos_x = packet.p3_pos_x;
        //    Player2.instance.pos_z = packet.p3_pos_z;
        //    Player2.instance.rot_y = packet.p3_rot_y;
        //}
        //else if (string.Compare(p2_ID, packet.p4_ID) == 0)
        //{
        //    Player2.instance.behavior_var = packet.p4_behavior;
        //    Player2.instance.pos_x = packet.p4_pos_x;
        //    Player2.instance.pos_z = packet.p4_pos_z;
        //    Player2.instance.rot_y = packet.p4_rot_y;
        //}


        ////Player3
        //if (string.Compare(p3_ID, packet.p1_ID) == 0)
        //{
        //    Player3.instance.behavior_var = packet.p1_behavior;
        //    Player3.instance.pos_x = packet.p1_pos_x;
        //    Player3.instance.pos_z = packet.p1_pos_z;
        //    Player3.instance.rot_y = packet.p1_rot_y;

        //}
        //else if (string.Compare(p3_ID, packet.p2_ID) == 0)
        //{
        //    Player3.instance.behavior_var = packet.p2_behavior;
        //    Player3.instance.pos_x = packet.p2_pos_x;
        //    Player3.instance.pos_z = packet.p2_pos_z;
        //    Player3.instance.rot_y = packet.p2_rot_y;
        //}
        //else if (string.Compare(p3_ID, packet.p3_ID) == 0)
        //{
        //    Player3.instance.behavior_var = packet.p3_behavior;
        //    Player3.instance.pos_x = packet.p3_pos_x;
        //    Player3.instance.pos_z = packet.p3_pos_z;
        //    Player3.instance.rot_y = packet.p3_rot_y;
        //}
        //else if (string.Compare(p3_ID, packet.p4_ID) == 0)
        //{
        //    Player3.instance.behavior_var = packet.p4_behavior;
        //    Player3.instance.pos_x = packet.p4_pos_x;
        //    Player3.instance.pos_z = packet.p4_pos_z;
        //    Player3.instance.rot_y = packet.p4_rot_y;
        //}

        ////Player4
        //if (string.Compare(p4_ID, packet.p1_ID) == 0)
        //{
        //    Player4.instance.behavior_var = packet.p1_behavior;
        //    Player4.instance.pos_x = packet.p1_pos_x;
        //    Player4.instance.pos_z = packet.p1_pos_z;
        //    Player4.instance.rot_y = packet.p1_rot_y;
        //}
        //else if (string.Compare(p4_ID, packet.p2_ID) == 0)
        //{
        //    Player4.instance.behavior_var = packet.p2_behavior;
        //    Player4.instance.pos_x = packet.p2_pos_x;
        //    Player4.instance.pos_z = packet.p2_pos_z;
        //    Player4.instance.rot_y = packet.p2_rot_y;
        //}
        //else if (string.Compare(p4_ID, packet.p3_ID) == 0)
        //{
        //    Player4.instance.behavior_var = packet.p3_behavior;
        //    Player4.instance.pos_x = packet.p3_pos_x;
        //    Player4.instance.pos_z = packet.p3_pos_z;
        //    Player4.instance.rot_y = packet.p3_rot_y;
        //}
        //else if (string.Compare(p4_ID, packet.p4_ID) == 0)
        //{
        //    Player4.instance.behavior_var = packet.p4_behavior;
        //    Player4.instance.pos_x = packet.p4_pos_x;
        //    Player4.instance.pos_z = packet.p4_pos_z;
        //    Player4.instance.rot_y = packet.p4_rot_y;
        //}
    }

    public void sc_playerFirstPosi_DO(sc_First_PlayerPosi packet)
    {

    }
}