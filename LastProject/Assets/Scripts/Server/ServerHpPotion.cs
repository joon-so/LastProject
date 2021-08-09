using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHpPotion : MonoBehaviour
{
    private bool check = false;

    void Update()
    {
        Debug.Log("HP: " + ServerItemManager.instance.is_Item_Active);
        if (ServerItemManager.instance.is_Item_Active == false)
            Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainCharacter"))
        {
            if (!check)
            {
                check = true;
                Destroy(gameObject);
                send_Item_packet();
                ServerItemManager.instance.onItem = false;
                ServerMyPlayerManager.instance.myHpPotionCount += 1;
            }
        }
    }

    void send_Item_packet()
    {
        cs_Item ItemPacket = new cs_Item();
        ItemPacket.item = 2; 
        ItemPacket.activate = false;

        NetworkManager.instance.Send(ItemPacket.Write());
    }
}
