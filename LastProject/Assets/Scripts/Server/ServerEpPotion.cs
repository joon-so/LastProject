using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEpPotion : MonoBehaviour
{
    private bool check = false;

    void Update()
    {
        if (ServerItemManager.instance.is_Item_Active == false)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainCharacter"))
        {
            if (!check)
            {
                Destroy(gameObject);
                send_Item_packet();
                ServerItemManager.instance.onItem = false;
                ServerMyPlayerManager.instance.myEpPotionCount += 1;
                check = true;
            }
        }
    }

    void send_Item_packet()
    {
        cs_Item ItemPacket = new cs_Item();
        ItemPacket.item = 1;
        ItemPacket.activate = false;

        NetworkManager.instance.Send(ItemPacket.Write());
    }
}