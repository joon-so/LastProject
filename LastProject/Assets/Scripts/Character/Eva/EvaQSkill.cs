using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaQSkill : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("º¸½º ºÒ½÷");
        if (other.gameObject.CompareTag("Enemy1"))
        {
            other.gameObject.GetComponent<Enemy1>().HitEvaQSkill();
        }
        if (other.gameObject.CompareTag("Enemy2"))
        {
            other.gameObject.GetComponent<Enemy2>().HitEvaQSkill();
        }
        if (other.gameObject.CompareTag("Enemy3"))
        {
            other.gameObject.GetComponent<Enemy3>().HitEvaQSkill();
        }
        if (other.gameObject.CompareTag("Enemy6"))
        {
            other.gameObject.GetComponent<Enemy6>().HitEvaQSkill();
        }
        if (other.gameObject.CompareTag("Pilot"))
        {
            other.gameObject.GetComponent<BossPilot>().HitEvaQSkill();
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<Boss>().HitEvaQSkill();
        }
    }
}
