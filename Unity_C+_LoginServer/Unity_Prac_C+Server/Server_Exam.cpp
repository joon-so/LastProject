//이거 되는거야 이거 이거
//TCP
// 소켓을 사용하기 위해서 라이브러리 참조해야 한다.
//#pragma comment(lib, "ws2_32")
//// inet_ntoa가 deprecated가 되었는데.. 사용하려면 아래 설정을 해야 한다.
//#pragma warning(disable:4996)
//#include <stdio.h>
//#include <iostream>
//#include <vector>
//#include <thread>
//// 소켓을 사용하기 위한 라이브러리
//#include <WinSock2.h>
//// 수신 버퍼 사이즈
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
//    // 접속 정보를 콘솔에 출력한다.
//    cout << "Client connected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
//
//    char x[1024];
//    int sign = recv(clientSock, (char*)&x, sizeof(x), 0);
//
//    cout << "수신한 데이터 크기: "<<sign << endl;
//
//
//    LoginPacket* packet = reinterpret_cast<LoginPacket*>(x);
//    //int 자료형 변환
//    int type = htonl(packet->type);
//    cout << type << endl;
//    cout << packet->ID << endl;
//    cout << packet->PW << endl;
//
//
//    //Client로 전송할 구조체 패킷
//    ResultPacket Rp;
//    Rp.type = 102;
//    memcpy(Rp.ID, "dnk1124", 20);
//    Rp.flo = 2.123f;
//    Rp.result = 3;
//
//    const wchar_t* message;
//    message = (const wchar_t*)&Rp;
//    //sign = send(clientSock, (char*)message, sizeof(Rp), 0);
//    cout <<"데이터 송신:" <<sign << endl;
//
//    //while (1)
//    //{
//    //   // 수신을 받을 때도 char* 형식으로 받기 때문에 타입 캐스팅을 한다.
//    //   if (recv(clientSock, (char*)&x, sizeof(x), 0) == SOCKET_ERROR)
//    //   {
//    //      cout << "error" << endl;
//    //      break;
//    //   }
//    //   DataPacket* packet = reinterpret_cast<DataPacket*>(x);
//    //   cout << packet->Name << endl;
//    //   // 만약 buffer의 끝자리가 개행일 경우
//    //   if (buffer.size() > 0 && *(buffer.end() - 1) == '\r' && x == '\n')
//    //   {
//    //      // 메시지가 exit일 경우는 수신대기를 멈춘다.
//    //      if (*buffer.begin() == 'e' && *(buffer.begin() + 1) == 'x' && *(buffer.begin() + 2) == 'i' && *(buffer.begin() + 3) == 't') {
//    //         break;
//    //      }
//    //      // 콘솔에 출력하고 에코 메시지를 받는다.
//    //      wchar_t* echo = print(&buffer);
//    //      // client로 에코 메시지 보낸다.
//    //      send(clientSock, (char*)echo, buffer.size() * 2 + 20, 0);
//    //      // 에코 메시지를 힙(new을 사용한 선언)에 선언했기 때문에 메모리 해지한다.
//    //      delete echo;
//    //      // 버퍼를 비운다.
//    //      buffer.clear();
//    //      // 다음 메시지 수신 대기
//    //      continue;
//    //   }
//    //   // 버퍼에 글자를 하나 넣는다.
//    //   buffer.push_back(x);
//    //}
//    // 수신 대기가 끝나면 client와 소켓 통신을 끊는다.
//
//    closesocket(clientSock);
//    // 접속 정보를 콘솔에 출력한다.
//    cout << "Client disconnected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
//    // threadlist에서 현재 쓰레드를 제거한다.
//    for (auto ptr = clientlist->begin(); ptr < clientlist->end(); ptr++)
//    {
//        // thread 아이디가 같은 것을 찾아서
//        if ((*ptr)->get_id() == this_thread::get_id())
//        {
//            // 리스트에서 뺀다.
//            clientlist->erase(ptr);
//            break;
//        }
//    }
//    // thread 메모리 해지는 thread가 종료 됨으로 자동으로 처리된다.
//}
//// 실행 함수
//int main()
//{
//    // 클라이언트 접속 중인 client list
//    vector<thread*> clientlist;
//    // 소켓 정보 데이터 설정
//    WSADATA wsaData;
//    // 소켓 실행.
//    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
//    {
//        return 1;
//    }
//    // Internet의 Stream 방식으로 소켓 생성
//    SOCKET serverSock = socket(PF_INET, SOCK_STREAM, 0);
//    // 소켓 주소 설정
//    SOCKADDR_IN addr;
//    // 구조체 초기화
//    memset(&addr, 0, sizeof(addr));
//    // 소켓은 Internet 타입
//    addr.sin_family = AF_INET;
//    // 서버이기 때문에 local 설정한다.
//    // Any인 경우는 호스트를 127.0.0.1로 잡아도 되고 localhost로 잡아도 되고 양쪽 다 허용하게 할 수 있따. 그것이 INADDR_ANY이다.
//    addr.sin_addr.s_addr = htonl(INADDR_ANY);
//    // 서버 포트 설정...저는 9090으로 설정함.
//    addr.sin_port = htons(9090);
//    // 설정된 소켓 정보를 소켓에 바인딩한다.
//    if (bind(serverSock, (SOCKADDR*)&addr, sizeof(SOCKADDR_IN)) == SOCKET_ERROR)
//    {
//        // 에러 콘솔 출력
//        cout << "error" << endl;
//        return 1;
//    }
//    // 소켓을 대기 상태로 기다린다.
//    if (listen(serverSock, SOMAXCONN) == SOCKET_ERROR)
//    {
//        // 에러 콘솔 출력
//        cout << "error" << endl;
//        return 1;
//    }
//    // 서버를 시작한다.
//    cout << "Server Start" << endl;
//    // 다중 접속을 위해 while로 소켓을 대기한다.
//    while (1)
//    {
//        // 접속 설정 구조체 사이즈
//        int len = sizeof(SOCKADDR_IN);
//        // 접속 설정 구조체
//        SOCKADDR_IN clientAddr;
//        // client가 접속을 하면 SOCKET을 받는다.
//        SOCKET clientSock = accept(serverSock, (SOCKADDR*)&clientAddr, &len);
//        // 쓰레드를 실행하고 쓰레드 리스트에 넣는다.
//        clientlist.push_back(new thread(client, clientSock, clientAddr, &clientlist));
//    }
//    // 종료가 되면 쓰레드 리스트에 남아 있는 쓰레드를 종료할 때까지 기다린다.
//    if (clientlist.size() > 0)
//    {
//        for (auto ptr = clientlist.begin(); ptr < clientlist.end(); ptr++)
//        {
//            (*ptr)->join();
//        }
//    }
//    // 서버 소켓 종료
//    closesocket(serverSock);
//    // 소켓 종료
//    WSACleanup();
//    return 0;
//}


