using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Enemy : MonoBehaviour
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
        if ((isMoving == false)&&(turnTimer<=0))
        {
            ChooseDiraction();
            prevPos = transform.position;
        }
        turnTimer = turnTimer - Time.deltaTime;
    }
    private void AvoidOtherEnemy()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.right, out hit, cellSize);
        if ((Physics.Raycast(transform.position, transform.forward, out hit, cellSize) != false) && (hit.transform.gameObject.GetComponent<Enemy>() != null))
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
    void ChooseDiraction()//случайный маршрут
    { bool choose = false;
        while (choose == false)
        {
            RaycastHit hit;
            int a = Random.Range(1, 5);
            switch (a)
            {
                case 1:
                    {
                        if ((Physics.Raycast(transform.position, transform.forward, out hit, cellSize) == false) || (hit.collider.gameObject.GetComponent<Player>() != null))
                        {
                            destPos = transform.position + transform.forward * cellSize;
                            isMoving = true;
                            choose = true;
                        }
                        break;
                    }
                case 2:
                    {
                        if ((Physics.Raycast(transform.position, transform.right, out hit, cellSize) == false) || (hit.collider.gameObject.GetComponent<Player>() != null))
                        {
                            transform.eulerAngles += new Vector3(0, 90f, 0);
                            destPos = transform.position + transform.forward * cellSize;
                            isMoving = true;
                            choose = true;
                        }
                        break;
                    }
                case 3:
                    {
                        if ((Physics.Raycast(transform.position, transform.right*-1, out hit, cellSize) == false) || (hit.collider.gameObject.GetComponent<Player>() != null))
                        {
                            transform.eulerAngles += new Vector3(0, -90f, 0);
                            destPos = transform.position + transform.forward * cellSize;
                            isMoving = true;
                            choose = true;
                        }
                        break;
                    }
                case 4:
                    {
                        if ((Physics.Raycast(transform.position, transform.forward*-1, out hit, cellSize) == false)|| (hit.collider.gameObject.GetComponent<Player>() != null))
                        {
                            transform.eulerAngles += new Vector3(0, 180, 0);
                            destPos = transform.position + transform.forward * cellSize;
                            isMoving = true;
                            choose = true;
                        }
                        break;
                    }
            }
        }
        turnTimer = cellSize/ moveSpeed;
    }
}
