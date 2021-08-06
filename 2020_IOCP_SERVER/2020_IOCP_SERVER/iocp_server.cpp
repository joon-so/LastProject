//이게졸작
#include <iostream>
#include <WS2tcpip.h>
#include <MSWSock.h>
#include <thread>
#include <vector>
#include <mutex>
#include <unordered_set>
#include <chrono>
#include <queue>
#include <sqlext.h>
#include <windows.h>
#include <stdio.h> 
#include "Default.h"
#include "protocol.h"
using namespace std;
using namespace chrono;

#pragma comment(lib, "Ws2_32.lib")
#pragma comment(lib, "MSWSock.lib")
//#pragma comment(lib, "lua54.lib")
#pragma comment(lib, "odbc32.lib")

constexpr int MAX_BUFFER = 4096;

constexpr char OP_MODE_RECV = 0;
constexpr char OP_MODE_SEND = 1;
constexpr char OP_MODE_ACCEPT = 2;
constexpr char OP_SEND_MOVE = 3;

constexpr int  KEY_SERVER = 1000000;

bool first_operate = false;

struct OVER_EX {
    WSAOVERLAPPED wsa_over;
    char   op_mode;
    WSABUF   wsa_buf;
    unsigned char iocp_buf[MAX_BUFFER];
    int		object_id;
};

struct Player_Attribute {
    bool Set_Name = false;
    char ID[20] = "";

    short is_Main_CH = 0;

    //메인 캐릭터
    int Main_CH = 0;    //캐릭터 종류
    short Main_HP = 0;
    short Main_MP = 0;
    int Main_Behavior = 0;  //캐릭터 애니메이션
    float Main_X = 0;
    float Main_Z = 0;
    float Main_Rot_Y = 0;

    //서브 캐릭터
    int Sub_CH = 0;    //캐릭터 종류
    short Sub_HP = 0;
    short Sub_MP = 0;
    int Sub_Behavior = 0;  //캐릭터 애니메이션
    float Sub_X = 0;
    float Sub_Z = 0;
    float Sub_Rot_Y = 0;
};

struct Player_data {
    int playerNum = 0;

    Player_Attribute player1;
    Player_Attribute player2;
    Player_Attribute player3;
    Player_Attribute player4;
};

Player_data playerdata;

struct client_info {
    mutex c_lock;
    int id;

    bool login_set;
    char name[MAX_ID_LEN];
    short first_x, first_y;
    short x, y;

    bool move_1s_time;
    bool attack_1s_time;
    bool hp_recov_5s_time;
    bool recovery = false;

    mutex lua_lock;

    bool in_use;
    atomic_bool is_active;
    SOCKET   m_sock;
    OVER_EX   m_recv_over;
    unsigned char* m_packet_start;
    unsigned char* m_recv_start;

    mutex vl;

};

mutex id_lock;

client_info g_clients[MAX_USER];

HANDLE      h_iocp;

SOCKET g_lSocket;
OVER_EX g_accept_over;

struct event_type {
    int obj_id;
    system_clock::time_point wakeup_time;
    int event_id;
    int target_id;
    constexpr bool operator < (const event_type& _Left) const
    {
        return (wakeup_time > _Left.wakeup_time);
    }
};

priority_queue<event_type> timer_queue;

mutex timer_l;

vector<cs_Login> login_vec;

void disconnect_client(int id);


void error_display(const char* msg, int err_no)
{
    WCHAR* lpMsgBuf;
    FormatMessage(
        FORMAT_MESSAGE_ALLOCATE_BUFFER |
        FORMAT_MESSAGE_FROM_SYSTEM,
        NULL, err_no,
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (LPTSTR)&lpMsgBuf, 0, NULL);
    std::cout << msg;
    std::wcout << L"에러 " << lpMsgBuf << std::endl;
    while (true);
    LocalFree(lpMsgBuf);
}

//타이머 관련 함수
void add_timer(int obj_id, int ev_type, system_clock::time_point t, int target_id)
{
    timer_l.lock();
    event_type ev{ obj_id,t,ev_type,target_id };
    timer_queue.push(ev);
    timer_l.unlock();
}

