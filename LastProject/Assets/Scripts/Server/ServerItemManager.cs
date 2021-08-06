using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerItemManager : MonoBehaviour
{
    [SerializeField] GameObject hpPotion;
    [SerializeField] Transform createHpPotionPosition;
    
    [SerializeField] GameObject epPotion;
    [SerializeField] Transform createEpPotionPosition;

    void Start()
    {
        
    }

    void Update()
    {
        // if (아이템 생성시간)
        {
            CreateHpPotion();
        }
        // if (아이템 생성시간)
        {
            CreateEpPotion();
        }
    }

    void CreateHpPotion()
    {
        Instantiate(hpPotion, createHpPotionPosition.position, createHpPotionPosition.rotation);
    }

    void CreateEpPotion()
    {
        Instantiate(epPotion, createEpPotionPosition.position, createEpPotionPosition.rotation);
    }
}
