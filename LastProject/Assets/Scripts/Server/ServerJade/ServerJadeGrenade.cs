using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerJadeGrenade : MonoBehaviour
{
    public GameObject msehObj;
    public GameObject effectObj;
    public Rigidbody rigid;
    public AudioClip clip;
    ServerCollisionManager collisionManager;

    void Start()
    {
        collisionManager = GameObject.Find("ServerIngameManager").GetComponent<ServerCollisionManager>();
        StartCoroutine(Explosion());
    }
    IEnumerator Explosion()
    {

        yield return new WaitForSeconds(3.0f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        effectObj.SetActive(true);
        msehObj.SetActive(false);
        //SoundManager.instance.SFXPlay("WSkillEffect", clip);

        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
        Collider[] hitCol = Physics.OverlapSphere(gameObject.transform.position, 2f);

        for (int i =0;i< hitCol.Length;++i)
        {
            if (hitCol[i].gameObject.CompareTag("MainCharacter"))
            {
                collisionManager.JadeWSkillAttack();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(gameObject.transform.position, 2f);
    }
}