void time_worker()
{
    while (true) {
        while (true) {
            if (false == timer_queue.empty())
            {
                event_type ev = timer_queue.top();
                if (ev.wakeup_time > system_clock::now())break;
                timer_l.lock();
                timer_queue.pop();
                timer_l.unlock();

                if (ev.event_id == OP_SEND_MOVE) {
                    OVER_EX* send_over = new OVER_EX();
                    send_over->op_mode = ev.event_id;
                    PostQueuedCompletionStatus(h_iocp, 1, ev.obj_id, &send_over->wsa_over);
                    break;
                }
                //else if (ev.event_id == OP_NPC_RESPAWN) {
                //    OVER_EX* send_over = new OVER_EX();
                //    send_over->op_mode = ev.event_id;
                //    PostQueuedCompletionStatus(h_iocp, 1, ev.obj_id, &send_over->wsa_over);
                //    break;
                //}
                //else if (ev.event_id == OP_PLAYER_MOVE_1s) {
                //    OVER_EX* send_over = new OVER_EX();
                //    send_over->op_mode = ev.event_id;
                //    PostQueuedCompletionStatus(h_iocp, 1, ev.obj_id, &send_over->wsa_over);
                //    break;
                //}
                //else if (ev.event_id == OP_PLAYER_ATTACK_1s) {
                //    OVER_EX* send_over = new OVER_EX();
                //    send_over->op_mode = ev.event_id;
                //    PostQueuedCompletionStatus(h_iocp, 1, ev.obj_id, &send_over->wsa_over);
                //    break;
                //}
                //else if (ev.event_id == OP_NPC_ATTACK_1s) {
                //    OVER_EX* send_over = new OVER_EX();
                //    send_over->op_mode = ev.event_id;
                //    send_over->object_id = ev.target_id;
                //    PostQueuedCompletionStatus(h_iocp, 1, ev.obj_id, &send_over->wsa_over);
                //    break;
                //}
                //else if (ev.event_id == OP_PLAYER_HP_RECOVERY_5s) {
                //    OVER_EX* send_over = new OVER_EX();
                //    send_over->op_mode = ev.event_id;
                //    PostQueuedCompletionStatus(h_iocp, 1, ev.obj_id, &send_over->wsa_over);
                //    break;
                //}
            }
            else {
                break;
            }

        }
        this_thread::sleep_for(1ms);
    }
}

//패킷 전송 함수
void send_packet(int id, void* p)
{
    unsigned char* packet = reinterpret_cast<unsigned char*>(p);
    OVER_EX* send_over = new OVER_EX;
    memcpy(send_over->iocp_buf, packet, packet[0]);
    send_over->op_mode = OP_MODE_SEND;
    send_over->wsa_buf.buf = reinterpret_cast<CHAR*>(send_over->iocp_buf);
    send_over->wsa_buf.len = packet[0];
    ZeroMemory(&send_over->wsa_over, sizeof(send_over->wsa_over));
    g_clients[id].c_lock.lock();
    if (true == g_clients[id].in_use)
        WSASend(g_clients[id].m_sock, &send_over->wsa_buf, 1,
            NULL, 0, &send_over->wsa_over, NULL);
    g_clients[id].c_lock.unlock();
}

