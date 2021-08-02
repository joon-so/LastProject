using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCollisionManager : MonoBehaviour
{
    [SerializeField] short karmenAttackDamage;
    [SerializeField] short karmenQSkillDamage;
    [SerializeField] short karmenWSkillDamage;
    [SerializeField] short JadeAttackDamage;
    [SerializeField] short JadeQSkillDamage;
    [SerializeField] short JadeWSkillDamage;
    [SerializeField] short LeinaAttackDamage;
    [SerializeField] short LeinaQSkillDamage;
    [SerializeField] short LeinaWSkillDamage;
    [SerializeField] short EvaAttackDamage;
    [SerializeField] short EvaQSkillDamage;
    [SerializeField] short EvaWSkillDamage;

    void send_Attack_packet(short damage)
    {
        cs_Attack AttackPacket = new cs_Attack();
        AttackPacket.Player_ID = ServerLoginManager.playerList[0].playerID;
        AttackPacket.damage = damage;

        NetworkManager.instance.Send(AttackPacket.Write());
    }
    public void KarmenBasicAttack()
    {
        send_Attack_packet(karmenAttackDamage);
        Debug.Log("Hit: 카르멘 기본공격");
    }
    public void KarmenQSkillAttack()
    {
        send_Attack_packet(karmenQSkillDamage);
        Debug.Log("Hit: 카르멘 Q스킬");
    }
    public void KarmenWSkillAttack()
    {
        send_Attack_packet(karmenWSkillDamage);
        Debug.Log("Hit: 카르멘 W스킬");
    }
    public void JadeBasicAttack()
    {
        send_Attack_packet(JadeAttackDamage);
        Debug.Log("Hit: 제이드 기본공격");
    }
    public void JadeQSkillAttack()
    {
        send_Attack_packet(JadeQSkillDamage);
        Debug.Log("Hit: 제이드 Q스킬");
    }
    public void JadeWSkillAttack()
    {
        send_Attack_packet(JadeWSkillDamage);
        Debug.Log("Hit: 제이드 W스킬");
    }
    public void LeinaBasicAttack()
    {
        send_Attack_packet(LeinaAttackDamage);
        Debug.Log("Hit: 레이나 기본공격");
    }
    public void LeinaQSkillAttack()
    {
        send_Attack_packet(LeinaQSkillDamage);
        Debug.Log("Hit: 레이나 Q스킬");
    }
    public void LeinaWSkillAttack()
    {
        send_Attack_packet(LeinaWSkillDamage);
        Debug.Log("Hit: 레이나 W스킬");
    }
    public void EvaBasicAttack()
    {
        send_Attack_packet(EvaAttackDamage);
        Debug.Log("Hit: 에바 기본공격");
    }

    public void EvaQSkillAttack()
    {
        send_Attack_packet(EvaQSkillDamage);
        Debug.Log("Hit: 에바 Q스킬");
    }

    public void EvaWSkillAttack()
    {
        send_Attack_packet(EvaWSkillDamage);
        Debug.Log("Hit: 에바 W스킬");
    }
}