
///TCP ����

//// ������ ����ϱ� ���ؼ� ���̺귯�� �����ؾ� �Ѵ�.
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
//// ���� ���� ��, �ܼ� ��� �� echo ������ ����� �Լ�
//// ���� Ÿ���� char*���� wchar_t*�� �����ϰ� �Ķ���͵� vector<char>���� vector<wchar_t>�� �����ߴ�.
//wchar_t* print(vector<wchar_t>* str)
//{
//	// ����Ʈ ��ġ
//	int p = 0;
//	// ���� ����. +2�� \0�� �ֱ� ���� ũ�� (���۵� wchar_tŸ������ ����)
//	wchar_t out[BUFFERSIZE + 2];
//	// return�� �ϱ� ���ؼ��� ���� �����͸� ���� �ؾ� �Ѵ�. wchar_tŸ������ ����
//	wchar_t* ret = new wchar_t[str->size() + 10];
//	// �޸� ���� "echo - "�� ������.
//	// wchar_t�� 2byte �����̱� ������ �޸� ������ 7�� �ƴ� 14�� �ι�� �����Ѵ�.
//	memcpy(ret, L"echo - ", 14);
//	// �ܼ� ���
//	cout << "From Client message : ";
//	// buffer�������� �Ѿ�� �������� ��� �ݺ��� ���ؼ� �޴´�.
//	for (int n = 0; n < (str->size() / BUFFERSIZE) + 1; n++)
//	{
//		// ���� ������ ����
//		int size = str->size();
//		// ���� �����Ͱ� ���� ����� �Ѿ��� ���.
//		if (size > BUFFERSIZE) {
//			if (str->size() < (n + 1) * BUFFERSIZE)
//			{
//				size = str->size() % BUFFERSIZE;
//			}
//			else
//			{
//				size = BUFFERSIZE;
//			}
//		}
//		// echo �޽����� �ܼ� �޽����� �ۼ��Ѵ�.
//		for (int i = 0; i < size; i++, p++)
//		{
//			out[i] = *(str->begin() + p);
//			if (out[i] == '\0')
//			{
//				out[i] = ' ';
//			}
//			*(ret + p + 7) = out[i];
//		}
//		out[size] = '\0';
//		// �ܼ� �޽��� �ܼ� ���.
//		wprintf(L"%s", out);
//	}
//	cout << endl;
//	// ���� �޽����� ���� ���� + ">"�� �ִ´�.
//	memcpy(ret + p + 7, L"\n>\0", 6);
//	return ret;
//}
//// ���ӵǴ� client�� ������
//void client(SOCKET clientSock, SOCKADDR_IN clientAddr, vector<thread*>* clientlist)
//{
//	// ���� ������ �ֿܼ� ����Ѵ�.
//	cout << "Client connected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
//	// client�� �޽����� ������.(wchar_t Ÿ������ �����ؼ� ������.)
//	const wchar_t* message = L"Welcome server!\r\n>\0";
//	// send�Լ��� char* �������� ���� �� �ֱ� ������ Ÿ�� ĳ������ �Ѵ�.
//	// ������� ���ڿ� ���� * 2 �׸��� ������ \0�� ������ ���� +2�� �߰��Ѵ�.
//	send(clientSock, (char*)message, wcslen(message) * 2 + 2, 0);
//	// telent�� �ѱ��ھ� �����Ͱ� ���� ������ ���ڸ� ���� buffer�� �ʿ��ϴ�. (wchar_t Ÿ������ ����)
//	vector<wchar_t> buffer;
//	// ���� ������ char�� �ƴ� wchar_t�̴�.
//	wchar_t x;
//	while (1)
//	{
//		// ������ ���� ���� char* �������� �ޱ� ������ Ÿ�� ĳ������ �Ѵ�.
//		if (recv(clientSock, (char*)&x, sizeof(x), 0) == SOCKET_ERROR)
//		{
//			cout << "error" << endl;
//			break;
//		}
//		// ���� buffer�� ���ڸ��� ������ ���
//		if (buffer.size() > 0 && *(buffer.end() - 1) == '\r' && x == '\n')
//		{
//			// �޽����� exit�� ���� ���Ŵ�⸦ �����.
//			if (*buffer.begin() == 'e' && *(buffer.begin() + 1) == 'x' && *(buffer.begin() + 2) == 'i' && *(buffer.begin() + 3) == 't') {
//				break;
//			}
//			// �ֿܼ� ����ϰ� ���� �޽����� �޴´�.
//			wchar_t* echo = print(&buffer);
//			// client�� ���� �޽��� ������.
//			send(clientSock, (char*)echo, buffer.size() * 2 + 20, 0);
//			// ���� �޽����� ��(new�� ����� ����)�� �����߱� ������ �޸� �����Ѵ�.
//			delete echo;
//			// ���۸� ����.
//			buffer.clear();
//			// ���� �޽��� ���� ���
//			continue;
//		}
//		// ���ۿ� ���ڸ� �ϳ� �ִ´�.
//		buffer.push_back(x);
//	}
//	// ���� ��Ⱑ ������ client�� ���� ����� ���´�.
//	closesocket(clientSock);
//	// ���� ������ �ֿܼ� ����Ѵ�.
//	cout << "Client disconnected IP address = " << inet_ntoa(clientAddr.sin_addr) << ":" << ntohs(clientAddr.sin_port) << endl;
//	// threadlist���� ���� �����带 �����Ѵ�.
//	for (auto ptr = clientlist->begin(); ptr < clientlist->end(); ptr++)
//	{
//		// thread ���̵� ���� ���� ã�Ƽ�
//		if ((*ptr)->get_id() == this_thread::get_id())
//		{
//			// ����Ʈ���� ����.
//			clientlist->erase(ptr);
//			break;
//		}
//	}
//	// thread �޸� ������ thread�� ���� ������ �ڵ����� ó���ȴ�.
//}
//// ���� �Լ�
//int main()
//{
//	// Ŭ���̾�Ʈ ���� ���� client list
//	vector<thread*> clientlist;
//	// ���� ���� ������ ����
//	WSADATA wsaData;
//	// ���� ����.
//	if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
//	{
//		return 1;
//	}
//	// Internet�� Stream ������� ���� ����
//	SOCKET serverSock = socket(PF_INET, SOCK_STREAM, 0);
//	// ���� �ּ� ����
//	SOCKADDR_IN addr;
//	// ����ü �ʱ�ȭ
//	memset(&addr, 0, sizeof(addr));
//	// ������ Internet Ÿ��
//	addr.sin_family = AF_INET;
//	// �����̱� ������ local �����Ѵ�.
//	// Any�� ���� ȣ��Ʈ�� 127.0.0.1�� ��Ƶ� �ǰ� localhost�� ��Ƶ� �ǰ� ���� �� ����ϰ� �� �� �ֵ�. �װ��� INADDR_ANY�̴�.
//	addr.sin_addr.s_addr = htonl(INADDR_ANY);
//	// ���� ��Ʈ ����...���� 9090���� ������.
//	addr.sin_port = htons(9090);
//	// ������ ���� ������ ���Ͽ� ���ε��Ѵ�.
//	if (bind(serverSock, (SOCKADDR*)&addr, sizeof(SOCKADDR_IN)) == SOCKET_ERROR)
//	{
//		// ���� �ܼ� ���
//		cout << "error" << endl;
//		return 1;
//	}
//	// ������ ��� ���·� ��ٸ���.
//	if (listen(serverSock, SOMAXCONN) == SOCKET_ERROR)
//	{
//		// ���� �ܼ� ���
//		cout << "error" << endl;
//		return 1;
//	}
//	// ������ �����Ѵ�.
//	cout << "Server Start" << endl;
//	// ���� ������ ���� while�� ������ ����Ѵ�.
//	while (1)
//	{
//		// ���� ���� ����ü ������
//		int len = sizeof(SOCKADDR_IN);
//		// ���� ���� ����ü
//		SOCKADDR_IN clientAddr;
//		// client�� ������ �ϸ� SOCKET�� �޴´�.
//		SOCKET clientSock = accept(serverSock, (SOCKADDR*)&clientAddr, &len);
//		// �����带 �����ϰ� ������ ����Ʈ�� �ִ´�.
//		clientlist.push_back(new thread(client, clientSock, clientAddr, &clientlist));
//	}
//	// ���ᰡ �Ǹ� ������ ����Ʈ�� ���� �ִ� �����带 ������ ������ ��ٸ���.
//	if (clientlist.size() > 0)
//	{
//		for (auto ptr = clientlist.begin(); ptr < clientlist.end(); ptr++)
//		{
//			(*ptr)->join();
//		}
//	}
//	// ���� ���� ����
//	closesocket(serverSock);
//	// ���� ����
//	WSACleanup();
//	return 0;
//}



