using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagInfoUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Canvas (UI)").GetComponent<InGameUI>().ExplanManipulationTag();
        gameObject.SetActive(false);
    }
}