////UDP
//#pragma comment(lib, "ws2_32")
//// inet_ntoa가 deprecated가 되었는데.. 사용하려면 아래 설정을 해야 한다.
//#pragma warning(disable:4996)
//#include <stdio.h>
//#include <iostream>
//// 소켓을 사용하기 위한 라이브러리
//#include <vector>
//#include <WinSock2.h>
//#include <sqlext.h>
//
//#define PORT 9090   /* 포트 번호 */
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
//        cout << "수신한 데이터 크기: " << endl;
//
//        //타입 확인
//        TypeCheck* typecheck = reinterpret_cast<TypeCheck*>(msg);
//        int typechange = htonl(typecheck->type);
//        cout << typechange << endl;
//
//        //로그인 패킷으로 변환
//        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);
//
//        int type = htonl(packet->type);
//        cout << type << endl;
//        cout << packet->ID << endl;
//        cout << packet->PW << endl;
//        /* 받은 데이터를 출력 */
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
//        cout << "데이터 전송 완료!" << endl;
//    }
//}


//TCP서버
// 소켓을 사용하기 위해서 라이브러리 참조해야 한다.
#pragma comment(lib, "ws2_32").
#pragma warning(disable:4996)
#include <stdio.h>
#include <iostream>
#include <vector>
#include <thread>
// 소켓을 사용하기 위한 라이브러리
#include <WinSock2.h>
#include <sqlext.h>

// 수신 버퍼 사이즈
#define BUFFERSIZE 1024
#define PORT 9090
using namespace std;

//client -> server
//type 101 = 로그인 패킷, 102 = 계정 생성, 103 = stage 바꾸기

