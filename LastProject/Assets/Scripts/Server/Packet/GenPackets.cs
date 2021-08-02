using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;


using UnityEngine;
public enum PacketID
{
	CS_Login = 1,
	CS_PlayerData = 2,
	CS_GameStart = 3,
	CS_Attack = 4,
	CS_InGame = 5,

	SC_PlayerPosi = 101,
	SC_First_PlayerPosi = 102
}

public interface IPacket
{
	ushort Protocol { get; }
	void Read(ArraySegment<byte> segment);
	ArraySegment<byte> Write();
}


//Client -> Server
public class cs_Login : IPacket
{
	public string Player_ID;
	public int main_charc;
	public int sub_charc;


	public ushort Protocol { get { return (ushort)PacketID.CS_Login; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.Player_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.main_charc = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.sub_charc = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.CS_Login), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		byte[] rawUnicode = Encoding.UTF8.GetBytes(this.Player_ID);
		Array.Copy(rawUnicode, 0, segment.Array, segment.Offset + count, this.Player_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.main_charc), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.sub_charc), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);

		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class cs_GameStart : IPacket
{
	public bool is_Start;

	public ushort Protocol { get { return (ushort)PacketID.CS_GameStart; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.is_Start = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);

	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.CS_GameStart), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.is_Start), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);

		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class cs_InGameStart : IPacket
{
	public bool is_Start;

	public ushort Protocol { get { return (ushort)PacketID.CS_InGame; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.is_Start = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
		count += sizeof(bool);

	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.CS_InGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes(this.is_Start), 0, segment.Array, segment.Offset + count, sizeof(bool));
		count += sizeof(bool);

		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class cs_Attack : IPacket
{
	public string Player_ID;
	public short damage;

	public ushort Protocol { get { return (ushort)PacketID.CS_Attack; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.Player_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.damage = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.CS_Attack), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		byte[] rawUnicode = Encoding.UTF8.GetBytes(this.Player_ID);
		Array.Copy(rawUnicode, 0, segment.Array, segment.Offset + count, this.Player_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.damage), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);


		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class cs_PlayerData : IPacket
{
	public string ID;

	public int mainPlayer_Behavior;
	public float mainPlayer_Pos_X;
	public float mainPlayer_Pos_Z;
	public float mainPlayer_Rot_Y;
	public short mainPlayer_Hp;
	public short mainPlayer_Mp;

	public int subPlayer_Behavior;
	public float subPlayer_Pos_X;
	public float subPlayer_Pos_Z;
	public float subPlayer_Rot_Y;
	public short subPlayer_Hp;
	public short subPlayer_Mp;

	public short is_Main_Ch;  //메인 캐릭터 확인

	public ushort Protocol { get { return (ushort)PacketID.CS_PlayerData; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.mainPlayer_Behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.mainPlayer_Pos_X = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.mainPlayer_Pos_Z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.mainPlayer_Rot_Y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.mainPlayer_Hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.mainPlayer_Mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.subPlayer_Behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.subPlayer_Pos_X = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.subPlayer_Pos_Z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.subPlayer_Rot_Y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.subPlayer_Hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.subPlayer_Mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.is_Main_Ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.CS_PlayerData), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		byte[] rawUnicode = Encoding.UTF8.GetBytes(this.ID);
		Array.Copy(rawUnicode, 0, segment.Array, segment.Offset + count, this.ID.Length);
		count += 20;

		Array.Copy(BitConverter.GetBytes(this.mainPlayer_Behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.mainPlayer_Pos_X), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.mainPlayer_Pos_Z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.mainPlayer_Rot_Y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.mainPlayer_Hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.mainPlayer_Mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		Array.Copy(BitConverter.GetBytes(this.subPlayer_Behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.subPlayer_Pos_X), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.subPlayer_Pos_Z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.subPlayer_Rot_Y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.subPlayer_Hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.subPlayer_Mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		Array.Copy(BitConverter.GetBytes(this.is_Main_Ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);


		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class sc_PlayerPosi : IPacket
{
	public string p1_ID;
	public int p1_main_behavior;
	public float p1_main_pos_x;
	public float p1_main_pos_z;
	public float p1_main_rot_y;
	public short p1_main_hp;
	public short p1_main_mp;
	public int p1_sub_behavior;
	public float p1_sub_pos_x;
	public float p1_sub_pos_z;
	public float p1_sub_rot_y;
	public short p1_sub_hp;
	public short p1_sub_mp;

	public string p2_ID;
	public int p2_main_behavior;
	public float p2_main_pos_x;
	public float p2_main_pos_z;
	public float p2_main_rot_y;
	public short p2_main_hp;
	public short p2_main_mp;
	public int p2_sub_behavior;
	public float p2_sub_pos_x;
	public float p2_sub_pos_z;
	public float p2_sub_rot_y;
	public short p2_sub_hp;
	public short p2_sub_mp;

	public string p3_ID;
	public int p3_main_behavior;
	public float p3_main_pos_x;
	public float p3_main_pos_z;
	public float p3_main_rot_y;
	public short p3_main_hp;
	public short p3_main_mp;
	public int p3_sub_behavior;
	public float p3_sub_pos_x;
	public float p3_sub_pos_z;
	public float p3_sub_rot_y;
	public short p3_sub_hp;
	public short p3_sub_mp;

	public string p4_ID;
	public int p4_main_behavior;
	public float p4_main_pos_x;
	public float p4_main_pos_z;
	public float p4_main_rot_y;
	public short p4_main_hp;
	public short p4_main_mp;
	public int p4_sub_behavior;
	public float p4_sub_pos_x;
	public float p4_sub_pos_z;
	public float p4_sub_rot_y;
	public short p4_sub_hp;
	public short p4_sub_mp;

	public short p1_is_main_ch;
	public short p2_is_main_ch;
	public short p3_is_main_ch;
	public short p4_is_main_ch;

	public ushort Protocol { get { return (ushort)PacketID.SC_PlayerPosi; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.p1_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p1_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p1_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p1_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p1_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p1_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p1_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);


		this.p2_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p2_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p2_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p2_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.p3_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p3_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p3_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p3_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.p4_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p4_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p4_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p4_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.p1_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.SC_PlayerPosi), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);

		byte[] rawUnicode = Encoding.UTF8.GetBytes(this.p1_ID);
		Array.Copy(rawUnicode, 0, segment.Array, segment.Offset + count, this.p1_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p1_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p1_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p1_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		byte[] rawUnicode1 = Encoding.UTF8.GetBytes(this.p2_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p2_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p2_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p2_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		byte[] rawUnicode2 = Encoding.UTF8.GetBytes(this.p3_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p3_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p3_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p3_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		byte[] rawUnicode3 = Encoding.UTF8.GetBytes(this.p4_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p4_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p4_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p4_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		Array.Copy(BitConverter.GetBytes(this.p1_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class sc_First_PlayerPosi : IPacket
{
	public string p1_ID;
	public int p1_main_behavior;
	public float p1_main_pos_x;
	public float p1_main_pos_z;
	public float p1_main_rot_y;
	public short p1_main_hp;
	public short p1_main_mp;
	public int p1_sub_behavior;
	public float p1_sub_pos_x;
	public float p1_sub_pos_z;
	public float p1_sub_rot_y;
	public short p1_sub_hp;
	public short p1_sub_mp;

	public string p2_ID;
	public int p2_main_behavior;
	public float p2_main_pos_x;
	public float p2_main_pos_z;
	public float p2_main_rot_y;
	public short p2_main_hp;
	public short p2_main_mp;
	public int p2_sub_behavior;
	public float p2_sub_pos_x;
	public float p2_sub_pos_z;
	public float p2_sub_rot_y;
	public short p2_sub_hp;
	public short p2_sub_mp;

	public string p3_ID;
	public int p3_main_behavior;
	public float p3_main_pos_x;
	public float p3_main_pos_z;
	public float p3_main_rot_y;
	public short p3_main_hp;
	public short p3_main_mp;
	public int p3_sub_behavior;
	public float p3_sub_pos_x;
	public float p3_sub_pos_z;
	public float p3_sub_rot_y;
	public short p3_sub_hp;
	public short p3_sub_mp;

	public string p4_ID;
	public int p4_main_behavior;
	public float p4_main_pos_x;
	public float p4_main_pos_z;
	public float p4_main_rot_y;
	public short p4_main_hp;
	public short p4_main_mp;
	public int p4_sub_behavior;
	public float p4_sub_pos_x;
	public float p4_sub_pos_z;
	public float p4_sub_rot_y;
	public short p4_sub_hp;
	public short p4_sub_mp;

	public short p1_is_main_ch;
	public short p2_is_main_ch;
	public short p3_is_main_ch;
	public short p4_is_main_ch;

	public ushort Protocol { get { return (ushort)PacketID.SC_First_PlayerPosi; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.p1_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p1_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p1_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p1_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p1_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p1_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p1_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);


		this.p2_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p2_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p2_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p2_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.p3_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p3_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p3_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p3_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.p4_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p4_main_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p4_main_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_main_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_main_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_main_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_main_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_sub_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p4_sub_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_sub_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_sub_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_sub_hp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_sub_mp = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

		this.p1_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p2_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p3_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);
		this.p4_is_main_ch = BitConverter.ToInt16(segment.Array, segment.Offset + count);
		count += sizeof(short);

	}

	public ArraySegment<byte> Write()
	{
		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
		ushort count = 0;

		count += sizeof(ushort);
		Array.Copy(BitConverter.GetBytes((ushort)PacketID.SC_First_PlayerPosi), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);

		byte[] rawUnicode = Encoding.UTF8.GetBytes(this.p1_ID);
		Array.Copy(rawUnicode, 0, segment.Array, segment.Offset + count, this.p1_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p1_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p1_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p1_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p1_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		byte[] rawUnicode1 = Encoding.UTF8.GetBytes(this.p2_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p2_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p2_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p2_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		byte[] rawUnicode2 = Encoding.UTF8.GetBytes(this.p3_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p3_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p3_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p3_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		byte[] rawUnicode3 = Encoding.UTF8.GetBytes(this.p4_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p4_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p4_main_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p4_main_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_main_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_main_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_main_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_main_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_hp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_sub_mp), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		Array.Copy(BitConverter.GetBytes(this.p1_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p2_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p3_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);
		Array.Copy(BitConverter.GetBytes(this.p4_is_main_ch), 0, segment.Array, segment.Offset + count, sizeof(short));
		count += sizeof(short);

		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}


//public class S_BroadcastEnterGame : IPacket
//{
//	public int playerId;
//	public float posX;
//	public float posY;
//	public float posZ;

//	public ushort Protocol { get { return (ushort)PacketID.S_BroadcastEnterGame; } }

//	public void Read(ArraySegment<byte> segment)
//	{
//		ushort count = 0;
//		count += sizeof(ushort);
//		count += sizeof(ushort);
//		this.playerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
//		count += sizeof(int);
//		this.posX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
//		count += sizeof(float);
//		this.posY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
//		count += sizeof(float);
//		this.posZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
//		count += sizeof(float);
//	}

//	public ArraySegment<byte> Write()
//	{
//		ArraySegment<byte> segment = SendBufferHelper.Open(4096);
//		ushort count = 0;

//		count += sizeof(ushort);
//		Array.Copy(BitConverter.GetBytes((ushort)PacketID.S_BroadcastEnterGame), 0, segment.Array, segment.Offset + count, sizeof(ushort));
//		count += sizeof(ushort);
//		Array.Copy(BitConverter.GetBytes(this.playerId), 0, segment.Array, segment.Offset + count, sizeof(int));
//		count += sizeof(int);
//		Array.Copy(BitConverter.GetBytes(this.posX), 0, segment.Array, segment.Offset + count, sizeof(float));
//		count += sizeof(float);
//		Array.Copy(BitConverter.GetBytes(this.posY), 0, segment.Array, segment.Offset + count, sizeof(float));
//		count += sizeof(float);
//		Array.Copy(BitConverter.GetBytes(this.posZ), 0, segment.Array, segment.Offset + count, sizeof(float));
//		count += sizeof(float);

//		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

//		return SendBufferHelper.Close(count);
//	}
//}


