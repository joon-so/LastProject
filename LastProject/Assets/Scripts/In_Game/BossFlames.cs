using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlames : MonoBehaviour
{
    ClientCollisionManager collisionManager;

    void Start()
    {
        collisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("MainCharacter"))
        {
            collisionManager.BossAttack4();
        }
    }
}