void send_data(int id)
{
    sc_player_posi p;
    p.size = sizeof(p);
    p.type = SC_PlayerPosi;
    strncpy_s(p.p1_ID, playerdata.player1.ID, sizeof(playerdata.player1.ID));
    p.p1_is_main_ch = playerdata.player1.is_Main_CH;
    p.p1_main_behavior = playerdata.player1.Main_Behavior;
    p.p1_main_pos_x = playerdata.player1.Main_X;
    p.p1_main_pos_z = playerdata.player1.Main_Z;
    p.p1_main_rot_y = playerdata.player1.Main_Rot_Y;
    p.p1_main_hp = playerdata.player1.Main_HP;
    p.p1_main_mp = playerdata.player1.Main_MP;
    p.p1_sub_behavior = playerdata.player1.Sub_Behavior;
    p.p1_sub_pos_x = playerdata.player1.Sub_X;
    p.p1_sub_pos_z = playerdata.player1.Sub_Z;
    p.p1_sub_rot_y = playerdata.player1.Sub_Rot_Y;
    p.p1_sub_hp = playerdata.player1.Sub_HP;
    p.p1_sub_mp = playerdata.player1.Sub_MP;

    strncpy_s(p.p2_ID, playerdata.player2.ID, sizeof(playerdata.player2.ID));
    p.p2_is_main_ch = playerdata.player2.is_Main_CH;
    p.p2_main_behavior = playerdata.player2.Main_Behavior;
    p.p2_main_pos_x = playerdata.player2.Main_X;
    p.p2_main_pos_z = playerdata.player2.Main_Z;
    p.p2_main_rot_y = playerdata.player2.Main_Rot_Y;
    p.p2_main_hp = playerdata.player2.Main_HP;
    p.p2_main_mp = playerdata.player2.Main_MP;
    p.p2_sub_behavior = playerdata.player2.Sub_Behavior;
    p.p2_sub_pos_x = playerdata.player2.Sub_X;
    p.p2_sub_pos_z = playerdata.player2.Sub_Z;
    p.p2_sub_rot_y = playerdata.player2.Sub_Rot_Y;
    p.p2_sub_hp = playerdata.player2.Sub_HP;
    p.p2_sub_mp = playerdata.player2.Sub_MP;

    strncpy_s(p.p3_ID, playerdata.player3.ID, sizeof(playerdata.player3.ID));
    p.p3_is_main_ch = playerdata.player3.is_Main_CH;
    p.p3_main_behavior = playerdata.player3.Main_Behavior;
    p.p3_main_pos_x = playerdata.player3.Main_X;
    p.p3_main_pos_z = playerdata.player3.Main_Z;
    p.p3_main_rot_y = playerdata.player3.Main_Rot_Y;
    p.p3_main_hp = playerdata.player3.Main_HP;
    p.p3_main_mp = playerdata.player3.Main_MP;
    p.p3_sub_behavior = playerdata.player3.Sub_Behavior;
    p.p3_sub_pos_x = playerdata.player3.Sub_X;
    p.p3_sub_pos_z = playerdata.player3.Sub_Z;
    p.p3_sub_rot_y = playerdata.player3.Sub_Rot_Y;
    p.p3_sub_hp = playerdata.player3.Sub_HP;
    p.p3_sub_mp = playerdata.player3.Sub_MP;

    strncpy_s(p.p4_ID, playerdata.player4.ID, sizeof(playerdata.player4.ID));
    p.p4_is_main_ch = playerdata.player4.is_Main_CH;
    p.p4_main_behavior = playerdata.player4.Main_Behavior;
    p.p4_main_pos_x = playerdata.player4.Main_X;
    p.p4_main_pos_z = playerdata.player4.Main_Z;
    p.p4_main_rot_y = playerdata.player4.Main_Rot_Y;
    p.p4_main_hp = playerdata.player4.Main_HP;
    p.p4_main_mp = playerdata.player4.Main_MP;
    p.p4_sub_behavior = playerdata.player4.Sub_Behavior;
    p.p4_sub_pos_x = playerdata.player4.Sub_X;
    p.p4_sub_pos_z = playerdata.player4.Sub_Z;
    p.p4_sub_rot_y = playerdata.player4.Sub_Rot_Y;
    p.p4_sub_hp = playerdata.player4.Sub_HP;
    p.p4_sub_mp = playerdata.player4.Sub_MP;

    //cout << p.p1_ID << p.p1_pos_x << endl;
    //cout << p.p2_ID << p.p2_pos_x << endl;
    //cout << p.p3_ID << p.p3_pos_x << endl;
    //cout << p.p4_ID << p.p4_pos_x << endl;

    for (int i = 0; i < MAX_USER; i++) {
        if (g_clients[i].in_use == true) {
            send_packet(i, &p);
        }
    }
    add_timer(1000, OP_SEND_MOVE, system_clock::now() + 10ms, 0);
}
//클라이언트 연결
void add_new_client(SOCKET ns)
{
    cout << "New Player Login" << endl;

    int i;
    id_lock.lock();
    for (i = 0; i < MAX_USER; ++i)
        if (false == g_clients[i].in_use) break;
    id_lock.unlock();
    if (MAX_USER == i) {
        cout << "Max user limit exceeded.\n";
        closesocket(ns);
    }
    else {
        g_clients[i].c_lock.lock();
        g_clients[i].in_use = true;
        g_clients[i].m_sock = ns;
        g_clients[i].name[0] = 0;
        g_clients[i].id = -1;
        g_clients[i].c_lock.unlock();

        g_clients[i].m_packet_start = g_clients[i].m_recv_over.iocp_buf;
        g_clients[i].m_recv_over.op_mode = OP_MODE_RECV;
        g_clients[i].m_recv_over.wsa_buf.buf
            = reinterpret_cast<CHAR*>(g_clients[i].m_recv_over.iocp_buf);
        g_clients[i].m_recv_over.wsa_buf.len = sizeof(g_clients[i].m_recv_over.iocp_buf);
        ZeroMemory(&g_clients[i].m_recv_over.wsa_over, sizeof(g_clients[i].m_recv_over.wsa_over));
        g_clients[i].m_recv_start = g_clients[i].m_recv_over.iocp_buf;

        CreateIoCompletionPort(reinterpret_cast<HANDLE>(ns), h_iocp, i, 0);
        DWORD flags = 0;
        int ret;
        g_clients[i].c_lock.lock();
        if (true == g_clients[i].in_use) {
            ret = WSARecv(g_clients[i].m_sock, &g_clients[i].m_recv_over.wsa_buf, 1, NULL,
                &flags, &g_clients[i].m_recv_over.wsa_over, NULL);
        }
        g_clients[i].c_lock.unlock();
        if (SOCKET_ERROR == ret) {
            int err_no = WSAGetLastError();
            if (ERROR_IO_PENDING != err_no)
                error_display("WSARecv : ", err_no);
        }
    }

    SOCKET cSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
    g_accept_over.op_mode = OP_MODE_ACCEPT;
    g_accept_over.wsa_buf.len = static_cast<ULONG> (cSocket);
    ZeroMemory(&g_accept_over.wsa_over, sizeof(&g_accept_over.wsa_over));
    AcceptEx(g_lSocket, cSocket, g_accept_over.iocp_buf, 0, 32, 32, NULL, &g_accept_over.wsa_over);
}
//연결 종료
void disconnect_client(int id)
{
    g_clients[id].c_lock.lock();
    g_clients[id].in_use = false;

    closesocket(g_clients[id].m_sock);
    g_clients[id].m_sock = 0;
    g_clients[id].first_x = 0;
    g_clients[id].first_y = 0;
    g_clients[id].x = 0;
    g_clients[id].y = 0;
    g_clients[id].c_lock.unlock();
}

