using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    [Serializable]
    public struct ObjectInfo//для инспектора тип,перфаб и начальное кол-во
    {
        public enum ObjectType
        {
            Bombs,
            Explosion

        }

        public ObjectType type;
        public GameObject prefab;
        public int startCount;
    }
    [SerializeField]
     List<ObjectInfo> objectInfo;

     Dictionary<ObjectInfo.ObjectType, PoolContener> pools;//список типов объектов пула



     void Awake()
    {
        if (Instance == null)
        {  
            Instance = this;
        }
        InitPool();
    }

     void InitPool() //начальное заполнение пула
    {
        pools = new Dictionary<ObjectInfo.ObjectType, PoolContener>();

        var emptyGO = new GameObject();

        foreach( var obj in objectInfo)
        {
            var container = Instantiate(emptyGO, transform, false);
            container.name = obj.type.ToString();
            pools[obj.type] = new PoolContener(container.transform);
            for(int i = 0; i < obj.startCount; i++)
            {
                var go = InstantiateObjects(obj.type, container.transform);
                pools[obj.type].objects.Enqueue(go);
            }

        }

        Destroy(emptyGO);
    }
     GameObject InstantiateObjects(ObjectInfo.ObjectType type,Transform parient)//adding a new object
    {
        var go = Instantiate(objectInfo.Find(x => x.type == type).prefab, parient);
        go.SetActive(false);
        return go;
    }


    public GameObject GetObject(ObjectInfo.ObjectType type)//taking an object from the pool
    {
        var obj = pools[type].objects.Count > 0 ?
            pools[type].objects.Dequeue() : InstantiateObjects(type, pools[type].contener);
        obj.SetActive(true);
        return obj;
    }
    public void DestryObject(GameObject obj)//returning an object to the pool
    {
        pools[obj.GetComponent<IPoolObject>().Type].objects.Enqueue(obj);
        obj.SetActive(false);
    }
}
