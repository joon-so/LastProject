//�̰� �Ǵ°ž� �̰� �̰�
//TCP
// ������ ����ϱ� ���ؼ� ���̺귯�� �����ؾ� �Ѵ�.
//#pragma comment(lib, "ws2_32")
//// inet_ntoa�� deprecated�� �Ǿ��µ�.. ����Ϸ��� �Ʒ� ������ �ؾ� �Ѵ�.
//#pragma warning(disable:4996)
//#include <stdio.h>
//#include <iostream>
//#include <vector>
//#include <thread>
//// ������ ����ϱ� ���� ���̺귯��
//#include <WinSock2.h>
//// ���� ���� ������
//#define BUFFERSIZE 1024
//using namespace std;
//
//
//struct LoginPacket {
//    int type;
//    char ID[20];
//    char PW[20];
//};
//
//struct ResultPacket {
//    int type;
//    int result;
//    float flo;
//    char ID[20];
//};
//
//void client(SOCKET clientSock, SOCKADDR_IN clientAddr, vector<thread*>* clientlist)
//{
//    // ���� ������ �ֿܼ� ����Ѵ�.
//    cout << "Client connected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
//
//    char x[1024];
//    int sign = recv(clientSock, (char*)&x, sizeof(x), 0);
//
//    cout << "������ ������ ũ��: "<<sign << endl;
//
//
//    LoginPacket* packet = reinterpret_cast<LoginPacket*>(x);
//    //int �ڷ��� ��ȯ
//    int type = htonl(packet->type);
//    cout << type << endl;
//    cout << packet->ID << endl;
//    cout << packet->PW << endl;
//
//
//    //Client�� ������ ����ü ��Ŷ
//    ResultPacket Rp;
//    Rp.type = 102;
//    memcpy(Rp.ID, "dnk1124", 20);
//    Rp.flo = 2.123f;
//    Rp.result = 3;
//
//    const wchar_t* message;
//    message = (const wchar_t*)&Rp;
//    //sign = send(clientSock, (char*)message, sizeof(Rp), 0);
//    cout <<"������ �۽�:" <<sign << endl;
//
//    //while (1)
//    //{
//    //   // ������ ���� ���� char* �������� �ޱ� ������ Ÿ�� ĳ������ �Ѵ�.
//    //   if (recv(clientSock, (char*)&x, sizeof(x), 0) == SOCKET_ERROR)
//    //   {
//    //      cout << "error" << endl;
//    //      break;
//    //   }
//    //   DataPacket* packet = reinterpret_cast<DataPacket*>(x);
//    //   cout << packet->Name << endl;
//    //   // ���� buffer�� ���ڸ��� ������ ���
//    //   if (buffer.size() > 0 && *(buffer.end() - 1) == '\r' && x == '\n')
//    //   {
//    //      // �޽����� exit�� ���� ���Ŵ�⸦ �����.
//    //      if (*buffer.begin() == 'e' && *(buffer.begin() + 1) == 'x' && *(buffer.begin() + 2) == 'i' && *(buffer.begin() + 3) == 't') {
//    //         break;
//    //      }
//    //      // �ֿܼ� ����ϰ� ���� �޽����� �޴´�.
//    //      wchar_t* echo = print(&buffer);
//    //      // client�� ���� �޽��� ������.
//    //      send(clientSock, (char*)echo, buffer.size() * 2 + 20, 0);
//    //      // ���� �޽����� ��(new�� ����� ����)�� �����߱� ������ �޸� �����Ѵ�.
//    //      delete echo;
//    //      // ���۸� ����.
//    //      buffer.clear();
//    //      // ���� �޽��� ���� ���
//    //      continue;
//    //   }
//    //   // ���ۿ� ���ڸ� �ϳ� �ִ´�.
//    //   buffer.push_back(x);
//    //}
//    // ���� ��Ⱑ ������ client�� ���� ����� ���´�.
//
//    closesocket(clientSock);
//    // ���� ������ �ֿܼ� ����Ѵ�.
//    cout << "Client disconnected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
//    // threadlist���� ���� �����带 �����Ѵ�.
//    for (auto ptr = clientlist->begin(); ptr < clientlist->end(); ptr++)
//    {
//        // thread ���̵� ���� ���� ã�Ƽ�
//        if ((*ptr)->get_id() == this_thread::get_id())
//        {
//            // ����Ʈ���� ����.
//            clientlist->erase(ptr);
//            break;
//        }
//    }
//    // thread �޸� ������ thread�� ���� ������ �ڵ����� ó���ȴ�.
//}
//// ���� �Լ�
//int main()
//{
//    // Ŭ���̾�Ʈ ���� ���� client list
//    vector<thread*> clientlist;
//    // ���� ���� ������ ����
//    WSADATA wsaData;
//    // ���� ����.
//    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
//    {
//        return 1;
//    }
//    // Internet�� Stream ������� ���� ����
//    SOCKET serverSock = socket(PF_INET, SOCK_STREAM, 0);
//    // ���� �ּ� ����
//    SOCKADDR_IN addr;
//    // ����ü �ʱ�ȭ
//    memset(&addr, 0, sizeof(addr));
//    // ������ Internet Ÿ��
//    addr.sin_family = AF_INET;
//    // �����̱� ������ local �����Ѵ�.
//    // Any�� ���� ȣ��Ʈ�� 127.0.0.1�� ��Ƶ� �ǰ� localhost�� ��Ƶ� �ǰ� ���� �� ����ϰ� �� �� �ֵ�. �װ��� INADDR_ANY�̴�.
//    addr.sin_addr.s_addr = htonl(INADDR_ANY);
//    // ���� ��Ʈ ����...���� 9090���� ������.
//    addr.sin_port = htons(9090);
//    // ������ ���� ������ ���Ͽ� ���ε��Ѵ�.
//    if (bind(serverSock, (SOCKADDR*)&addr, sizeof(SOCKADDR_IN)) == SOCKET_ERROR)
//    {
//        // ���� �ܼ� ���
//        cout << "error" << endl;
//        return 1;
//    }
//    // ������ ��� ���·� ��ٸ���.
//    if (listen(serverSock, SOMAXCONN) == SOCKET_ERROR)
//    {
//        // ���� �ܼ� ���
//        cout << "error" << endl;
//        return 1;
//    }
//    // ������ �����Ѵ�.
//    cout << "Server Start" << endl;
//    // ���� ������ ���� while�� ������ ����Ѵ�.
//    while (1)
//    {
//        // ���� ���� ����ü ������
//        int len = sizeof(SOCKADDR_IN);
//        // ���� ���� ����ü
//        SOCKADDR_IN clientAddr;
//        // client�� ������ �ϸ� SOCKET�� �޴´�.
//        SOCKET clientSock = accept(serverSock, (SOCKADDR*)&clientAddr, &len);
//        // �����带 �����ϰ� ������ ����Ʈ�� �ִ´�.
//        clientlist.push_back(new thread(client, clientSock, clientAddr, &clientlist));
//    }
//    // ���ᰡ �Ǹ� ������ ����Ʈ�� ���� �ִ� �����带 ������ ������ ��ٸ���.
//    if (clientlist.size() > 0)
//    {
//        for (auto ptr = clientlist.begin(); ptr < clientlist.end(); ptr++)
//        {
//            (*ptr)->join();
//        }
//    }
//    // ���� ���� ����
//    closesocket(serverSock);
//    // ���� ����
//    WSACleanup();
//    return 0;
//}