//패킷 처리 함수 
void process_packet(int id)
{
    USHORT p_type = g_clients[id].m_packet_start[2];
    //cout <<"Type: "<< p_type << endl;
    switch (p_type) {
    case CS_LOGIN:
    {
        playerdata.playerNum += 1;
        cs_Login* p = reinterpret_cast<cs_Login*>(g_clients[id].m_packet_start);
        cout << "로그인 패킷 수신 ID:" << p->ID << "Main 케릭터: " << p->main_charc << "Sub 케릭터: " << p->sub_charc << endl;
        if (playerdata.playerNum == 1 && playerdata.player1.Set_Name == false)
        {
            strncpy_s(playerdata.player1.ID, p->ID, sizeof(p->ID));
            playerdata.player1.Set_Name = true;
            playerdata.player1.Main_CH = p->main_charc;
            playerdata.player1.Sub_CH = p->sub_charc;
            //if (p->main_charc == 1) {
            //    playerdata.player1.Main_HP = 500;
            //}
            //else if (p->main_charc == 2) {
            //    playerdata.player1.Main_HP = 400;
            //}
            //else if (p->main_charc == 3) {
            //    playerdata.player1.Main_HP = 400;
            //}
            //else if (p->main_charc == 4) {
            //    playerdata.player1.Main_HP = 500;
            //}
            //if (p->sub_charc == 1) {
            //    playerdata.player1.Sub_HP = 500;
            //}
            //else if (p->sub_charc == 2) {
            //    playerdata.player1.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 3) {
            //    playerdata.player1.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 4) {
            //    playerdata.player1.Sub_HP = 500;
            //}
        }
        else if (playerdata.playerNum == 2 && playerdata.player2.Set_Name == false)
        {
            strncpy_s(playerdata.player2.ID, p->ID, sizeof(p->ID));
            playerdata.player2.Set_Name = true;
            playerdata.player2.Main_CH = p->main_charc;
            playerdata.player2.Sub_CH = p->sub_charc;
            //if (p->main_charc == 1) {
            //    playerdata.player2.Main_HP = 500;
            //}
            //else if (p->main_charc == 2) {
            //    playerdata.player2.Main_HP = 400;
            //}
            //else if (p->main_charc == 3) {
            //    playerdata.player2.Main_HP = 400;
            //}
            //else if (p->main_charc == 4) {
            //    playerdata.player2.Main_HP = 500;
            //}
            //if (p->sub_charc == 1) {
            //    playerdata.player2.Sub_HP = 500;
            //}
            //else if (p->sub_charc == 2) {
            //    playerdata.player2.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 3) {
            //    playerdata.player2.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 4) {
            //    playerdata.player2.Sub_HP = 500;
            //}
        }
        else if (playerdata.playerNum == 3 && playerdata.player3.Set_Name == false)
        {
            strncpy_s(playerdata.player3.ID, p->ID, sizeof(p->ID));
            playerdata.player3.Set_Name = true;
            playerdata.player3.Main_CH = p->main_charc;
            playerdata.player3.Sub_CH = p->sub_charc;
            //if (p->main_charc == 1) {
            //    playerdata.player3.Main_HP = 500;
            //}
            //else if (p->main_charc == 2) {
            //    playerdata.player3.Main_HP = 400;
            //}
            //else if (p->main_charc == 3) {
            //    playerdata.player3.Main_HP = 400;
            //}
            //else if (p->main_charc == 4) {
            //    playerdata.player3.Main_HP = 500;
            //}
            //if (p->sub_charc == 1) {
            //    playerdata.player3.Sub_HP = 500;
            //}
            //else if (p->sub_charc == 2) {
            //    playerdata.player3.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 3) {
            //    playerdata.player3.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 4) {
            //    playerdata.player3.Sub_HP = 500;
            //}
        }
        else if (playerdata.playerNum == 4 && playerdata.player4.Set_Name == false)
        {
            strncpy_s(playerdata.player4.ID, p->ID, sizeof(p->ID));
            playerdata.player4.Set_Name = true;
            playerdata.player4.Main_CH = p->main_charc;
            playerdata.player4.Sub_CH = p->sub_charc;
            //if (p->main_charc == 1) {
            //    playerdata.player4.Main_HP = 500;
            //}
            //else if (p->main_charc == 2) {
            //    playerdata.player4.Main_HP = 400;
            //}
            //else if (p->main_charc == 3) {
            //    playerdata.player4.Main_HP = 400;
            //}
            //else if (p->main_charc == 4) {
            //    playerdata.player4.Main_HP = 500;
            //}
            //if (p->sub_charc == 1) {
            //    playerdata.player4.Sub_HP = 500;
            //}
            //else if (p->sub_charc == 2) {
            //    playerdata.player4.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 3) {
            //    playerdata.player4.Sub_HP = 400;
            //}
            //else if (p->sub_charc == 4) {
            //    playerdata.player4.Sub_HP = 500;
            //}
        }

        cs_Login sp;
        sp.size = sizeof(sp);
        sp.type = p->type;
        strncpy_s(sp.ID, p->ID, sizeof(p->ID));
        sp.main_charc = p->main_charc;
        sp.sub_charc = p->sub_charc;
        //cout << sp.ID << ' ' << sp.main_charc << ' ' << sp.sub_charc << ' ' << endl;
        //cout << sp.size << endl;



        //맥스 유저 사용자로 바꾸자 for 문 부하 줄이기

        //기존 애한테 새로운 애가 왔다는걸 알려주고
        for (int i = 0; i < MAX_USER; i++) {
            if (g_clients[i].login_set == true) {
                if (g_clients[i].in_use == true) {
                    if (i != id) {
                        send_packet(i, &sp);
                        cout << i << " " << sp.ID << " " << sp.main_charc << " " << sp.sub_charc << endl;
                    }
                }
            }
        }

        //새로운 애한테 기존의 애의 정보를 주는거야
        cout << "새로운 애" << endl;
        for (auto lp : login_vec) {
            send_packet(id, &lp);
            cout << id << " " << lp.ID << " " << lp.main_charc << " " << lp.sub_charc << endl;
        }

        login_vec.push_back(sp);

        g_clients[id].login_set = true;

        break;
    }
    case CS_PlayerData: {
        cs_PlayerMove* p = reinterpret_cast<cs_PlayerMove*>(g_clients[id].m_packet_start);
        //스위치 문으로 변경 가능한지 확인하기

        if (strcmp(playerdata.player1.ID, p->ID) == 0) {
            playerdata.player1.is_Main_CH = p->is_main_ch;
            playerdata.player1.Main_Behavior = p->main_behavior_var;
            playerdata.player1.Main_X = p->main_pos_x;
            playerdata.player1.Main_Z = p->main_pos_z;
            playerdata.player1.Main_Rot_Y = p->main_rot_y;
            //playerdata.player1.Main_HP = p->main_hp;
            playerdata.player1.Main_MP = p->main_mp;
            playerdata.player1.Sub_Behavior = p->sub_behavior_var;
            playerdata.player1.Sub_X = p->sub_pos_x;
            playerdata.player1.Sub_Z = p->sub_pos_z;
            playerdata.player1.Sub_Rot_Y = p->sub_rot_y;
            //playerdata.player1.Sub_HP = p->sub_hp;
            playerdata.player1.Sub_MP = p->sub_mp;

        }
        else if (strcmp(playerdata.player2.ID, p->ID) == 0) {
            playerdata.player2.is_Main_CH = p->is_main_ch;
            playerdata.player2.Main_Behavior = p->main_behavior_var;
            playerdata.player2.Main_X = p->main_pos_x;
            playerdata.player2.Main_Z = p->main_pos_z;
            playerdata.player2.Main_Rot_Y = p->main_rot_y;
            //playerdata.player2.Main_HP = p->main_hp;
            playerdata.player2.Main_MP = p->main_mp;
            playerdata.player2.Sub_Behavior = p->sub_behavior_var;
            playerdata.player2.Sub_X = p->sub_pos_x;
            playerdata.player2.Sub_Z = p->sub_pos_z;
            playerdata.player2.Sub_Rot_Y = p->sub_rot_y;
            //playerdata.player2.Sub_HP = p->sub_hp;
            playerdata.player2.Sub_MP = p->sub_mp;

        }
        else if (strcmp(playerdata.player3.ID, p->ID) == 0) {
            playerdata.player3.is_Main_CH = p->is_main_ch;
            playerdata.player3.Main_Behavior = p->main_behavior_var;
            playerdata.player3.Main_X = p->main_pos_x;
            playerdata.player3.Main_Z = p->main_pos_z;
            playerdata.player3.Main_Rot_Y = p->main_rot_y;
            //playerdata.player3.Main_HP = p->main_hp;
            playerdata.player3.Main_MP = p->main_mp;
            playerdata.player3.Sub_Behavior = p->sub_behavior_var;
            playerdata.player3.Sub_X = p->sub_pos_x;
            playerdata.player3.Sub_Z = p->sub_pos_z;
            playerdata.player3.Sub_Rot_Y = p->sub_rot_y;
            //playerdata.player3.Sub_HP = p->sub_hp;
            playerdata.player3.Sub_MP = p->sub_mp;
        
        }
        else if (strcmp(playerdata.player4.ID, p->ID) == 0) {
            playerdata.player4.is_Main_CH = p->is_main_ch;
            playerdata.player4.Main_Behavior = p->main_behavior_var;
            playerdata.player4.Main_X = p->main_pos_x;
            playerdata.player4.Main_Z = p->main_pos_z;
            playerdata.player4.Main_Rot_Y = p->main_rot_y;
            //playerdata.player4.Main_HP = p->main_hp;
            playerdata.player4.Main_MP = p->main_mp;
            playerdata.player4.Sub_Behavior = p->sub_behavior_var;
            playerdata.player4.Sub_X = p->sub_pos_x;
            playerdata.player4.Sub_Z = p->sub_pos_z;
            playerdata.player4.Sub_Rot_Y = p->sub_rot_y;
            //playerdata.player4.Sub_HP = p->sub_hp;
            playerdata.player4.Sub_MP = p->sub_mp;
        
        }

        if (first_operate == false) {
            add_timer(1000, OP_SEND_MOVE, system_clock::now() + 10ms, 0);
            first_operate = true;
        }
        break;
    }
    case CS_GameStart:
    {
        cout << "StartGame 패킷 수신" << endl;
        cs_GameStart* p = reinterpret_cast<cs_GameStart*>(g_clients[id].m_packet_start);

        cs_GameStart gs;
        gs.size = sizeof(gs);
        gs.type = p->type;
        gs.is_Start = true;

        for (int i = 0; i < MAX_USER; i++) {
            if (g_clients[i].in_use == true) {
                send_packet(i, &gs);
            }
        }

        ////초기 좌표값 전송
        //sc_player_posi fp;
        //fp.size = sizeof(fp);
        //fp.type = SC_First_PlayerPosi;
        //strncpy_s(fp.p1_ID, playerdata.player1.ID, sizeof(playerdata.player1.ID));
        //fp.p1_main_behavior = 0;
        //fp.p1_main_pos_x = 6;
        //fp.p1_main_pos_z = 6;
        //fp.p1_main_rot_y = 0;
        //fp.p1_sub_behavior = 0;
        //fp.p1_sub_pos_x = 5;
        //fp.p1_sub_pos_z = 5;
        //fp.p1_sub_rot_y = 0;
        //strncpy_s(fp.p2_ID, playerdata.player2.ID, sizeof(playerdata.player2.ID));
        //fp.p2_main_behavior = 0;
        //fp.p2_main_pos_x = -6;
        //fp.p2_main_pos_z = 6;
        //fp.p2_main_rot_y = 0;
        //fp.p2_sub_behavior = 0;
        //fp.p2_sub_pos_x = -5;
        //fp.p2_sub_pos_z = 5;
        //fp.p2_sub_rot_y = 0;
        //strncpy_s(fp.p3_ID, playerdata.player3.ID, sizeof(playerdata.player3.ID));
        //fp.p3_main_behavior = 0;
        //fp.p3_main_pos_x = -6;
        //fp.p3_main_pos_z = -6;
        //fp.p3_main_rot_y = 0;
        //fp.p3_sub_behavior = 0;
        //fp.p3_sub_pos_x = -5;
        //fp.p3_sub_pos_z = -5;
        //fp.p3_sub_rot_y = 0;
        //strncpy_s(fp.p4_ID, playerdata.player4.ID, sizeof(playerdata.player4.ID));
        //fp.p4_main_behavior = 0;
        //fp.p4_main_pos_x = 6;
        //fp.p4_main_pos_z = -6;
        //fp.p4_main_rot_y = 0;
        //fp.p4_sub_behavior = 0;
        //fp.p4_sub_pos_x = 5;
        //fp.p4_sub_pos_z = -5;
        //fp.p4_sub_rot_y = 0;

        //for (int i = 0; i < MAX_USER; i++) {
        //    if (g_clients[i].in_use == true) {
        //        send_packet(i, &fp);
        //    }
        //}

        break;
    }
    case CS_Attack: {
        cs_Attack* p = reinterpret_cast<cs_Attack*>(g_clients[id].m_packet_start);
        cout << "Attack Packet 수신" << endl;
        //cout << p->target_id << " " << p->damage << endl;

        if (strcmp(playerdata.player1.ID, p->target_id) == 0) {
            playerdata.player1.Main_HP = p->damage;
            cout << p->target_id << ": " << playerdata.player1.Main_HP << endl;
        }
        else if (strcmp(playerdata.player2.ID, p->target_id) == 0) {
            playerdata.player2.Main_HP = p->damage;
            cout << p->target_id << ": " << playerdata.player2.Main_HP << endl;
        }
        else if (strcmp(playerdata.player3.ID, p->target_id) == 0) {
            playerdata.player3.Main_HP = p->damage;
            cout << p->target_id << ": " << playerdata.player3.Main_HP << endl;
        }
        else if (strcmp(playerdata.player4.ID, p->target_id) == 0) {
            playerdata.player4.Main_HP = p->damage;
            cout << p->target_id << ": " << playerdata.player4.Main_HP << endl;
        }
        break;
    }
    case CS_InGame:
    {
        cout << "InGame 패킷 수신" << endl;
        //초기 좌표값 전송
        sc_player_posi fp;
        fp.size = sizeof(fp);
        fp.type = SC_First_PlayerPosi;
        strncpy_s(fp.p1_ID, playerdata.player1.ID, sizeof(playerdata.player1.ID));
        fp.p1_main_behavior = 0;
        fp.p1_main_pos_x = 6;
        fp.p1_main_pos_z = 6;
        fp.p1_main_rot_y = 0;
        fp.p1_sub_behavior = 0;
        fp.p1_sub_pos_x = 5;
        fp.p1_sub_pos_z = 5;
        fp.p1_sub_rot_y = 0;

        strncpy_s(fp.p2_ID, playerdata.player2.ID, sizeof(playerdata.player2.ID));
        fp.p2_main_behavior = 0;
        fp.p2_main_pos_x = -6;
        fp.p2_main_pos_z = 6;
        fp.p2_main_rot_y = 0;
        fp.p2_sub_behavior = 0;
        fp.p2_sub_pos_x = -5;
        fp.p2_sub_pos_z = 5;
        fp.p2_sub_rot_y = 0;

        strncpy_s(fp.p3_ID, playerdata.player3.ID, sizeof(playerdata.player3.ID));
        fp.p3_main_behavior = 0;
        fp.p3_main_pos_x = -6;
        fp.p3_main_pos_z = -6;
        fp.p3_main_rot_y = 0;
        fp.p3_sub_behavior = 0;
        fp.p3_sub_pos_x = -5;
        fp.p3_sub_pos_z = -5;
        fp.p3_sub_rot_y = 0;

        strncpy_s(fp.p4_ID, playerdata.player4.ID, sizeof(playerdata.player4.ID));
        fp.p4_main_behavior = 0;
        fp.p4_main_pos_x = 6;
        fp.p4_main_pos_z = -6;
        fp.p4_main_rot_y = 0;
        fp.p4_sub_behavior = 0;
        fp.p4_sub_pos_x = 5;
        fp.p4_sub_pos_z = -5;
        fp.p4_sub_rot_y = 0;

        for (int i = 0; i < MAX_USER; i++) {
            if (g_clients[i].in_use == true) {
                send_packet(i, &fp);
            }
        }

        break;
    }
    default: cout << "Unknown Packet type [" << p_type << "] from Client [" << id << "]\n";
        while (true);
    }
}

