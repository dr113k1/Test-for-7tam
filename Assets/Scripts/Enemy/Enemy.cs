using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject manger;
    public void OnGameStart( GameObject manger)
    {
        this.manger = manger;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Explosion>() != null)
        {
            manger.GetComponent<GameRandomaiser>().EnemyDestroyed();//сообщает о своей смерти
            Destroy(gameObject, 0.1f);
        }
    }
}
