using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerItemManager : MonoBehaviour
{
    public static ServerItemManager instance;

    public bool is_Item_Active;
    public short kindOfItem;

    public short hpValue;
    public short epValue;

    [SerializeField] GameObject hpPotion;
    [SerializeField] GameObject epPotion;
    [SerializeField] Transform itemCreatePos;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        CreateHpPotion();
    }

    void Update()
    {
        if(is_Item_Active)
        {
            if(kindOfItem == 1)
                CreateHpPotion();
            else if (kindOfItem == 2)
                CreateEpPotion();
        }
    }

    void CreateHpPotion()
    {
        Instantiate(hpPotion, itemCreatePos.position, itemCreatePos.rotation);
    }

    void CreateEpPotion()
    {
        Instantiate(epPotion, itemCreatePos.position, itemCreatePos.rotation);
    }
}
