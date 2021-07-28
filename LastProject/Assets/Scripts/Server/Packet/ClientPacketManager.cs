using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();

	public void Register()
	{
		//Client -> Server
		_makeFunc.Add((ushort)PacketID.CS_Login, MakePacket<cs_Login>);
		_handler.Add((ushort)PacketID.CS_Login, PacketHandler.cs_LoginGameHandler);

		_makeFunc.Add((ushort)PacketID.CS_PlayerData, MakePacket<cs_PlayerData>);
		_handler.Add((ushort)PacketID.CS_PlayerData, PacketHandler.cs_PlayerMoveGameHandler);

		_makeFunc.Add((ushort)PacketID.CS_GameStart, MakePacket<cs_GameStart>);
		_handler.Add((ushort)PacketID.CS_GameStart, PacketHandler.cs_GameStartHandler);


		//Server -> Client
		_makeFunc.Add((ushort)PacketID.SC_PlayerPosi, MakePacket<sc_PlayerPosi>);
		_handler.Add((ushort)PacketID.SC_PlayerPosi, PacketHandler.sc_playerPosiGameHandler);

		_makeFunc.Add((ushort)PacketID.SC_First_PlayerPosi, MakePacket<sc_First_PlayerPosi>);
		_handler.Add((ushort)PacketID.SC_First_PlayerPosi, PacketHandler.sc_playerFirstPosiGameHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			if (onRecvCallback != null)
				onRecvCallback.Invoke(session, packet);
			else
				HandlePacket(session, packet);
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);
		return pkt;
	}

	public void HandlePacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action.Invoke(session, packet);
	}
}