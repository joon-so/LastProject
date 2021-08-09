using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerItemManager : MonoBehaviour
{
    public static ServerItemManager instance;

    public bool is_Item_Active;
    public bool createItem;
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
        is_Item_Active = true;
        Instantiate(hpPotion, itemCreatePos.position, itemCreatePos.rotation);
    }

    void LateUpdate()
    {
        if (onItem)
            StartCoroutine(CreatePotion());
    }


    IEnumerator CreatePotion()
    {
        if (kindOfItem == 1)
        {
            is_Item_Active = true;
            onItem = false;
            Instantiate(hpPotion, itemCreatePos.position, itemCreatePos.rotation);
        }
        else if (kindOfItem == 2)
        {
            is_Item_Active = true;
            onItem = false;
            Instantiate(epPotion, itemCreatePos.position, itemCreatePos.rotation);
        }
        yield return null;
    }
}
