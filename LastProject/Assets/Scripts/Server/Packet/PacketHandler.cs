using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



class PacketHandler
{

	public static void sc_playerPosiGameHandler(PacketSession session, IPacket packet)
	{
		sc_PlayerPosi pkt = packet as sc_PlayerPosi;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.sc_playerPosi_DO(pkt);
	}
	public static void sc_playerFirstPosiGameHandler(PacketSession session, IPacket packet)
	{
		sc_First_PlayerPosi pkt = packet as sc_First_PlayerPosi;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.sc_playerFirstPosi_DO(pkt);
	}

	public static void cs_LoginGameHandler(PacketSession session, IPacket packet)
	{
		cs_Login pkt = packet as cs_Login;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.cs_Login_Process(pkt);
	}
	public static void cs_PlayerMoveGameHandler(PacketSession session, IPacket packet)
	{
		cs_PlayerData pkt = packet as cs_PlayerData;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.cs_PlayerData_Process(pkt);
	}

	public static void cs_GameStartHandler(PacketSession session, IPacket packet)
	{
		cs_GameStart pkt = packet as cs_GameStart;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.cs_GameStart_Process(pkt);
	}

	public static void cs_AttackHandler(PacketSession session, IPacket packet)
	{
		cs_Attack pkt = packet as cs_Attack;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.cs_Attack_Process(pkt);
	}

	public static void cs_InGameStartHandler(PacketSession session, IPacket packet)
	{
		cs_InGameStart pkt = packet as cs_InGameStart;
		ServerSession serverSession = session as ServerSession;

		ServerToClientManager.Instance.cs_InGameStart_Process(pkt);
	}
}