//UDP ����
#pragma comment(lib, "ws2_32")
// inet_ntoa�� deprecated�� �Ǿ��µ�.. ����Ϸ��� �Ʒ� ������ �ؾ� �Ѵ�.
#pragma warning(disable:4996)
#include <stdio.h>
#include <iostream>
#include <vector>
#include <thread>
// ������ ����ϱ� ���� ���̺귯��
#include <WinSock2.h>
#include <iostream>
#include <string>

using namespace std;

#define PORT_NUM 9090
SOCKET socket_;
char buffer[1024]; // ���� ������ ��
int addrlen = 0;
SOCKADDR_IN clientAddr;

void bufferManager(string b, SOCKADDR_IN s);
void sendBuffer(string buff, SOCKADDR_IN s);


struct Account {
    string  id;
    string  pw;
};

int main() {
    SOCKADDR_IN addr;
    WSADATA wsaData;

    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != NO_ERROR) {
        return 1;
    }
    if ((socket_ = socket(AF_INET, SOCK_DGRAM, 0)) == -1) {
        return 1;
    }
    memset((void*)&addr, 0x00, sizeof(addr));
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    addr.sin_port = htons(PORT_NUM);

    addrlen = sizeof(addr);

    if (bind(socket_, (SOCKADDR*)&addr, addrlen) == -1) {
        cout << "error...3";
        return 1;
    }

    cout << "start server !";
    while (1) { //Ŭ���̾�Ʈ���� ���� ��Ŷ�� üũ
        memset((void*)&buffer, 0x00, sizeof(buffer));
        addrlen = sizeof(clientAddr);
        recvfrom(socket_, (char*)&buffer, sizeof(buffer), 0, (SOCKADDR*)&clientAddr, &addrlen);
        //bufferManager(buffer, clientAddr);
        string client_addr = inet_ntoa(clientAddr.sin_addr);
        cout << "Client IP :" << client_addr << "���� �����Ͽ����ϴ�." << endl;
        string ID(buffer);
        cout << "ID :" << ID << endl;




        //Account account;
        //memcpy((void*)&account, buffer, sizeof(account));
        //cout <<"Client: " << account.id << endl;
    }


}

