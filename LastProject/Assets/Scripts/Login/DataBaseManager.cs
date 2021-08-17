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
/*
아이디 비밀번호는 10자리 이하로 설정하기
*/
//client -> server
//type 101 = 로그인 패킷, 102 = 계정 생성, 103 = stage 바꾸기

//server -> client
//type 201 = 로그인 결과 패킷

[Serializable]
struct LoginPacket
{
    public int type;
    public string id;
    public string pw;
};

[Serializable]
struct ChangeStagePacket
{
    public int type;
    public string id;
    public int stage;
};


[Serializable]
struct ResultPacket
{
    public int type;
    public int result;
    public int stageData;
}

public class DataBaseManager : MonoBehaviour
{
    [Header("LoginPanel")]
    public InputField IDInputField;
    public InputField PWInputField;
    [Header("CreateAccountPanel")]
    public InputField New_IDInputField;
    public InputField New_PWInputField;

    [SerializeField] Text message;

    // 플레이어 ID, PW 정보 들어있는 곳
    public static string playerID;
    public static string playerPW;
    public static string playerIP;
    public static int PlayerPvELevel;

    public AudioClip uiButtonSound;

    byte[] buffer = new byte[1024]; // 버퍼의 크기 필요한 만큼 크기를 정하도록 하자 작을수록 좋음
    Socket socket;

    void Start()
    {
        PlayerPvELevel = 0;
    }

    void serverOn()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        socket.Connect(IPAddress.Parse(playerIP), 9080);
    }

    public void OnEndEditPlayerID(InputField inputField)
    {
        playerID = inputField.text;
    }

    public void OnEndEditPlayerPassWord(InputField inputField)
    {
        playerPW = inputField.text;
    }

    public void OnEndEditPlayerIP(InputField inputField)
    {
        if (inputField.text == "0")
        {
            inputField.text = "127.0.0.1";
        }

        playerIP = inputField.text;
    }

    public void LoginBtn()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("LoadMainScene", 1f);

    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
       // StartCoroutine(LoginCo());
    }

    public void applyAccountBtn()
    {
        StartCoroutine(createAccountCo());
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        //StartCoroutine(changeStageDataCo());
    }


    //스테이지 변경 패킷 구조체를 바이트 배열로 변환하는 함수
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

    public static T ByteToStruct<T>(byte[] buffer) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));

        Marshal.FreeHGlobal(ptr);
        return obj;
    }
    IEnumerator LoginCo()
    {
        Debug.Log(IDInputField.text);
        Debug.Log(PWInputField.text);

        LoginPacket loginpacket = new LoginPacket();
        loginpacket.type = 101;
        loginpacket.id = IDInputField.text;
        loginpacket.pw = PWInputField.text;

        //서버 연결
        serverOn();
        byte[] buffer = GetBytes_Bind_LoginPacket(loginpacket);
        int sign = socket.Send(buffer, buffer.Length, SocketFlags.None);
        Debug.Log("데이터 전송 완료");
        Debug.Log(sign);

        byte[] ret = new byte[1024];
        sign = socket.Receive(ret, 1024, SocketFlags.None);

        Debug.Log("데이터 수신 완료");
        Debug.Log(sign);
        Debug.Log("\n");

        ResultPacket Rp = new ResultPacket();

        Rp = ByteToStruct<ResultPacket>(ret);

        if (Rp.type == 201)
        {
            if (Rp.result == 1)
            {
                message.text = "해당 ID 존재하지 않음";
            }
            else if (Rp.result == 2)
            {
                message.text = "아이디는 존재하지만 비밀번호 틀림";
            }
            else if (Rp.result == 3)
            {
                message.text = "로그인 완료!";
                SceneManager.LoadScene("Main");
                playerID = IDInputField.text;
                PlayerPvELevel = Rp.stageData;
            }
        }
        else
            Debug.Log("패킷 타입 에러!!");


        yield return null;
    }
    IEnumerator createAccountCo()
    {
        Debug.Log(New_IDInputField.text);
        Debug.Log(New_PWInputField.text);

        LoginPacket loginpacket = new LoginPacket();
        loginpacket.type = 101;
        loginpacket.id = New_IDInputField.text;
        loginpacket.pw = New_PWInputField.text;

        //서버 연결
        serverOn();
        byte[] buffer = GetBytes_Bind_LoginPacket(loginpacket);
        int sign = socket.Send(buffer, buffer.Length, SocketFlags.None);
        Debug.Log("데이터 전송 완료");
        Debug.Log(sign);

        byte[] ret = new byte[1024];
        sign = socket.Receive(ret, 1024, SocketFlags.None);

        Debug.Log("데이터 수신 완료");
        Debug.Log(sign);
        Debug.Log("\n");

        ResultPacket Rp = new ResultPacket();

        Rp = ByteToStruct<ResultPacket>(ret);

        if (Rp.type == 201)
        {
            if (Rp.result == 1)
            {
                message.text = "계정 생성 완료! 로그인 하세요.";
                Debug.Log("계정 생성 가능");
                LoginPacket newAccountPacket = new LoginPacket();
                newAccountPacket.type = 102;
                newAccountPacket.id = New_IDInputField.text;
                newAccountPacket.pw = New_PWInputField.text;

                serverOn();
                byte[] buf = GetBytes_Bind_LoginPacket(newAccountPacket);
                int sendnum = socket.Send(buf, buf.Length, SocketFlags.None);
                Debug.Log("데이터 전송 완료");
                Debug.Log(sendnum);
            }
            else if (Rp.result == 2 || Rp.result == 3)
            {
                message.text = "이미 존재하는 계정입니다. 새로운 ID를 입력하세요";
            }
        }
        else
            Debug.Log("패킷 타입 에러!!");


        yield return null;
    }
    IEnumerator changeStageDataCo()
    {

        ChangeStagePacket changestagepacket = new ChangeStagePacket();
        changestagepacket.type = 103;
        ///////////////////////////////////바꿀 해당 ID
        changestagepacket.id = "asdf";
        //////////////////////////////////바꿀 Stage 변수
        changestagepacket.stage = 123;

        //서버 연결
        serverOn();
        byte[] buffer = GetBytes_Bind_ChangeStagePacket(changestagepacket);
        int sign = socket.Send(buffer, buffer.Length, SocketFlags.None);
        Debug.Log("데이터 전송 완료");
        Debug.Log(sign);

        yield return null;
    }

    //로그인 패킷 구조체를 바이트 배열로 변환하는 함수
    public const int BIND_SIZE_LoginPacket = 4 + 20 + 20;
    static byte[] GetBytes_Bind_LoginPacket(LoginPacket packet)
    {
        byte[] btBuffer = new byte[BIND_SIZE_LoginPacket];

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

        // Subject - string
        try
        {
            byte[] btName = new byte[20];
            Encoding.UTF8.GetBytes(packet.pw, 0, packet.pw.Length, btName, 0);
            bw.Write(btName);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error : {0}", ex.Message.ToString());
        }

        bw.Close();
        ms.Close();

        return btBuffer;
    }
}