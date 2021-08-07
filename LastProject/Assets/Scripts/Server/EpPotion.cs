using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpPotion : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        // È¸Àü
    }

    private void OnCollisionEnter(Collision collision)
    {
        send_Item_packet();
        Destroy(gameObject);
    }

    void send_Item_packet()
    {
        cs_Item ItemPacket = new cs_Item();
        ItemPacket.item = 1;
        ItemPacket.activate = false;

        NetworkManager.instance.Send(ItemPacket.Write());
    }
}