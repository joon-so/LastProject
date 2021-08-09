using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHpPotion : MonoBehaviour
{
    private bool check = false;

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCharacter"))
        {
            if (!check)
            {
                check = true;
                Destroy(gameObject);
                Debug.Log("�Ծ���.");
                ServerItemManager.instance.onItem = false;
                ServerMyPlayerManager.instance.myHpPotionCount += 1;
                send_Item_packet();
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("MainCharacter"))
    //    {
    //        if(!check)
    //        {
    //            Destroy(gameObject);
    //            Debug.Log("�Ծ���.");
    //            send_Item_packet();
    //            ServerItemManager.instance.onItem = false;
    //            ServerMyPlayerManager.instance.myHpPotionCount += 1;
    //            check = true;
    //        }
    //    }
    //}

    void send_Item_packet()
    {
        Debug.Log("�Ծ���.");
        cs_Item ItemPacket = new cs_Item();
        ItemPacket.item = 2; 
        ItemPacket.activate = false;

        NetworkManager.instance.Send(ItemPacket.Write());
    }
}