void bufferManager(string b, SOCKADDR_IN s)  // ���� ó�� , ���⼭ ���� ���� ������ ó���Ͽ� ĳ���͸� �����̵� �������� ȹ�� �ϵ� ����� ó��
{
    string client_addr = inet_ntoa(s.sin_addr);
    
    cout << "cleint" << client_addr << "- >" << b << endl;  // Ŭ���̾�Ʈ���� b ��� ������ �����Դ�.
    //Account* p = reinterpret_cast<Account*>(b);

    // ���� �������� b ��� ������ �����Ͽ� ���� Ŭ���̾�Ʈ���� �ʿ��� ������ �ְų� ����
    //sendBuffer(n, s); // ������ ������ Ŭ���̾�Ʈ�� �ٽ� ������. SOCKADDR_IN �� ��Ŷ�� ������ Ŭ���̾�Ʈ�� ip�� ����ִ�. 
}

void sendBuffer(string buff, SOCKADDR_IN s) // Ŭ���̾�Ʈ�� ��Ŷ ������
{
    sendto(socket_, buff.c_str(), buff.length(), 0, (SOCKADDR*)&s, addrlen); // Ŭ���̾�Ʈ�� ������ ���۸� ��Ŷ���� ������
    cout << "Server-> " << buff << endl;
}