constexpr int MIN_BUFF_SIZE = 1024;

void process_recv(int id, DWORD iosize)
{
    unsigned short p_size = g_clients[id].m_packet_start[0];
    //cout << p_size << endl;
    unsigned char* next_recv_ptr = g_clients[id].m_recv_start + iosize;
    while (p_size <= next_recv_ptr - g_clients[id].m_packet_start) {
        process_packet(id);
        g_clients[id].m_packet_start += p_size;
        if (g_clients[id].m_packet_start < next_recv_ptr)
            p_size = g_clients[id].m_packet_start[0];
        else break;
    }

    long long left_data = next_recv_ptr - g_clients[id].m_packet_start;

    if ((MAX_BUFFER - (next_recv_ptr - g_clients[id].m_recv_over.iocp_buf))
        < MIN_BUFF_SIZE) {
        memcpy(g_clients[id].m_recv_over.iocp_buf,
            g_clients[id].m_packet_start, left_data);
        g_clients[id].m_packet_start = g_clients[id].m_recv_over.iocp_buf;
        next_recv_ptr = g_clients[id].m_packet_start + left_data;
    }
    DWORD recv_flag = 0;
    g_clients[id].m_recv_start = next_recv_ptr;
    g_clients[id].m_recv_over.wsa_buf.buf = reinterpret_cast<CHAR*>(next_recv_ptr);
    g_clients[id].m_recv_over.wsa_buf.len = MAX_BUFFER -
        static_cast<int>(next_recv_ptr - g_clients[id].m_recv_over.iocp_buf);

    g_clients[id].c_lock.lock();
    if (true == g_clients[id].in_use) {
        WSARecv(g_clients[id].m_sock, &g_clients[id].m_recv_over.wsa_buf,
            1, NULL, &recv_flag, &g_clients[id].m_recv_over.wsa_over, NULL);
    }
    g_clients[id].c_lock.unlock();
}

