using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoUI : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCharacter"))
        {
            GameObject.Find("Canvas (UI)").GetComponent<InGameUI>().ExplanManipulationSkill();
            gameObject.SetActive(false);
        }
    }
}
