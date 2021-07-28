using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	public static NetworkManager instance;

	ServerSession _session = new ServerSession();

	public void Send(ArraySegment<byte> sendBuff)
	{
		_session.Send(sendBuff);
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		DontDestroyOnLoad(gameObject);
	}


	void Start()
	{
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = IPAddress.Parse(ServerLoginManager.playerList[0].ipAdress); //string 
		IPEndPoint endPoint = new IPEndPoint(ipAddr, 9090);

		Connector connector = new Connector();

		connector.Connect(endPoint,
			() => { return _session; },
			1);
	}

	void Update()
	{
		List<IPacket> list = PacketQueue.Instance.PopAll();
		foreach (IPacket packet in list)
			PacketManager.Instance.HandlePacket(_session, packet);
	}
}
