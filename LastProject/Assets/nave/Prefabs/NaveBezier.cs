using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveBezier : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject fire1;
    [SerializeField] GameObject fire2;
    [SerializeField] GameObject fire3;
    [SerializeField] GameObject fire4;

    public GameObject shootPos1;
    public GameObject shootPos2;

    Vector3 P1;
    Vector3 P2;
    Vector3 P3;
    Vector3 explosionPos;


    // Start is called before the first frame update
    void Start()
    {
        P1 = transform.position;
        P2 = new Vector3(13.65f, -10f, 1.09f);
        P3 = new Vector3(138.6f, 48.5f, 1.09f);
        explosionPos = new Vector3(40.6f, 2.21f, 0.68f);
        StartCoroutine(Motion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Motion()
    {
        Vector3 bezier;
        yield return new WaitForSeconds(3.5f);
        float bezierValue = 0f;
        float shootTime = 2.5f;
        bool firstExplosion = false;
        bool shoot = false;
        while (bezierValue < shootTime)
        {
            bezierValue += Time.deltaTime;
            if (bezierValue > 0.65f && !shoot)
            {
                shoot = true;
                Instantiate(bullet, shootPos1.transform.position, transform.rotation * Quaternion.Euler(110f, 0f, 0));
                Instantiate(bullet, shootPos2.transform.position, transform.rotation * Quaternion.Euler(110f, 0f, 0));
            }
            if (bezierValue > 0.95f && !firstExplosion)
            {
                firstExplosion = true;
                Instantiate(explosion, explosionPos, explosion.transform.rotation);
                fire1.SetActive(false);
                fire2.SetActive(false);
                fire3.SetActive(false);
                fire4.SetActive(false);
            }
            bezier = Bezier(P1, P2, P3, bezierValue * 1 / shootTime);
            //transform.LookAt(bezier );
            transform.position = bezier;
            yield return null;
        }
    }

    Vector3 Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, float value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 D = Vector3.Lerp(A, B, value);

        return D;
    }
}
