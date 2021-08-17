using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvaQSkill : MonoBehaviour
{
    ServerCollisionManager collisionManager;

    void Start()
    {
        collisionManager = GameObject.Find("ServerIngameManager").GetComponent<ServerCollisionManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 10)
            return;

        if (ServerLoginManager.playerList[0].character1Hp <= 0 || ServerLoginManager.playerList[0].character2Hp <= 0)
            return;

        if (other.gameObject.CompareTag("MainCharacter"))
        {
            collisionManager.EvaQSkillAttack();
        }
    }
}
