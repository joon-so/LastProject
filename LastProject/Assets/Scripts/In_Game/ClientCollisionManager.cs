using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCollisionManager : MonoBehaviour
{
    public int karmenAttackDamage;
    public int karmenQSkillDamage;
    public int karmenWSkillDamage;
    public int karmenESkillDamage;
    
    public int jadeAttackDamage;
    public int jadeQSkillDamage;
    public int jadeWSkillDamage;
    public int jadeESkillDamage;
    
    public int leinaAttackDamage;
    public int leinaQSkillDamage;
    public int leinaWSkillDamage;
    public int leinaESkillDamage;
    
    public int evaAttackDamage;
    public int evaQSkillDamage;
    public int evaWSkillDamage;
    public int evaESkillDamage;
    
    public int enemy1Attack;
    public int enemy2Attack;
    public int enemy3Attack;
    public int enemy6Attack;
    public int enemy7Attack;

    public int miniBossAttack;
    public int bossAttack1;
    public int bossAttack2;
    public int bossAttack3;
    public int bossAttack4;
    public int bossAttack5;

    public bool hit;

    public void PlayerHpSetting(int damage)
    {
        hit = true;
        if(GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
            GameManager.instance.clientPlayer.character1Hp -= damage;
            if (GameManager.instance.clientPlayer.character1Hp <= 0)
                GameManager.instance.clientPlayer.character1Hp = 0;
        }
        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
        {
            GameManager.instance.clientPlayer.character2Hp -= damage;
            if (GameManager.instance.clientPlayer.character2Hp <= 0)
                GameManager.instance.clientPlayer.character2Hp = 0;
        }
    }

    public void Enemy1Attack()
    {
        PlayerHpSetting(enemy1Attack);
    }
    public void Enemy2Attack()
    {
        PlayerHpSetting(enemy2Attack);
    }
    public void Enemy3Attack()
    {
        PlayerHpSetting(enemy3Attack);
    }
    public void Enemy6Attack()
    {
        PlayerHpSetting(enemy6Attack);
    }
    public void Enemy7Attack()
    {
        PlayerHpSetting(enemy7Attack);
    }
    public void MiniBossAttack()
    {
        PlayerHpSetting(miniBossAttack);
    }
    public void BossAttack1()
    {
        PlayerHpSetting(bossAttack1);
    }
    public void BossAttack2()
    {
        PlayerHpSetting(bossAttack2);
    }
    public void BossAttack3()
    {
        PlayerHpSetting(bossAttack3);
    }
    public void BossAttack4()
    {
        PlayerHpSetting(bossAttack4);
    }
    public void BossAttack5()
    {
        PlayerHpSetting(bossAttack5);
    }
}