//Worker Thread
void worker_thread()
{
    // 반복
    //   - 이 쓰레드를 IOCP thread pool에 등록  => GQCS
    //   - iocp가 처리를 맞긴 I/O완료 데이터를 꺼내기 => GQCS
    //   - 꺼낸 I/O완료 데이터를 처리
    while (true) {
        DWORD io_size;
        int key;
        ULONG_PTR iocp_key;
        WSAOVERLAPPED* lpover;
        int ret = GetQueuedCompletionStatus(h_iocp, &io_size, &iocp_key, &lpover, INFINITE);
        key = static_cast<int>(iocp_key);
        //cout << "Completion Detected" << endl;
        if (FALSE == ret) {
            //오류난 플레이어 disconnect
            disconnect_client(key);
            error_display("hGQCS Error : ", WSAGetLastError());

        }

        OVER_EX* over_ex = reinterpret_cast<OVER_EX*>(lpover);
        switch (over_ex->op_mode) {
        case OP_MODE_ACCEPT:
            add_new_client(static_cast<SOCKET>(over_ex->wsa_buf.len));
            break;
        case OP_MODE_RECV:
            if (0 == io_size)
                disconnect_client(key);
            else {
                //cout << "Packet from Client [" << key << "]" << endl;
                process_recv(key, io_size);
            }
            break;
        case OP_MODE_SEND:
            delete over_ex;
            break;
        case OP_SEND_MOVE:
        {
            send_data(key);
            delete over_ex;
            break;
        }
        //case OP_NPC_RESPAWN: {

        //    delete over_ex;
        //    break;
        //}
        //case OP_PLAYER_MOVE_NOTIFY: {
        //    g_clients[key].lua_lock.lock();
        //    g_clients[key].lua_lock.unlock();
        //    delete over_ex;
        //    break;
        //}
        //case OP_PLAYER_MOVE_1s: {
        //    g_clients[key].move_1s_time = false;
        //    delete over_ex;
        //    break;
        //}
        //case OP_PLAYER_ATTACK_1s: {
        //    g_clients[key].attack_1s_time = false;
        //    delete over_ex;
        //    break;
        //}
        //case OP_PLAYER_HP_RECOVERY_5s: {
        //    g_clients[key].hp_recov_5s_time = false;

        //    delete over_ex;
        //    break;
        //}
        //case OP_NPC_ATTACK_1s: {
        //    g_clients[key].attack_1s_time = false;
        //    delete over_ex;
        //    break;
        //}
        }
    }
}


