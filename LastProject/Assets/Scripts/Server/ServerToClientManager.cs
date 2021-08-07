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

    public void cs_PlayerData_Process(cs_PlayerData packet)
    {
        //Debug.Log($"[From Server]Please type: {packet.GetType()} ID : {packet.ID}  floatx: {packet.player_pos_x} floaty: {packet.player_pos_z} Behavior :{packet.behavior_var}");


        cs_PlayerData movePacket = new cs_PlayerData();
        NetworkManager.instance.Send(movePacket.Write());

    }

    public void cs_GameStart_Process(cs_GameStart packet)
    {
        SceneManager.LoadScene("ServerStage");
    }

    public void cs_Attack_Process(cs_Attack packet)
    {
        //공격 받았을 때 처리
        //쓸일 없음 서버에서만 처리
    }

    public void cs_InGameStart_Process(cs_InGameStart packet)
    {
        //Ingame에 들어왔음을 서버에게 알려줌
    }

    public void cs_ItemActivate_Process(cs_Item packet)
    {
        //Ingame에 들어왔음을 서버에게 알려줌
    }


    //Server -> Client
    public void sc_playerPosi_DO(sc_PlayerPosi packet)
    {
        //Debug.Log("c1-----------------------------------------");
        //Debug.Log(ServerLoginManager.playerList[0].character1Hp);
        //Debug.Log(ServerLoginManager.playerList[0].character1Ep);
        //Debug.Log("c2-----------------------------------------");
        //Debug.Log(ServerLoginManager.playerList[0].character2Hp);
        //Debug.Log(ServerLoginManager.playerList[0].character2Ep);

        for (int i = 1; i < 4; ++i)
        {
            //Debug.Log("c1-----------------------------------------");
            //Debug.Log(ServerLoginManager.playerList[i].character1Hp);
            //Debug.Log(ServerLoginManager.playerList[i].character1Ep);
            //Debug.Log("c2-----------------------------------------");
            //Debug.Log(ServerLoginManager.playerList[i].character2Hp);
            //Debug.Log(ServerLoginManager.playerList[i].character2Ep);
            // 여기문제---------------------------------------------------------------------
            if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p1_ID) == 0)
            {

                if (ServerLoginManager.playerList[i].is_Main_Character == 1)
                {
                    ServerLoginManager.playerList[i].character1Hp = packet.p1_main_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p1_main_mp;
                    ServerLoginManager.playerList[i].character2Hp = packet.p1_sub_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p1_sub_mp;
                }
                else if (ServerLoginManager.playerList[i].is_Main_Character == 2)
                {
                    ServerLoginManager.playerList[i].character2Hp = packet.p1_main_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p1_main_mp;
                    ServerLoginManager.playerList[i].character1Hp = packet.p1_sub_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p1_sub_mp;
                }
                //ServerLoginManager.playerList[i].character1Hp = packet.p1_main_hp;
                //ServerLoginManager.playerList[i].character1Ep = packet.p1_main_mp;
                //ServerLoginManager.playerList[i].character2Hp = packet.p1_sub_hp;
                //ServerLoginManager.playerList[i].character2Ep = packet.p1_sub_mp;
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p2_ID) == 0)
            {
                if (ServerLoginManager.playerList[i].is_Main_Character == 1)
                {
                    ServerLoginManager.playerList[i].character1Hp = packet.p2_main_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p2_main_mp;
                    ServerLoginManager.playerList[i].character2Hp = packet.p2_sub_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p2_sub_mp;
                }
                else if (ServerLoginManager.playerList[i].is_Main_Character == 2)
                {
                    ServerLoginManager.playerList[i].character2Hp = packet.p2_main_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p2_main_mp;
                    ServerLoginManager.playerList[i].character1Hp = packet.p2_sub_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p2_sub_mp;
                }
                //ServerLoginManager.playerList[i].character1Hp = packet.p2_main_hp;
                //ServerLoginManager.playerList[i].character1Ep = packet.p2_main_mp;
                //ServerLoginManager.playerList[i].character2Hp = packet.p2_sub_hp;
                //ServerLoginManager.playerList[i].character2Ep = packet.p2_sub_mp;
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p3_ID) == 0)
            {
                if (ServerLoginManager.playerList[i].is_Main_Character == 1)
                {
                    ServerLoginManager.playerList[i].character1Hp = packet.p3_main_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p3_main_mp;
                    ServerLoginManager.playerList[i].character2Hp = packet.p3_sub_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p3_sub_mp;
                }
                else if (ServerLoginManager.playerList[i].is_Main_Character == 2)
                {
                    ServerLoginManager.playerList[i].character2Hp = packet.p3_main_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p3_main_mp;
                    ServerLoginManager.playerList[i].character1Hp = packet.p3_sub_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p3_sub_mp;
                }
                //ServerLoginManager.playerList[i].character1Hp = packet.p3_main_hp;
                //ServerLoginManager.playerList[i].character1Ep = packet.p3_main_mp;
                //ServerLoginManager.playerList[i].character2Hp = packet.p3_sub_hp;
                //ServerLoginManager.playerList[i].character2Ep = packet.p3_sub_mp;
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p4_ID) == 0)
            {
                if (ServerLoginManager.playerList[i].is_Main_Character == 1)
                {
                    ServerLoginManager.playerList[i].character1Hp = packet.p4_main_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p4_main_mp;
                    ServerLoginManager.playerList[i].character2Hp = packet.p4_sub_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p4_sub_mp;
                }
                else if (ServerLoginManager.playerList[i].is_Main_Character == 2)
                {
                    ServerLoginManager.playerList[i].character2Hp = packet.p4_main_hp;
                    ServerLoginManager.playerList[i].character2Ep = packet.p4_main_mp;
                    ServerLoginManager.playerList[i].character1Hp = packet.p4_sub_hp;
                    ServerLoginManager.playerList[i].character1Ep = packet.p4_sub_mp;
                }
                //ServerLoginManager.playerList[i].character1Hp = packet.p4_main_hp;
                //ServerLoginManager.playerList[i].character1Ep = packet.p4_main_mp;
                //ServerLoginManager.playerList[i].character2Hp = packet.p4_sub_hp;
                //ServerLoginManager.playerList[i].character2Ep = packet.p4_sub_mp;
            }
        }
        //-------------------------------------------------------------------------------

        for (int i = 1; i < 4; ++i)
        {
            if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p1_ID) == 0)
            {
                ServerLoginManager.playerList[i].is_Main_Character = packet.p1_is_main_ch;

                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p1_main_behavior;
                ServerLoginManager.playerList[i].subCharacterBehavior = packet.p1_sub_behavior;

                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p1_main_pos_x, 0, packet.p1_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p1_main_rot_y, 0);

                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p1_sub_pos_x, 0, packet.p1_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p1_sub_rot_y, 0);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p2_ID) == 0)
            {
                ServerLoginManager.playerList[i].is_Main_Character = packet.p2_is_main_ch;

                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p2_main_behavior;
                ServerLoginManager.playerList[i].subCharacterBehavior = packet.p2_sub_behavior;

                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p2_main_pos_x, 0, packet.p2_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p2_main_rot_y, 0);

                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p2_sub_pos_x, 0, packet.p2_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p2_sub_rot_y, 0);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p3_ID) == 0)
            {
                ServerLoginManager.playerList[i].is_Main_Character = packet.p3_is_main_ch;

                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p3_main_behavior;
                ServerLoginManager.playerList[i].subCharacterBehavior = packet.p3_sub_behavior;

                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p3_main_pos_x, 0, packet.p3_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p3_main_rot_y, 0);

                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p3_sub_pos_x, 0, packet.p3_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p3_sub_rot_y, 0);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p4_ID) == 0)
            {
                ServerLoginManager.playerList[i].is_Main_Character = packet.p4_is_main_ch;

                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p4_main_behavior;
                ServerLoginManager.playerList[i].subCharacterBehavior = packet.p4_sub_behavior;

                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p4_main_pos_x, 0, packet.p4_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p4_main_rot_y, 0);

                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p4_sub_pos_x, 0, packet.p4_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p4_sub_rot_y, 0);
            }
        }
    }

    public void sc_playerFirstPosi_DO(sc_First_PlayerPosi packet)
    {
        for (int i = 0; i < 4; ++i)
        {
            if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p1_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p1_main_behavior;
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p1_main_pos_x, 0, packet.p1_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p1_main_rot_y, 0);
                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p1_sub_pos_x, 0, packet.p1_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p1_sub_rot_y, 0);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p2_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p2_main_behavior;
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p2_main_pos_x, 0, packet.p2_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p2_main_rot_y, 0);
                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p2_sub_pos_x, 0, packet.p2_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p2_sub_rot_y, 0);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p3_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p3_main_behavior;
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p3_main_pos_x, 0, packet.p3_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p3_main_rot_y, 0);
                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p3_sub_pos_x, 0, packet.p3_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p3_sub_rot_y, 0);
            }
            else if (string.Compare(ServerLoginManager.playerList[i].playerID, packet.p4_ID) == 0)
            {
                ServerLoginManager.playerList[i].mainCharacterBehavior = packet.p4_main_behavior;
                ServerLoginManager.playerList[i].mainCharacterPos = new Vector3(packet.p4_main_pos_x, 0, packet.p4_main_pos_z);
                ServerLoginManager.playerList[i].mainCharacterRot.eulerAngles = new Vector3(0, packet.p4_main_rot_y, 0);
                ServerLoginManager.playerList[i].subCharacterPos = new Vector3(packet.p4_sub_pos_x, 0, packet.p4_sub_pos_z);
                ServerLoginManager.playerList[i].subCharacterRot.eulerAngles = new Vector3(0, packet.p4_sub_rot_y, 0);
            }
        }
    }

    public void sc_ItemActivate_Process(sc_Item packet)
    {
        //Ingame에 들어왔음을 서버에게 알려줌
    }
}