using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfoUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Canvas (UI)").GetComponent<InGameUI>().ExplanManipulationAttack();
        gameObject.SetActive(false);
    }
}