////UDP
//#pragma comment(lib, "ws2_32")
//// inet_ntoa�� deprecated�� �Ǿ��µ�.. ����Ϸ��� �Ʒ� ������ �ؾ� �Ѵ�.
//#pragma warning(disable:4996)
//#include <stdio.h>
//#include <iostream>
//// ������ ����ϱ� ���� ���̺귯��
//#include <vector>
//#include <WinSock2.h>
//#include <sqlext.h>
//
//#define PORT 9090   /* ��Ʈ ��ȣ */
//#define BUFSIZE 1024
//using namespace std;
//
//bool find_db(char id[20]);
//
//
//struct TypeCheck {
//    int type;
//};
//
//struct LoginPacket {
//    int type;
//    char ID[20];
//    char PW[20];
//};
//
//struct ResultPacket {
//    int type;
//    int result;
//    float flo;
//    char ID[20];
//};
//
//int main()
//{
//    WSADATA wsdata;
//    WSAStartup(MAKEWORD(2, 2), &wsdata);
//
//    SOCKET sock = socket(AF_INET, SOCK_DGRAM, 0);
//    SOCKADDR_IN addr;
//    ZeroMemory(&addr, sizeof(addr));
//    addr.sin_family = AF_INET;
//    addr.sin_addr.s_addr = htonl(INADDR_ANY);
//    addr.sin_port = htons(PORT);
//
//    bind(sock, (SOCKADDR*)&addr, sizeof(addr));
//    cout << "Server Open!" << endl;
//
//    while (true) {
//        char msg[BUFSIZE];
//        SOCKADDR_IN clntAddr;
//        ZeroMemory(&clntAddr, sizeof(clntAddr));
//        int clntAddrSz = sizeof(clntAddr);
//        recvfrom(sock, msg, sizeof(msg), 0, (SOCKADDR*)&clntAddr, &clntAddrSz);
//        cout << "������ ������ ũ��: " << endl;
//
//        //Ÿ�� Ȯ��
//        TypeCheck* typecheck = reinterpret_cast<TypeCheck*>(msg);
//        int typechange = htonl(typecheck->type);
//        cout << typechange << endl;
//
//        //�α��� ��Ŷ���� ��ȯ
//        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);
//
//        int type = htonl(packet->type);
//        cout << type << endl;
//        cout << packet->ID << endl;
//        cout << packet->PW << endl;
//        /* ���� �����͸� ��� */
//
//        find_db(packet->ID);
//
//        ResultPacket Rp;
//        Rp.type = 102;
//        char name[20] = "dnk9728";
//        memcpy(Rp.ID, name, 20);
//        Rp.flo = 2.123f;
//        Rp.result = 3;
//
//        const wchar_t* message;
//        message = (const wchar_t*)&Rp;
//
//
//        sendto(sock, (char*)message, sizeof(Rp), 0, (SOCKADDR*)&clntAddr, sizeof(clntAddr));
//
//        cout << "������ ���� �Ϸ�!" << endl;
//    }
//}


