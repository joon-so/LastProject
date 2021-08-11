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
    public int enemy4Attack;
    public int enemy5Attack;
    public int enemy6Attack;
    
    public int miniBossAttack;
    public int bossAttack1;
    public int bossAttack2;
    public int bossAttack3;
    public int bossAttack4;
    public int bossAttack5;

    public void PlayerHpSetting(int damage)
    {
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

    public void EnemyHpSetting(int damage)
    {
        // 플레이어가 깎은 enemy hp 처리
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
    public void Enemy4Attack()
    {
        PlayerHpSetting(enemy4Attack);
    }
    public void Enemy5Attack()
    {
        PlayerHpSetting(enemy5Attack);
    }
    public void Enemy6Attack()
    {
        PlayerHpSetting(enemy6Attack);
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
    
    public void KarmenBasicAttack()
    {
        EnemyHpSetting(karmenAttackDamage);
    }
    public void KarmenQSkillAttack()
    {
        EnemyHpSetting(karmenQSkillDamage);
    }
    public void KarmenWSkillAttack()
    {
        EnemyHpSetting(karmenWSkillDamage);
    }
    public void KarmenESkillAttack()
    {
        EnemyHpSetting(karmenESkillDamage);
    }

    public void JadeBasicAttack()
    {
        EnemyHpSetting(jadeAttackDamage);
    }
    public void JadeQSkillAttack()
    {
        EnemyHpSetting(jadeQSkillDamage);
    }
    public void JadeWSkillAttack()
    {
        EnemyHpSetting(jadeWSkillDamage);
    }
    public void JadeESkillAttack()
    {
        EnemyHpSetting(jadeESkillDamage);
    }

    public void LeinaBasicAttack()
    {
        EnemyHpSetting(leinaAttackDamage);
    }
    public void LeinaQSkillAttack()
    {
        EnemyHpSetting(leinaQSkillDamage);
    }
    public void LeinaWSkillAttack()
    {
        EnemyHpSetting(leinaWSkillDamage);
    }
    public void LeinaESkillAttack()
    {
        EnemyHpSetting(leinaESkillDamage);
    }

    public void EvaBasicAttack()
    {
        EnemyHpSetting(evaAttackDamage);
    }
    public void EvaQSkillAttack()
    {
        EnemyHpSetting(evaQSkillDamage);
    }
    public void EvaWSkillAttack()
    {
        EnemyHpSetting(evaWSkillDamage);
    }
    public void EvaESkillAttack()
    {
        EnemyHpSetting(evaESkillDamage);
    }
}