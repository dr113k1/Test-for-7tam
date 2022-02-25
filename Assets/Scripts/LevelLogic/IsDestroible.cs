using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDestroible : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Explosion>() != null)
        { 
            Destroy(gameObject, 0.1f);
        }
    }
}
