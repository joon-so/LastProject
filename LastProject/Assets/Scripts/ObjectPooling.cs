using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;
    [SerializeField] private GameObject poolingObject;
    private Queue<JadeAssaultRifleBullet> objectQueue = new Queue<JadeAssaultRifleBullet>();
    private void Awake()
    {
        Instance = this;

        Initialize(5);
    }
    private JadeAssaultRifleBullet CreateObject()
    {
        var newObject = Instantiate(poolingObject, transform).GetComponent<JadeAssaultRifleBullet>();
        newObject.gameObject.SetActive(false);
        return newObject;
    }
    private void Initialize(int count)
    {
        for(int i =0; i< count; ++i)
        {
            objectQueue.Enqueue(CreateObject());
        }
    }
    public static JadeAssaultRifleBullet GetObject()
    {
        if(Instance.objectQueue.Count>0)
        {
            var obj = Instance.objectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObject = Instance.CreateObject();
            newObject.transform.SetParent(null);
            newObject.gameObject.SetActive(true);
            return newObject;
        }
    }
    public static void ReturnObject(JadeAssaultRifleBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        Instance.objectQueue.Enqueue(bullet);
    }
}