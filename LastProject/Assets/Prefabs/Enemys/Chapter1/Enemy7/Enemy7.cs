using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy7 : MonoBehaviour
{
    [SerializeField] [Range(10f, 30f)] float detectDistance = 15f;
    [SerializeField] GameObject explosion = null;
    [SerializeField] AudioClip deadSound;
    protected List<GameObject> targets;

    bool movable;
    bool alive = true;

    NavMeshAgent nav;

    float playerDistance;
    GameObject mainCharacter;

    public static int damage = 40;


    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        targets = GameObject.Find("Enemys").GetComponent<EnemyList>().Enemys;

        nav.enabled = false;

        //InvokeRepeating("Find", 0f, 0.5f);

        detectDistance = 20f;
        movable = false;
    }
                                                                                                   
    void FixedUpdate()
    {
        if (mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        if (alive && !movable)
        {
            Find();
        }
        else if(movable)
        {
            nav.SetDestination(mainCharacter.transform.position);
        }
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        //Change Target
        if (Input.GetKeyDown(KeyCode.F))
        {
            mainCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        }
    }

    void Find()
    {
        playerDistance = Vector3.Distance(
            new Vector3(mainCharacter.transform.position.x, 0, mainCharacter.transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z));

        if (playerDistance < detectDistance)
        {
            nav.enabled = true;
            movable = true;
        }
    }

    IEnumerator ExploseAndDistroy()
    {
        SoundManager.instance.SFXPlay("Explosion", deadSound);
        movable = false;
        Instantiate(explosion, transform.position, transform.rotation);
        //targets.Remove(gameObject);
        GetComponent<TriangleExplosion>().ExplosionMesh();
        yield return new WaitForSeconds(1f);
        //Destroy(gameObject);
    }

    //public void HitJadeGrenade()
    //{
    //    currentHp -= Jade.wSkillDamage;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "KarmenAttack")
    //    {
    //        currentHp -= Karmen.attackDamage;
    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MainCharacter")
        {
            if (alive)
            {
                alive = false;
                StartCoroutine(ExploseAndDistroy());
            }
        }
        //// Karmen
        //if (collision.gameObject.tag == "KarmenAttack")
        //{
        //    currentHp -= Karmen.attackDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "KarmenQSkill")
        //{
        //    currentHp -= Karmen.qSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "KarmenWSkill")
        //{
        //    currentHp -= Karmen.wSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //// Jade
        //if (collision.gameObject.tag == "JadeAttack")
        //{
        //    currentHp -= Jade.attackDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "JadeQSkill")
        //{
        //    currentHp -= Jade.qSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "JadeWSkill")
        //{
        //    currentHp -= Jade.wSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //// Leina
        //if (collision.gameObject.tag == "LeinaAttack")
        //{
        //    currentHp -= Leina.attackDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "LeinaQSkill")
        //{
        //    currentHp -= Leina.qSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "LeinaWSkill")
        //{
        //    currentHp -= Leina.wSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //// Eva
        //if (collision.gameObject.tag == "EvaAttack")
        //{
        //    currentHp -= Eva.attackDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "EvaQSkill")
        //{
        //    currentHp -= Eva.qSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
        //if (collision.gameObject.tag == "EvaWSkill")
        //{
        //    currentHp -= Eva.wSkillDamage;
        //    hpBar.SetHp(currentHp);
        //}
    }

}