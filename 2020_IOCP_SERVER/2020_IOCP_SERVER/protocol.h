#pragma once


constexpr int SERVER_PORT = 9090;
constexpr int MAX_ID_LEN = 10;
constexpr int MAX_USER = 20000;
constexpr int MAX_STR_LEN = 100;

//#pragma pack (push, 1)


constexpr USHORT CS_LOGIN = 1;
constexpr USHORT CS_PlayerData = 2;

constexpr USHORT SC_PlayerPosi = 101;

struct cs_Login {
	unsigned short size;
	unsigned short type;
	char ID[20];


	//메인 캐릭터 서브캐릭터 설정
	int main_charc;
	int sub_charc;
};

struct cs_PlayerMove {
	unsigned short size;
	unsigned short type;
	char ID[20];
	int behavior_var;
	float player_pos_x;
	float player_pos_z;
	float player_rot_y;
};

struct sc_player_posi {
	unsigned short size;
	unsigned short type;

	char p1_ID[20];
	int p1_behavior;
	float p1_pos_x;
	float p1_pos_z;
	float p1_rot_y;

	char p2_ID[20];
	int p2_behavior;
	float p2_pos_x;
	float p2_pos_z;
	float p2_rot_y;

	char p3_ID[20];
	int p3_behavior;
	float p3_pos_x;
	float p3_pos_z;
	float p3_rot_y;

	char p4_ID[20];
	int p4_behavior;
	float p4_pos_x;
	float p4_pos_z;
	float p4_rot_y;
};


//#pragma pack (pop)
