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

	SC_PlayerPosi = 101
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

public class cs_PlayerData : IPacket
{
	public string ID;
	public int behavior_var;
	public float player_pos_x;
	public float player_pos_z;
	public float player_rot_y;

	public ushort Protocol { get { return (ushort)PacketID.CS_PlayerData; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);
		this.ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.behavior_var = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.player_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.player_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.player_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
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
		Array.Copy(BitConverter.GetBytes(this.behavior_var), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.player_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.player_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.player_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);


		Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

		return SendBufferHelper.Close(count);
	}
}

public class sc_PlayerPosi : IPacket
{
	public string p1_ID;
	public int p1_behavior;
	public float p1_pos_x;
	public float p1_pos_z;
	public float p1_rot_y;

	public string p2_ID;
	public int p2_behavior;
	public float p2_pos_x;
	public float p2_pos_z;
	public float p2_rot_y;

	public string p3_ID;
	public int p3_behavior;
	public float p3_pos_x;
	public float p3_pos_z;
	public float p3_rot_y;

	public string p4_ID;
	public int p4_behavior;
	public float p4_pos_x;
	public float p4_pos_z;
	public float p4_rot_y;

	public ushort Protocol { get { return (ushort)PacketID.SC_PlayerPosi; } }

	public void Read(ArraySegment<byte> segment)
	{
		ushort count = 0;
		count += sizeof(ushort);
		count += sizeof(ushort);

		this.p1_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p1_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p1_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p1_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);

		this.p2_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p2_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p2_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p2_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);

		this.p3_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p3_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p3_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p3_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);

		this.p4_ID = Encoding.UTF8.GetString(segment.Array, segment.Offset + count, 20);
		count += 20;
		this.p4_behavior = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.p4_pos_x = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_pos_z = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.p4_rot_y = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);

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
		Array.Copy(BitConverter.GetBytes(this.p1_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p1_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p1_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);

		byte[] rawUnicode1 = Encoding.UTF8.GetBytes(this.p2_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p2_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p2_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p2_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p2_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);

		byte[] rawUnicode2 = Encoding.UTF8.GetBytes(this.p3_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p3_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p3_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p3_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p3_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);

		byte[] rawUnicode3 = Encoding.UTF8.GetBytes(this.p4_ID);
		Array.Copy(rawUnicode1, 0, segment.Array, segment.Offset + count, this.p4_ID.Length);
		count += 20;
		Array.Copy(BitConverter.GetBytes(this.p4_behavior), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(this.p4_pos_x), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_pos_z), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(this.p4_rot_y), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);

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


