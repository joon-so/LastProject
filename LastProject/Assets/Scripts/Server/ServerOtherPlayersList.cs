using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherPlayersList : MonoBehaviour
{
    public List<GameObject> OtherPlayers;
    public List<GameObject> mainPlayers = new List<GameObject>();

    bool addObj;
    void Start()
    {
        addObj = false;
        OtherPlayers = GameObject.Find("ServerIngameManager").GetComponent<ServerIngameManager>().otherPlayerList;
    }

    void FixedUpdate()
    {
        if (!addObj)
        {
            for (int i = 0; i < OtherPlayers.Count; ++i)
            {
                mainPlayers.Add(OtherPlayers[i].GetComponent<ServerOtherPlayerManager>().mainObj);
            }
            addObj = true;
        }
        else
        {
            for (int i = 0; i < mainPlayers.Count; ++i)
            {
                mainPlayers[i] = OtherPlayers[i].GetComponent<ServerOtherPlayerManager>().mainObj;
            }
        }
    }
}