//// ������ ����ϱ� ���ؼ� ���̺귯�� �����ؾ� �Ѵ�.
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
//void err_quit( char* msg)
//{
//	LPVOID lpMsgBuf;
//	FormatMessage(
//		FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
//		NULL, WSAGetLastError(),
//		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
//		(LPTSTR)&lpMsgBuf, 0, NULL);
//	MessageBox(NULL, (LPCTSTR)lpMsgBuf, (LPCWSTR)msg, MB_ICONERROR);
//	LocalFree(lpMsgBuf);
//	exit(1);
//}
//void err_display(const char* msg)
//{
//	LPVOID lpMsgBuf;
//	FormatMessage(
//		FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
//		NULL, WSAGetLastError(), 
//		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
//		(LPTSTR)&lpMsgBuf, 0, NULL);
//	printf("[%s] %s", msg, (char*)lpMsgBuf);
//	LocalFree(lpMsgBuf);
//}


#pragma comment(lib, "ws2_32")
// inet_ntoa�� deprecated�� �Ǿ��µ�.. ����Ϸ��� �Ʒ� ������ �ؾ� �Ѵ�.
#pragma warning(disable:4996)
#include <stdio.h>
#include <iostream>
// ������ ����ϱ� ���� ���̺귯��
#include <vector>
#include <WinSock2.h>
#include <sqlext.h>

#define PORT 9090   /* ��Ʈ ��ȣ */
#define BUFSIZE 1024
using namespace std;

bool find_db(char id[20]);


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
    float flo;
    char ID[20];
};

int main()
{
    WSADATA wsdata;
    WSAStartup(MAKEWORD(2, 2), &wsdata);

    SOCKET sock = socket(AF_INET, SOCK_DGRAM, 0);
    SOCKADDR_IN addr;
    ZeroMemory(&addr, sizeof(addr));
    addr.sin_family = AF_INET;
    addr.sin_addr.s_addr = htonl(INADDR_ANY);
    addr.sin_port = htons(PORT);

    bind(sock, (SOCKADDR*)&addr, sizeof(addr));
    cout << "Server Open!" << endl;

    while (true) {
        char msg[BUFSIZE];
        SOCKADDR_IN clntAddr;
        ZeroMemory(&clntAddr, sizeof(clntAddr));
        int clntAddrSz = sizeof(clntAddr);
        recvfrom(sock, msg, sizeof(msg), 0, (SOCKADDR*)&clntAddr, &clntAddrSz);
        cout << "������ ������ ũ��: " << endl;

        //Ÿ�� Ȯ��
        TypeCheck* typecheck = reinterpret_cast<TypeCheck*>(msg);
        int typechange = htonl(typecheck->type);
        cout << typechange << endl;

        //�α��� ��Ŷ���� ��ȯ
        LoginPacket* packet = reinterpret_cast<LoginPacket*>(msg);

        int type = htonl(packet->type);
        cout << type << endl;
        cout << packet->ID << endl;
        cout << packet->PW << endl;
        /* ���� �����͸� ��� */

        find_db(packet->ID);

        ResultPacket Rp;
        Rp.type = 102;
        char name[20] = "dnk9728";
        memcpy(Rp.ID, name, 20);
        Rp.flo = 2.123f;
        Rp.result = 3;

        const wchar_t* message;
        message = (const wchar_t*)&Rp;


        sendto(sock, (char*)message, sizeof(Rp), 0, (SOCKADDR*)&clntAddr, sizeof(clntAddr));

        cout << "������ ���� �Ϸ�!" << endl;
    }
}


SQLHENV henv;
SQLHDBC hdbc;
SQLHSTMT hstmt = 0;
SQLRETURN retcode;

char id[20];
char pw[20];
int stagedata = 0;

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