//TCP����
// ������ ����ϱ� ���ؼ� ���̺귯�� �����ؾ� �Ѵ�.
#pragma comment(lib, "ws2_32").
#pragma warning(disable:4996)
#include <stdio.h>
#include <iostream>
#include <vector>
#include <thread>
// ������ ����ϱ� ���� ���̺귯��
#include <WinSock2.h>
#include <sqlext.h>

// ���� ���� ������
#define BUFFERSIZE 1024
#define PORT 9090
using namespace std;

//client -> server
//type 101 = �α��� ��Ŷ, 102 = ���� ����, 103 = stage �ٲٱ�

//server -> client
//type 201 = �α��� ��� ��Ŷ


int find_db(char id[20], char pw[20], int *stage);
void insert_new_account_db(char fid[20], char fpw[20]);
void update_stage_data_db(char fid[20], int stage);

struct TypeCheck {
    int type;
};

struct LoginPacket {
    int type;
    char ID[20];
    char PW[20];
};

struct ResultPacket {
    int type;
    int result;
    int stageData;
};

struct ChangeStagePacket {
    int type;
    char ID[20];
    int stageData;
};

void client(SOCKET clientSock, SOCKADDR_IN clientAddr, vector<thread*>* clientlist)
{
    // ���� ������ �ֿܼ� ����Ѵ�.
    cout << "Client connected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;

    //Ŭ���̾�Ʈ�κ��� ������ ����
    char msg[BUFFERSIZE];
    int sign = recv(clientSock, (char*)&msg, BUFFERSIZE, 0);

    TypeCheck* typecheck = reinterpret_cast<TypeCheck*>(msg);
    int typechange = htonl(typecheck->type);
    cout <<"������ Type" <<typechange << endl;
    cout << "������ ������ ũ��: "<<sign << endl;

    if (typechange == 101)  //�α��� ��Ŷ
    {
        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);
        //int �ڷ��� ��ȯ
        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        cout << packet->PW << endl;

        //DB���� �ش� ���̵� �ּ� �˻�
        int stageNum;
        int result_find = find_db(packet->ID, packet->PW, &stageNum);

        cout <<"DB ���" << result_find << endl;

        ResultPacket Rp;
        Rp.type = 201;
        Rp.result = result_find;
        if (result_find == 3)
            Rp.stageData = stageNum;
        else
            Rp.stageData = -1;

        cout <<"StageData: " <<Rp.stageData << endl;
        const wchar_t* message;
        message = (const wchar_t*)&Rp;
        sign = send(clientSock, (char*)message, sizeof(Rp), 0);
        cout << "������ �۽�:" << sign << endl;
    }
    else if (typechange == 102) //���ο� ���� ��Ŷ
    {
        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);
        //int �ڷ��� ��ȯ
        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        cout << packet->PW << endl;

        //DB���� �ش� ���̵� �ּ� �˻�
        insert_new_account_db(packet->ID, packet->PW);

        cout << "DB�� ���� �Ϸ�" << endl;
    }
    else if (typechange == 103) //���ο� ���� ��Ŷ
    {
        ChangeStagePacket* packet = reinterpret_cast<ChangeStagePacket*>(msg);
        //int �ڷ��� ��ȯ
        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        int stage = htonl(packet->stageData);
        cout << stage << endl;

        //DB���� �ش� ���̵� �ּ� �˻�
        update_stage_data_db(packet->ID, stage);

        cout << "DB�� �������� ���� ���� �Ϸ�" << endl;
    }

    closesocket(clientSock);
    // ���� ������ �ֿܼ� ����Ѵ�.
    cout << "Client disconnected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
    // threadlist���� ���� �����带 �����Ѵ�.
    for (auto ptr = clientlist->begin(); ptr < clientlist->end(); ptr++)
    {
        // thread ���̵� ���� ���� ã�Ƽ�
        if ((*ptr)->get_id() == this_thread::get_id())
        {
            // ����Ʈ���� ����.
            clientlist->erase(ptr);
            break;
        }
    }
    // thread �޸� ������ thread�� ���� ������ �ڵ����� ó���ȴ�.
}

