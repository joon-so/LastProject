using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEpPotion : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        // ȸ��
    }

    private void OnCollisionEnter(Collision collision)
    {
        send_Item_packet();
        Destroy(gameObject);
        ServerItemManager.instance.onItem = false;
    }

    void send_Item_packet()
    {
        cs_Item ItemPacket = new cs_Item();
        ItemPacket.item = 1;
        ItemPacket.activate = false;

        NetworkManager.instance.Send(ItemPacket.Write());
    }
}