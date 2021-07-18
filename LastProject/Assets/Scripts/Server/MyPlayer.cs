using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyPlayer : Player
{
    NetworkManager _network;
    public static MyPlayer instance;
    //플레이어의 ID
    public string My_ID;

    public int curAnimation;


    //bool is_send_Login_Packet = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    void Start()
    {
        My_ID = ServerLoginManager.playerList[0].playerID;
        curAnimation = 0;
        _network = GameObject.Find("Network").GetComponent<NetworkManager>();
        StartCoroutine("CoSendPacket");
    }

    void Update()
    {

    }

    //void send_Login_packet()
    //{
    //    cs_Login LoginPacket = new cs_Login();
    //    LoginPacket.Player_ID = My_ID;
        
    //    _network.Send(LoginPacket.Write());
    //}
	IEnumerator CoSendPacket()
	{
     while (true)
     {
            yield return new WaitForSeconds(0.01f);
            //if(is_send_Login_Packet == false)
            //{
            //    send_Login_packet();
            //    is_send_Login_Packet = true;
            //}
            Vector3 pos;
            pos = this.gameObject.transform.position;
            Quaternion rot;
            rot = this.gameObject.transform.rotation;


            cs_PlayerData movePacket = new cs_PlayerData();
            movePacket.ID = My_ID;
            //행동 변수
            movePacket.behavior_var = curAnimation;
            movePacket.player_pos_x = pos.x;
            movePacket.player_pos_z = pos.z;
            movePacket.player_rot_y = rot.eulerAngles.y;

            _network.Send(movePacket.Write());

            //Debug.Log(pos.x);
            //Debug.Log(pos.z);

        }
    }
}