// ���� �Լ�
int main()
{
    // Ŭ���̾�Ʈ ���� ���� client list
    vector<thread*> clientlist;
    // ���� ���� ������ ����
    WSADATA wsaData;
    // ���� ����.
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
    {
        return 1;
    }
    // Internet�� Stream ������� ���� ����
    SOCKET serverSock = socket(PF_INET, SOCK_STREAM, 0);
    // ���� �ּ� ����
    SOCKADDR_IN addr;
    // ����ü �ʱ�ȭ
    memset(&addr, 0, sizeof(addr));
    // ������ Internet Ÿ��
    addr.sin_family = AF_INET;
    // �����̱� ������ local �����Ѵ�.
    // Any�� ���� ȣ��Ʈ�� 127.0.0.1�� ��Ƶ� �ǰ� localhost�� ��Ƶ� �ǰ� ���� �� ����ϰ� �� �� �ֵ�. �װ��� INADDR_ANY�̴�.
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    // ���� ��Ʈ ����...���� 9090���� ������.
    addr.sin_port = htons(PORT);
    // ������ ���� ������ ���Ͽ� ���ε��Ѵ�.
    if (bind(serverSock, (SOCKADDR*)&addr, sizeof(SOCKADDR_IN)) == SOCKET_ERROR)
    {
        // ���� �ܼ� ���
        cout << "error" << endl;
        return 1;
    }
    // ������ ��� ���·� ��ٸ���.
    if (listen(serverSock, SOMAXCONN) == SOCKET_ERROR)
    {
        // ���� �ܼ� ���
        cout << "error" << endl;
        return 1;
    }
    // ������ �����Ѵ�.
    cout << "Server Start" << endl;
    // ���� ������ ���� while�� ������ ����Ѵ�.
    while (1)
    {
        // ���� ���� ����ü ������
        int len = sizeof(SOCKADDR_IN);
        // ���� ���� ����ü
        SOCKADDR_IN clientAddr;
        // client�� ������ �ϸ� SOCKET�� �޴´�.
        SOCKET clientSock = accept(serverSock, (SOCKADDR*)&clientAddr, &len);
        // �����带 �����ϰ� ������ ����Ʈ�� �ִ´�.
        clientlist.push_back(new thread(client, clientSock, clientAddr, &clientlist));
    }
    // ���ᰡ �Ǹ� ������ ����Ʈ�� ���� �ִ� �����带 ������ ������ ��ٸ���.
    if (clientlist.size() > 0)
    {
        for (auto ptr = clientlist.begin(); ptr < clientlist.end(); ptr++)
        {
            (*ptr)->join();
        }
    }
    // ���� ���� ����
    closesocket(serverSock);
    // ���� ����
    WSACleanup();
    return 0;
}

void show_error(SQLHANDLE hHandle, SQLSMALLINT hType, RETCODE RetCode)
{
    SQLSMALLINT iRec = 0;
    SQLINTEGER iError;
    WCHAR wszMessage[1000];
    WCHAR wszState[SQL_SQLSTATE_SIZE + 1];
    if (RetCode == SQL_INVALID_HANDLE) {
        wcout << L"Invalid handle!\n";
        return;
    }
    while (SQLGetDiagRec(hType, hHandle, ++iRec, wszState, &iError, wszMessage,
        (SQLSMALLINT)(sizeof(wszMessage) / sizeof(WCHAR)), (SQLSMALLINT*)NULL) == SQL_SUCCESS) {
        // Hide data truncated..
        if (wcsncmp(wszState, L"01004", 5)) {
            wcout << L"[" << wszState << L"]" << wszMessage << "(" << iError << ")" << endl;
        }
    }
}

