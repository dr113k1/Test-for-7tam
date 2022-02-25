using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IPoolObject
{
    public ObjectPooler.ObjectInfo.ObjectType Type => type;
    [SerializeField] private ObjectPooler.ObjectInfo.ObjectType type;
     public void OnCreate(Vector3 position)
    {
        transform.position = position;
    }

    void OnEnable()
    {
        Invoke("BackToPool", 0.55f);
    }
    private void BackToPool()
    {
        ObjectPooler.Instance.DestryObject(gameObject);
    }

}
