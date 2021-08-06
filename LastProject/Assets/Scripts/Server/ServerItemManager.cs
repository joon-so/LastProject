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
        // if (������ �����ð�)
        {
            CreateHpPotion();
        }
        // if (������ �����ð�)
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
