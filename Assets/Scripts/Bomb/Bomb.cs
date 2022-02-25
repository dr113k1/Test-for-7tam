using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour,IPoolObject
{
    public ObjectPooler.ObjectInfo.ObjectType Type => type;//тип для пула
    [SerializeField] private ObjectPooler.ObjectInfo.ObjectType type;
    [SerializeField] private ObjectPooler.ObjectInfo.ObjectType ExpType;
    public LayerMask levelMask;
    private bool exploded = false;
    int bombRadius;


    public void OnCreate(Vector3 postion, float bombTimer,int bombRadius )
    {

        this.bombRadius = bombRadius;//радиус вызрыва
        exploded = false;//взорвана ли(для одновременной дитонации)
        transform.position = postion;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponentInChildren<ParticleSystem>().Play();//фитиль
        GetComponentInChildren<Collider>().isTrigger = true;
        Invoke("Explode", bombTimer);
    }
  
    private void Explode()//вызрыв
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        var expl = ObjectPooler.Instance.GetObject(ExpType);
        expl.GetComponent<Explosion>().OnCreate(transform.position);
        {
            StartCoroutine(CreateExplosions(Vector3.forward));
            StartCoroutine(CreateExplosions(Vector3.right));
            StartCoroutine(CreateExplosions(Vector3.back));
            StartCoroutine(CreateExplosions(Vector3.left));
        }//одновременный взрыв в 4 стороны
        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        Invoke("BackToPool", 0.3f);
    }
    private IEnumerator CreateExplosions(Vector3 direction)//волна взрыва
    {
        for (int i = 1; i < bombRadius; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position , direction, out hit, i, levelMask);

            if ((hit.collider) &&(hit.collider.gameObject.GetComponent<IsDestroible>() != null))
            {
                var expl = ObjectPooler.Instance.GetObject(ExpType);
                expl.GetComponent<Explosion>().OnCreate(transform.position + (i * direction));
               
                break;
            }
            if ((!hit.collider))
            {
                var expl = ObjectPooler.Instance.GetObject(ExpType);
                expl.GetComponent<Explosion>().OnCreate(transform.position + (i * direction));
            }
            else
            { 
                break;
            }
            yield return new WaitForSeconds(.05f);
        }
    }
    private void BackToPool()//возвращение в пул
    {
        ObjectPooler.Instance.DestryObject(gameObject);
    }
    public void OnTriggerExit(Collider other)//позволяет игроку сойти с бомбы при ее постановке
    {
        if (other.gameObject.GetComponent<Player>()!=null)
        { 
            GetComponentInChildren<Collider>().isTrigger = false;
            
        }
    }
    public void OnTriggerEnter(Collider other)//дитонация от другой бомбы
    {
        if (!exploded && other.GetComponent<Explosion>()!=null)
        {
            CancelInvoke("Explode"); 
            Explode(); 
        }
    }
}