int find_db(char fid[20], char fpw[20], int *stage)
{
    SQLHENV henv;
    SQLHDBC hdbc;
    SQLHSTMT hstmt = 0;
    SQLRETURN retcode;

    char id[20];
    char pw[20];
    int stagedata = -1;

    //�����͸� �д� ����
    SQLINTEGER STAGEDATA;
    SQLWCHAR ID[20], PW[20];
    
    SQLLEN cbID = 0, cbPW = 0, cbSTAGEDATA = 0;

    char buf[1024];
    SQLWCHAR query[1024];

    sprintf(buf, "SELECT ID, PW, StageData FROM User_Data WHERE ID = \'%s\'", fid);
    MultiByteToWideChar(CP_UTF8, 0, buf, strlen(buf), query, sizeof query / sizeof * query);
    query[strlen(buf)] = '\0';

    bool no_data = true;
    std::wcout.imbue(std::locale("korean"));

    // Allocate environment handle  
    retcode = SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &henv);

    // Set the ODBC version environment attribute  
    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
        retcode = SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, 0);
        show_error(hstmt, SQL_HANDLE_STMT, retcode);

        // Allocate connection handle  
        if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
            retcode = SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);
            show_error(hstmt, SQL_HANDLE_STMT, retcode);

            // Set login timeout to 5 seconds  
            if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                SQLSetConnectAttr(hdbc, SQL_LOGIN_TIMEOUT, (SQLPOINTER)5, 0);
                show_error(hstmt, SQL_HANDLE_STMT, retcode);

                // Connect to data source  
                retcode = SQLConnect(hdbc, (SQLWCHAR*)L"2020_fall", SQL_NTS, (SQLWCHAR*)NULL, 0, NULL, 0);
                show_error(hstmt, SQL_HANDLE_STMT, retcode);

                // Allocate statement handle  
                if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                    retcode = SQLAllocHandle(SQL_HANDLE_STMT, hdbc, &hstmt);

                    retcode = SQLExecDirect(hstmt, (SQLWCHAR*)query, SQL_NTS);
                    if (retcode == SQL_ERROR || retcode == SQL_SUCCESS_WITH_INFO)
                        show_error(hstmt, SQL_HANDLE_STMT, retcode);
                    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                        // Bind columns 1, 2, and 3;

                       retcode = SQLBindCol(hstmt, 1, SQL_WCHAR, &ID, 20, &cbID);
                       retcode = SQLBindCol(hstmt, 2, SQL_WCHAR, &PW, 20, &cbPW);
                       retcode = SQLBindCol(hstmt, 3, SQL_INTEGER, &STAGEDATA, 100, &cbSTAGEDATA);


                        // Fetch and print each row of data. On an error, display a message and exit.  
                        for (int i = 0; ; i++) {

                            retcode = SQLFetch(hstmt);
                            if (retcode == SQL_ERROR || retcode == SQL_SUCCESS_WITH_INFO)
                                show_error(hstmt, SQL_HANDLE_STMT, retcode);
                            if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO)
                            {
                                no_data = false;
                                cout << "DB FIND ACCOUNT" << endl;

                                //wcout << ID << endl;
                                //wcout << PW << endl;
                                //cout << STAGEDATA << endl;
                                
                                //SQL�ڷ������� �Ϲ� �ڷ������� ��ȯ
                                int len = 0;
                                len = (wcslen(ID) + 1) * 2;
                                wcstombs(id, ID, len);
                                len = (wcslen(PW) + 1) * 2;
                                wcstombs(pw, PW, len);
                                stagedata = STAGEDATA;

                                break;
                            }
                            else
                                break;
                        }

                    }

                    // Process data  
                    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                        SQLCancel(hstmt);
                        SQLFreeHandle(SQL_HANDLE_STMT, hstmt);
                    }

                    SQLDisconnect(hdbc);
                }

                SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
            }
        }
        SQLFreeHandle(SQL_HANDLE_ENV, henv);
    }

    if (stagedata != -1)
        *stage = stagedata;

    string sizeid = fid;
    string sizepw = fpw;

    //1 = DB�� ����, 2= ���̵�� ������ ��й�ȣ Ʋ��, 3 = ������ ���� �α��� ����
    if (no_data) {
        cout << "������ ����" << endl;
        return 1;
    }
    else if (strncmp(id, fid, sizeid.size()) == 0)
    {
        if (strncmp(pw, fpw, sizepw.size()) == 0)
            return 3;
        else
            return 2;
    }
    else
        return 0;
}

