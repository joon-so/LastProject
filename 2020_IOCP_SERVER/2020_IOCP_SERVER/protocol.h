#pragma once


constexpr int SERVER_PORT = 9090;
constexpr int MAX_ID_LEN = 10;
constexpr int MAX_USER = 20000;
constexpr int MAX_STR_LEN = 100;

//#pragma pack (push, 1)


constexpr USHORT CS_LOGIN = 1;
constexpr USHORT CS_PlayerData = 2;
constexpr USHORT CS_GameStart = 3;
constexpr USHORT CS_Attack = 4;
constexpr USHORT CS_InGame = 5;
constexpr USHORT CS_Item_Activate = 6;

constexpr USHORT SC_PlayerPosi = 101;
constexpr USHORT SC_First_PlayerPosi = 102;		//플레이어들 초기좌표값
constexpr USHORT SC_Item_Activate = 103;

struct cs_Login {
	unsigned short size;
	unsigned short type;
	char ID[20];


	//메인 캐릭터 서브캐릭터 설정
	int main_charc;
	int sub_charc;
};

struct cs_GameStart {
	unsigned short size;
	unsigned short type;
	bool is_Start;
};

struct cs_PlayerMove {
	unsigned short size;
	unsigned short type;
	char ID[20];

	int main_behavior_var;
	float main_pos_x;
	float main_pos_z;
	float main_rot_y;
	short main_hp;
	short main_mp;

	int sub_behavior_var;
	float sub_pos_x;
	float sub_pos_z;
	float sub_rot_y;
	short sub_hp;
	short sub_mp;

	short is_main_ch;

};

struct cs_Attack {
	unsigned short size;
	unsigned short type;

	char target_id[20];
	short damage;
};

struct sc_player_posi {
	unsigned short size;
	unsigned short type;

	char p1_ID[20];
	int p1_main_behavior;
	float p1_main_pos_x;
	float p1_main_pos_z;
	float p1_main_rot_y;
	short p1_main_hp;
	short p1_main_mp;
	int p1_sub_behavior;
	float p1_sub_pos_x;
	float p1_sub_pos_z;
	float p1_sub_rot_y;
	short p1_sub_hp;
	short p1_sub_mp;

	char p2_ID[20];
	int p2_main_behavior;
	float p2_main_pos_x;
	float p2_main_pos_z;
	float p2_main_rot_y;
	short p2_main_hp;
	short p2_main_mp;
	int p2_sub_behavior;
	float p2_sub_pos_x;
	float p2_sub_pos_z;
	float p2_sub_rot_y;
	short p2_sub_hp;
	short p2_sub_mp;

	char p3_ID[20];
	int p3_main_behavior;
	float p3_main_pos_x;
	float p3_main_pos_z;
	float p3_main_rot_y;
	short p3_main_hp;
	short p3_main_mp;
	int p3_sub_behavior;
	float p3_sub_pos_x;
	float p3_sub_pos_z;
	float p3_sub_rot_y;
	short p3_sub_hp;
	short p3_sub_mp;

	char p4_ID[20];
	int p4_main_behavior;
	float p4_main_pos_x;
	float p4_main_pos_z;
	float p4_main_rot_y;
	short p4_main_hp;
	short p4_main_mp;
	int p4_sub_behavior;
	float p4_sub_pos_x;
	float p4_sub_pos_z;
	float p4_sub_rot_y;
	short p4_sub_hp;
	short p4_sub_mp;

	short p1_is_main_ch;
	short p2_is_main_ch;
	short p3_is_main_ch;
	short p4_is_main_ch;
};

struct All_Item {
	unsigned short size;
	unsigned short type;

	short item;
	bool activate;
};

//#pragma pack (pop)
