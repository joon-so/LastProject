using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHpPotion : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("MainCharacter"))
        {
            Debug.Log("¸Ô¾ú´Ù");
            send_Item_packet();
            Destroy(gameObject);
            ServerItemManager.instance.onItem = false;
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
