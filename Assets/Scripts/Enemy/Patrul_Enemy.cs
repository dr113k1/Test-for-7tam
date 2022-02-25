using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrul_Enemy : MonoBehaviour
{
    Vector3 destPos;
    Vector3 prevPos;

    bool isMoving = true;

    private float moveSpeed = 1f;
    private float cellSize = 1.0f;
    float turnTimer;
    private Animator animator;
    private void Start()
    {
        animator = transform.Find("PlayerModel").GetComponent<Animator>();
        destPos = transform.position;
    }
    void Update()
    {
        animator.SetBool("Walking", true);
        EnemyPatrol();
        AvoidOtherEnemy();
        if (isMoving == false)
        {
            ChooseDiraction();
            prevPos = transform.position;
        }
        turnTimer = turnTimer - Time.deltaTime;
    }
    private void AvoidOtherEnemy()//избегание ситуации когда 2 противника идут к одноц точке
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.right, out hit, cellSize);
       if((Physics.Raycast(transform.position, transform.forward, out hit, cellSize)!=false) && (hit.transform.gameObject.GetComponent<Enemy>() != null))
        {
            transform.eulerAngles += new Vector3(0, 180, 0);
            destPos = prevPos;
        }
    }
    private void EnemyPatrol()
    {
        if (isMoving == true)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destPos, step);
            if ((transform.position == destPos))
            {
                isMoving = false;
            }
        }
    }
    void ChooseDiraction()//маршрут до стенки и обратно
    {
        RaycastHit hit;
        if ((Physics.Raycast(transform.position, transform.forward, out hit, cellSize) == false) || (hit.collider.gameObject.GetComponent<Player>() != null))
        {
            destPos = transform.position + transform.forward * cellSize;
            isMoving = true;
        }
        else
        {
            if (turnTimer < 0)
            {
                transform.eulerAngles += new Vector3(0, 180f, 0);
                turnTimer = cellSize / moveSpeed;
            }
        }
    }
   


}

