using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeInfoUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("MainCharacter"))
        {
            GameObject.Find("Canvas (UI)").GetComponent<InGameUI>().ExplanManipulationDodge();
            gameObject.SetActive(false);
        }
    }
}
