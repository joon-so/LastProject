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
        Debug.Log("--------------------------------");
        //Debug.Log(OtherPlayers[0]);

        //for (int i=0; i< OtherPlayers.Count;++i)
        //{
        //mainPlayers[0] = OtherPlayers[0].GetComponent<ServerOtherPlayerManager>().mainObj;
        //}
    
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
        //for (int i = 0; i < mainPlayers.Count; ++i)
        //{
        //    mainPlayers[i] = OtherPlayers[i].GetComponent<ServerOtherPlayerManager>().mainObj;
        //}
    }
}
