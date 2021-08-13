using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveManager2 : MonoBehaviour
{
    void Awake()
    {
        GameObject.Find("GameManager").SetActive(true);
        GameObject.Find("Canvas (UI)").SetActive(true);
        GameObject.Find("Player").SetActive(true);
        Debug.Log(GameObject.Find("GameManager"));
    }
}
