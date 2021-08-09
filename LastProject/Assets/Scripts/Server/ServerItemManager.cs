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

    public bool onItem;

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
        if (is_Item_Active == true)
        {
            if (kindOfItem == 1)
            {
                if (onItem == false)
                    CreateHpPotion();
            }
            else if (kindOfItem == 2)
            {
                if (onItem == false)
                    CreateEpPotion();
            }
        }
    }

    void CreateHpPotion()
    {
        onItem = true;
        Debug.Log("HP 持失");
        Instantiate(hpPotion, itemCreatePos.position, itemCreatePos.rotation);
    }

    void CreateEpPotion()
    {
        onItem = true;
        Debug.Log("MP 持失");
        Instantiate(epPotion, itemCreatePos.position, itemCreatePos.rotation);
    }
}