int main()
{
    std::wcout.imbue(std::locale("korean"));

    for (auto& cl : g_clients)
        cl.in_use = false;

    WSADATA WSAData;
    WSAStartup(MAKEWORD(2, 0), &WSAData);
    h_iocp = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, NULL, 0);
    g_lSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
    CreateIoCompletionPort(reinterpret_cast<HANDLE>(g_lSocket), h_iocp, KEY_SERVER, 0);

    SOCKADDR_IN serverAddr;
    memset(&serverAddr, 0, sizeof(SOCKADDR_IN));
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_port = htons(SERVER_PORT);
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    ::bind(g_lSocket, (sockaddr*)&serverAddr, sizeof(serverAddr));
    listen(g_lSocket, 5); //5 -> SOMAXCONN

    SOCKET cSocket = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
    g_accept_over.op_mode = OP_MODE_ACCEPT;
    g_accept_over.wsa_buf.len = static_cast<int>(cSocket);
    ZeroMemory(&g_accept_over.wsa_over, sizeof(&g_accept_over.wsa_over));
    AcceptEx(g_lSocket, cSocket, g_accept_over.iocp_buf, 0, 32, 32, NULL, &g_accept_over.wsa_over);


    thread timer_thread{ time_worker };
    vector <thread> worker_threads;
    for (int i = 0; i < 6; ++i)
        worker_threads.emplace_back(worker_thread);
    for (auto& th : worker_threads)
        th.join();
    timer_thread.join();

    closesocket(g_lSocket);
    WSACleanup();
}