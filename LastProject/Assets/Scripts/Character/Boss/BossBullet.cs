using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Tooltip("From 0% to 100%")]
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;

    private Vector3 offset;
    private bool collided;

    float shootTime;
    float bezierValue;
    Vector3 P1;
    Vector3 P2;
    Vector3 P3;
    Vector3 bezier;
    GameObject target;
    //bool enemyCheck = true;

    void Start()
    {
        shootTime = 0.7f;
        bezierValue = 0;
        P1 = transform.position;
        P2 = transform.position + transform.forward * Random.Range(1f, 4f) + transform.right * Random.Range(-7f, 7f) + transform.up * Random.Range(3f, 5f);
        //P3 = Jade.enemyPos.transform.position;
        target = GameObject.FindGameObjectWithTag("MainCharacter");

        P3 = new Vector3(target.transform.position.x, 0f, target.transform.position.z);

        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward + offset;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(muzzleVFX, ps.main.duration);
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    void FixedUpdate()
    {
        bezierValue += Time.deltaTime;

        bezier = Bezier(P1, P2, P3, bezierValue * 1 / shootTime);
        transform.LookAt(bezier);
        transform.position = bezier;

        // ¹üÀ§
        if (bezierValue > shootTime)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.layer != 8 || collision.gameObject.tag == "MainCharacter") && !collided)
        {
            collided = true;

            GetComponent<Rigidbody>().isKinematic = true;

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot * Quaternion.Euler(0f, 90f, 0f)) as GameObject;

                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }

            StartCoroutine(DestroyParticle(0f));
        }
    }

    public IEnumerator DestroyParticle(float waitTime)
    {

        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> tList = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform)
            {
                tList.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < tList.Count; i++)
                {
                    tList[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }
         
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    public Vector3 Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, float value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 D = Vector3.Lerp(A, B, value);

        return D;
    }
}
