using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCollisionManager : MonoBehaviour
{
    void send_Attack_packet(short damage)
    {
        cs_Attack AttackPacket = new cs_Attack();
        AttackPacket.Player_ID = "";
        AttackPacket.damage = damage;

        NetworkManager.instance.Send(AttackPacket.Write());
    }
    public void KarmenBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "KarmenAttack")
        {
            Debug.Log("Hit: 카르멘 기본공격");
        }
    }

    public void KarmenQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "KarmenQSkill")
        {
            Debug.Log("Hit: 카르멘 Q스킬");
        }
    }

    public void KarmenWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "KarmenWSkill")
        {
            Debug.Log("Hit: 카르멘 W스킬");
        }
    }

    public void JadeBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "JadeAttack")
        {
            Debug.Log("Hit: 제이드 기본공격");
        }
    }

    public void JadeQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "JadeQSkill")
        {
            Debug.Log("Hit: 제이드 Q스킬");
        }
    }

    public void JadeWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "JadeWSkill")
        { 
            Debug.Log("Hit: 제이드 W스킬");
        }

    }

    public void LeinaBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "LeinaAttack")
        {
            Debug.Log("Hit: 레이나 기본공격");
        }
    }

    public void LeinaQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "LeinaQSkill")
        {
            Debug.Log("Hit: 레이나 Q스킬");
        }
    }

    public void LeinaWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "LeinaWSkill")
        {
            Debug.Log("Hit: 레이나 W스킬");
        }
    }

    public void EvaBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "EvaAttack")
        {
            Debug.Log("Hit: 에바 기본공격");
        }
    }

    public void EvaQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "EvaQSkill")
        {
            Debug.Log("Hit: 에바 Q스킬");
        }
    }

    public void EvaWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "EvaWSkill")
        {
            Debug.Log("Hit: 에바 W스킬");
        }
    }
}
