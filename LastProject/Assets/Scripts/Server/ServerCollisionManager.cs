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
        if (ServerLoginManager.playerList[0].is_Main_Character == 1)
        {
            ServerLoginManager.playerList[0].character1Hp -= damage;
            if (ServerLoginManager.playerList[0].character1Hp <= 0)
                ServerLoginManager.playerList[0].character1Hp = 0;
            AttackPacket.damage = ServerLoginManager.playerList[0].character1Hp;
        }
        else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
        {
            ServerLoginManager.playerList[0].character2Hp -= damage;
            if (ServerLoginManager.playerList[0].character2Hp <= 0)
                ServerLoginManager.playerList[0].character2Hp = 0;
            AttackPacket.damage = ServerLoginManager.playerList[0].character2Hp;
        }

        NetworkManager.instance.Send(AttackPacket.Write());
    }
    public void KarmenBasicAttack()
    {
        send_Attack_packet(karmenAttackDamage);
    }
    public void KarmenQSkillAttack()
    {
        send_Attack_packet(karmenQSkillDamage);
    }
    public void KarmenWSkillAttack()
    {
        send_Attack_packet(karmenWSkillDamage);
    }
    public void JadeBasicAttack()
    {
        send_Attack_packet(JadeAttackDamage);
    }
    public void JadeQSkillAttack()
    {
        send_Attack_packet(JadeQSkillDamage);
    }
    public void JadeWSkillAttack()
    {
        send_Attack_packet(JadeWSkillDamage);
    }
    public void LeinaBasicAttack()
    {
        send_Attack_packet(LeinaAttackDamage);
    }
    public void LeinaQSkillAttack()
    {
        send_Attack_packet(LeinaQSkillDamage);
    }
    public void LeinaWSkillAttack()
    {
        send_Attack_packet(LeinaWSkillDamage);
    }
    public void EvaBasicAttack()
    {
        send_Attack_packet(EvaAttackDamage);
    }
    public void EvaQSkillAttack()
    {
        send_Attack_packet(EvaQSkillDamage);
    }
    public void EvaWSkillAttack()
    {
        send_Attack_packet(EvaWSkillDamage);
    }
}