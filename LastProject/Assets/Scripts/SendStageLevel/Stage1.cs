using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.Threading;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;


public class Stage1 : MonoBehaviour
{
    bool once = false;
    byte[] buffer = new byte[1024]; // 버퍼의 크기 필요한 만큼 크기를 정하도록 하자 작을수록 좋음
    Socket socket;

    void serverOn()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        socket.Connect(IPAddress.Parse(DataBaseManager.playerIP), 9080);
    }

    public const int BIND_SIZE_ChangeStagePacket = 4 + 20 + 4;
    static byte[] GetBytes_Bind_ChangeStagePacket(ChangeStagePacket packet)
    {
        byte[] btBuffer = new byte[BIND_SIZE_ChangeStagePacket];

        MemoryStream ms = new MemoryStream(btBuffer, true);
        BinaryWriter bw = new BinaryWriter(ms);


        // Grade - long
        bw.Write(IPAddress.HostToNetworkOrder(packet.type));

        // Name - string
        try
        {
            byte[] btName = new byte[20];
            Encoding.UTF8.GetBytes(packet.id, 0, packet.id.Length, btName, 0);
            bw.Write(btName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error : {0}", ex.Message.ToString());
        }

        bw.Write(IPAddress.HostToNetworkOrder(packet.stage));

        bw.Close();
        ms.Close();

        return btBuffer;
    }

    IEnumerator changeStageDataCo()
    {

        ChangeStagePacket changestagepacket = new ChangeStagePacket();
        changestagepacket.type = 103;
        ///////////////////////////////////바꿀 해당 ID
        changestagepacket.id = DataBaseManager.playerID;
        //////////////////////////////////바꿀 Stage 변수
        changestagepacket.stage = 1;

        //서버 연결
        serverOn();
        byte[] buffer = GetBytes_Bind_ChangeStagePacket(changestagepacket);
        int sign = socket.Send(buffer, buffer.Length, SocketFlags.None);
        Debug.Log("데이터 전송 완료");
        Debug.Log(sign);

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //여기서 보냄
        if(!once)
        {
            once = true;
            StartCoroutine(changeStageDataCo());
        }
    }
}
