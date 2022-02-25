using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContener //for spawn  at  empty "container" objects
{
    public Transform contener { get;  set; }
    public Queue<GameObject> objects;

    public PoolContener(Transform contener)
    {
        this.contener = contener;
        objects = new Queue<GameObject>();
    }
}