void insert_new_account_db(char fid[20], char fpw[20]) {
    SQLHENV henv;
    SQLHDBC hdbc;
    SQLHSTMT hstmt = 0;
    SQLRETURN retcode;
    //�����͸� �д� ����

    char id[20];
    char pw[20];
    int stagedata = 0;

    char buf[1024];
    SQLWCHAR query[1024];

    sprintf(buf, "INSERT INTO User_Data (ID, PW, StageData) VALUES (\'%s\',\'%s\',%d)", fid, fpw, 0);
    MultiByteToWideChar(CP_UTF8, 0, buf, strlen(buf), query, sizeof query / sizeof * query);
    query[strlen(buf)] = '\0';

   
    std::wcout.imbue(std::locale("korean"));

    // Allocate environment handle  
    retcode = SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &henv);

    // Set the ODBC version environment attribute  
    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
        retcode = SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, 0);

        // Allocate connection handle  
        if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
            retcode = SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);

            // Set login timeout to 5 seconds  
            if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                SQLSetConnectAttr(hdbc, SQL_LOGIN_TIMEOUT, (SQLPOINTER)5, 0);

                // Connect to data source  
                retcode = SQLConnect(hdbc, (SQLWCHAR*)L"2020_fall", SQL_NTS, (SQLWCHAR*)NULL, 0, NULL, 0);

                // Allocate statement handle  
                if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                    retcode = SQLAllocHandle(SQL_HANDLE_STMT, hdbc, &hstmt);

                    retcode = SQLExecDirect(hstmt, (SQLWCHAR*)query, SQL_NTS);
                    show_error(hstmt, SQL_HANDLE_STMT, retcode);
                    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                    }

                    // Process data  
                    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                        SQLCancel(hstmt);
                        SQLFreeHandle(SQL_HANDLE_STMT, hstmt);
                    }

                    SQLDisconnect(hdbc);
                }

                SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
            }
        }
        SQLFreeHandle(SQL_HANDLE_ENV, henv);
    }
}

void update_stage_data_db(char fid[20], int stage) {
    SQLHENV henv;
    SQLHDBC hdbc;
    SQLHSTMT hstmt = 0;
    SQLRETURN retcode;
    //�����͸� �д� ����

    char id[20];

    char buf[1024];
    SQLWCHAR query[1024];

    sprintf(buf, "UPDATE User_Data SET StageData = %d WHERE ID = \'%s\' ",  stage, fid);
    MultiByteToWideChar(CP_UTF8, 0, buf, strlen(buf), query, sizeof query / sizeof * query);
    query[strlen(buf)] = '\0';


    std::wcout.imbue(std::locale("korean"));

    // Allocate environment handle  
    retcode = SQLAllocHandle(SQL_HANDLE_ENV, SQL_NULL_HANDLE, &henv);

    // Set the ODBC version environment attribute  
    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
        retcode = SQLSetEnvAttr(henv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, 0);

        // Allocate connection handle  
        if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
            retcode = SQLAllocHandle(SQL_HANDLE_DBC, henv, &hdbc);

            // Set login timeout to 5 seconds  
            if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                SQLSetConnectAttr(hdbc, SQL_LOGIN_TIMEOUT, (SQLPOINTER)5, 0);

                // Connect to data source  
                retcode = SQLConnect(hdbc, (SQLWCHAR*)L"2020_fall", SQL_NTS, (SQLWCHAR*)NULL, 0, NULL, 0);

                // Allocate statement handle  
                if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                    retcode = SQLAllocHandle(SQL_HANDLE_STMT, hdbc, &hstmt);

                    retcode = SQLExecDirect(hstmt, (SQLWCHAR*)query, SQL_NTS);
                    show_error(hstmt, SQL_HANDLE_STMT, retcode);
                    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                    }

                    // Process data  
                    if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
                        SQLCancel(hstmt);
                        SQLFreeHandle(SQL_HANDLE_STMT, hstmt);
                    }

                    SQLDisconnect(hdbc);
                }

                SQLFreeHandle(SQL_HANDLE_DBC, hdbc);
            }
        }
        SQLFreeHandle(SQL_HANDLE_ENV, henv);
    }
}