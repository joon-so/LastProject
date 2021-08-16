using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaHammer : MonoBehaviour
{
    public float speed;
    [Tooltip("From 0% to 100%")]
    public float accuracy;
    public float fireRate;

    private Vector3 startPos;
    private Vector3 offset;
    private bool collided;
    private Rigidbody rigid;
    private RotateToMouseScript rotateToMouse;
    private GameObject target;

    public float distance = 3.0f;
    void Start()
    {
        startPos = transform.position;
        rigid = GetComponent<Rigidbody>();

        if (accuracy != 100)
        {
            accuracy = 1 - (accuracy / 100);

            for (int i = 0; i < 2; i++)
            {
                var val = 1 * Random.Range(-accuracy, accuracy);
                var index = Random.Range(0, 2);
                if (i == 0)
                {
                    if (index == 0)
                        offset = new Vector3(0, -val, 0);
                    else
                        offset = new Vector3(0, val, 0);
                }
                else
                {
                    if (index == 0)
                        offset = new Vector3(0, offset.y, -val);
                    else
                        offset = new Vector3(0, offset.y, val);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (target != null)
            rotateToMouse.RotateToMouse(gameObject, target.transform.position);
        if (speed != 0 && rigid != null)
            rigid.position += (transform.forward + offset) * (speed * Time.deltaTime);

        // ¹üÀ§
        if (Vector3.Distance(startPos, transform.position) > distance)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.layer != 8 || collision.gameObject.tag != "SubCharacter") && !collided)
        {
            collided = true;

            speed = 0;
            GetComponent<Rigidbody>().isKinematic = true;

            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject trg, RotateToMouseScript rotateTo)
    {
        target = trg;
        rotateToMouse = rotateTo;
    }
}
