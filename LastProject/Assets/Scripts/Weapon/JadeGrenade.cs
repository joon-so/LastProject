using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeGrenade : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3.0f);


        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

    }
}