//server -> client
//type 201 = 로그인 결과 패킷


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
    // 접속 정보를 콘솔에 출력한다.
    cout << "Client connected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;

    //클라이언트로부터 데이터 수신
    char msg[BUFFERSIZE];
    int sign = recv(clientSock, (char*)&msg, BUFFERSIZE, 0);

    TypeCheck* typecheck = reinterpret_cast<TypeCheck*>(msg);
    int typechange = htonl(typecheck->type);
    cout <<"수신한 Type" <<typechange << endl;
    cout << "수신한 데이터 크기: "<<sign << endl;

    if (typechange == 101)  //로그인 패킷
    {
        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);
        //int 자료형 변환
        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        cout << packet->PW << endl;

        //DB에서 해당 아이디 주소 검색
        int stageNum;
        int result_find = find_db(packet->ID, packet->PW, &stageNum);

        cout <<"DB 결과" << result_find << endl;

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
        cout << "데이터 송신:" << sign << endl;
    }
    else if (typechange == 102) //새로운 계정 패킷
    {
        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);
        //int 자료형 변환
        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        cout << packet->PW << endl;

        //DB에서 해당 아이디 주소 검색
        insert_new_account_db(packet->ID, packet->PW);

        cout << "DB에 저장 완료" << endl;
    }
    else if (typechange == 103) //새로운 계정 패킷
    {
        ChangeStagePacket* packet = reinterpret_cast<ChangeStagePacket*>(msg);
        //int 자료형 변환
        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        int stage = htonl(packet->stageData);
        cout << stage << endl;

        //DB에서 해당 아이디 주소 검색
        update_stage_data_db(packet->ID, stage);

        cout << "DB에 스테이지 정보 변경 완료" << endl;
    }

    closesocket(clientSock);
    // 접속 정보를 콘솔에 출력한다.
    cout << "Client disconnected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
    // threadlist에서 현재 쓰레드를 제거한다.
    for (auto ptr = clientlist->begin(); ptr < clientlist->end(); ptr++)
    {
        // thread 아이디가 같은 것을 찾아서
        if ((*ptr)->get_id() == this_thread::get_id())
        {
            // 리스트에서 뺀다.
            clientlist->erase(ptr);
            break;
        }
    }
    // thread 메모리 해지는 thread가 종료 됨으로 자동으로 처리된다.
}

// 실행 함수
int main()
{
    // 클라이언트 접속 중인 client list
    vector<thread*> clientlist;
    // 소켓 정보 데이터 설정
    WSADATA wsaData;
    // 소켓 실행.
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
    {
        return 1;
    }
    // Internet의 Stream 방식으로 소켓 생성
    SOCKET serverSock = socket(PF_INET, SOCK_STREAM, 0);
    // 소켓 주소 설정
    SOCKADDR_IN addr;
    // 구조체 초기화
    memset(&addr, 0, sizeof(addr));
    // 소켓은 Internet 타입
    addr.sin_family = AF_INET;
    // 서버이기 때문에 local 설정한다.
    // Any인 경우는 호스트를 127.0.0.1로 잡아도 되고 localhost로 잡아도 되고 양쪽 다 허용하게 할 수 있따. 그것이 INADDR_ANY이다.
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    // 서버 포트 설정...저는 9090으로 설정함.
    addr.sin_port = htons(PORT);
    // 설정된 소켓 정보를 소켓에 바인딩한다.
    if (bind(serverSock, (SOCKADDR*)&addr, sizeof(SOCKADDR_IN)) == SOCKET_ERROR)
    {
        // 에러 콘솔 출력
        cout << "error" << endl;
        return 1;
    }
    // 소켓을 대기 상태로 기다린다.
    if (listen(serverSock, SOMAXCONN) == SOCKET_ERROR)
    {
        // 에러 콘솔 출력
        cout << "error" << endl;
        return 1;
    }
    // 서버를 시작한다.
    cout << "Server Start" << endl;
    // 다중 접속을 위해 while로 소켓을 대기한다.
    while (1)
    {
        // 접속 설정 구조체 사이즈
        int len = sizeof(SOCKADDR_IN);
        // 접속 설정 구조체
        SOCKADDR_IN clientAddr;
        // client가 접속을 하면 SOCKET을 받는다.
        SOCKET clientSock = accept(serverSock, (SOCKADDR*)&clientAddr, &len);
        // 쓰레드를 실행하고 쓰레드 리스트에 넣는다.
        clientlist.push_back(new thread(client, clientSock, clientAddr, &clientlist));
    }
    // 종료가 되면 쓰레드 리스트에 남아 있는 쓰레드를 종료할 때까지 기다린다.
    if (clientlist.size() > 0)
    {
        for (auto ptr = clientlist.begin(); ptr < clientlist.end(); ptr++)
        {
            (*ptr)->join();
        }
    }
    // 서버 소켓 종료
    closesocket(serverSock);
    // 소켓 종료
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

    //데이터를 읽는 변수
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
                                
                                //SQL자료형에서 일반 자료형으로 변환
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

    //1 = DB에 없음, 2= 아이디는 있으나 비밀번호 틀림, 3 = 데이터 있음 로그인 성공
    if (no_data) {
        cout << "데이터 없음" << endl;
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
    //데이터를 읽는 변수

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
    //데이터를 읽는 변수

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