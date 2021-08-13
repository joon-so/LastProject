using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3To4LightExpolsion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<TriangleExplosion>().ExplosionMesh();
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<TriangleExplosion>().ExplosionMesh();
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
