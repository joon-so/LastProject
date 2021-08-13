using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveManager : MonoBehaviour
{
    void Awake()
    {
        GameObject.Find("GameManager").SetActive(false);
        GameObject.Find("Canvas (UI)").SetActive(false);
        GameObject.Find("Player").SetActive(false);
    }
}
