using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeGrenade : MonoBehaviour
{
    public GameObject msehObj;
    public GameObject effectObj;
    public Rigidbody rigid;
    public AudioClip clip;

    ClientCollisionManager collisionManager;

    void Start()
    {
        collisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
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

        for (int i = 0; i < hitCol.Length; ++i)
        {
            if (hitCol[i].gameObject.CompareTag("Enemy1"))
            {
                hitCol[i].gameObject.GetComponent<Enemy1>().HitJadeGrenade();
                Debug.Log("ad");
            }
            if (hitCol[i].gameObject.CompareTag("Enemy2"))
            {
                GetComponent<Enemy2>().HitJadeGrenade();
            }
            if (hitCol[i].gameObject.CompareTag("Enemy3"))
            {
                GetComponent<Enemy3>().HitJadeGrenade();
            }
            if (hitCol[i].gameObject.CompareTag("Enemy6"))
            {
                GetComponent<Enemy6>().HitJadeGrenade();
            }
        }
    }
}