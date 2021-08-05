using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherPlayersList : MonoBehaviour
{
    public List<GameObject> OtherPlayers = new List<GameObject>();
    public List<GameObject> childPlayers = new List<GameObject>();

    void Start()
    {
        childPlayers = GameObject.Find("ServerIngameManager").GetComponent<ServerIngameManager>().player;
        //Debug.Log(childPlayers[0]);
        //Debug.Log(childPlayers[1]);
    }

    void FixedUpdate()
    {
        for (int i = 1; i < childPlayers.Count; ++i)
        {
            OtherPlayers.Add(childPlayers[i].GetComponent<ServerOtherPlayerManager>().mainObj);
        }
    }
}
