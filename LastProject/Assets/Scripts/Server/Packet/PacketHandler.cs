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

		ServerPlayerManager.Instance.sc_playerPosi_DO(pkt);
	}
	public static void cs_LoginGameHandler(PacketSession session, IPacket packet)
	{
		cs_Login pkt = packet as cs_Login;
		ServerSession serverSession = session as ServerSession;

		ServerPlayerManager.Instance.cs_Login_Process(pkt);
	}
	public static void cs_PlayerMoveGameHandler(PacketSession session, IPacket packet)
	{
		cs_PlayerData pkt = packet as cs_PlayerData;
		ServerSession serverSession = session as ServerSession;

		ServerPlayerManager.Instance.please(pkt);
	}
}