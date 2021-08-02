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
            Debug.Log("Hit: ī���� �⺻����");
        }
    }

    public void KarmenQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "KarmenQSkill")
        {
            Debug.Log("Hit: ī���� Q��ų");
        }
    }

    public void KarmenWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "KarmenWSkill")
        {
            Debug.Log("Hit: ī���� W��ų");
        }
    }

    public void JadeBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "JadeAttack")
        {
            Debug.Log("Hit: ���̵� �⺻����");
        }
    }

    public void JadeQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "JadeQSkill")
        {
            Debug.Log("Hit: ���̵� Q��ų");
        }
    }

    public void JadeWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "JadeWSkill")
        { 
            Debug.Log("Hit: ���̵� W��ų");
        }

    }

    public void LeinaBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "LeinaAttack")
        {
            Debug.Log("Hit: ���̳� �⺻����");
        }
    }

    public void LeinaQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "LeinaQSkill")
        {
            Debug.Log("Hit: ���̳� Q��ų");
        }
    }

    public void LeinaWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "LeinaWSkill")
        {
            Debug.Log("Hit: ���̳� W��ų");
        }
    }

    public void EvaBasicAttack(Collision collision)
    {
        if (collision.gameObject.tag == "EvaAttack")
        {
            Debug.Log("Hit: ���� �⺻����");
        }
    }

    public void EvaQSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "EvaQSkill")
        {
            Debug.Log("Hit: ���� Q��ų");
        }
    }

    public void EvaWSkillAttack(Collision collision)
    {
        if (collision.gameObject.tag == "EvaWSkill")
        {
            Debug.Log("Hit: ���� W��ų");
        }
    }
}
