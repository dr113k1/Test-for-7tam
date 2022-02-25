using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private ObjectPooler.ObjectInfo.ObjectType bombType;
    private float moveSpeed;
    private float bombCoolDown;
    private float cellSize = 1.0f;

    Vector3 destPos;

    bool isMoving = false;
    float localBombCoolDown;
    float bombTimer;
    int bombRadius;
    bool upTrigger = false;
    bool downTrigger = false;
    bool leftTrigger = false;
    bool rightTrigger = false;
    GameObject manger;
    




    private Animator animator;

    void Start()
    {
        animator = transform.Find("PlayerModel").GetComponent<Animator>();
         localBombCoolDown = bombCoolDown;
       
    }
    public void OnGameStart(float moveSpeed, float bombCoolDown,float bombTimer,int bombRadius,GameObject manger)
    {
        this.moveSpeed = moveSpeed;
        this.bombCoolDown = bombCoolDown;
        this.bombTimer = bombTimer;
        this.bombRadius = bombRadius;
        this.manger = manger;
    }

   
    void FixedUpdate()
    {
       PlayerMovement();
       PlayerMovementAnimation();
       BombPlant();
    }
  
    private void BombPlant()//установка бомб
    { 
        if (Input.GetKey(KeyCode.L)&& (localBombCoolDown<=0))
        {
            var bomb = ObjectPooler.Instance.GetObject(bombType);
            bomb.GetComponent<Bomb>().OnCreate(new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z)), bombTimer, bombRadius);
            localBombCoolDown = bombCoolDown;
        }
        localBombCoolDown = localBombCoolDown - Time.deltaTime;
    } 
    private void PlayerMovementAnimation()//проигрывание анимации(пока нажата хоть одна кнопка и впереди нет препятсвий)
    {
        if (((Input.GetKey(KeyCode.W) == true)  || (Input.GetKey(KeyCode.A) == true) || (Input.GetKey(KeyCode.S) == true) || (Input.GetKey(KeyCode.D) == true) || leftTrigger == true || rightTrigger == true || upTrigger == true || downTrigger == true)
             && (Physics.Raycast(transform.position, transform.forward, cellSize) == false))
            animator.SetBool("Walking", true);
        else
            animator.SetBool("Walking", false);
    }
    public void BombPlantButton()
    {
        if ((localBombCoolDown <= 0))
        {
            var bomb = ObjectPooler.Instance.GetObject(bombType);
            bomb.GetComponent<Bomb>().OnCreate(new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z)), bombTimer, bombRadius);
            localBombCoolDown = bombCoolDown;
        }
    }

    // способ сделать удерживаемую кнопку
  public void UpTriggerFalse()
    {
        upTrigger = false;
    }
    public void UpTriggerTrue()
    {
        upTrigger = true;
    }
    public void DownTriggerFalse()
    {
        downTrigger = false;
    }
    public void DownTriggerTrue()
    {
        downTrigger = true;
    }
    public void LeftTriggerFalse()
    {
        leftTrigger = false;
    }
    public void LeftTriggerTrue()
    {
        leftTrigger = true;
    }
    public void RightTriggerFalse()
    {
        rightTrigger = false;
    }
    public void RightTriggerTrue()
    {
        rightTrigger = true;
    }

    private void PlayerMovement()//движение
    {
        if (isMoving == true)
        {
          
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, destPos, step);
            if ((transform.position == destPos))
                isMoving = false;
        }
            else
            {
           
                if (Input.GetKey(KeyCode.W)||upTrigger==true) 
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    isMoving = true;
                }
                 if (Input.GetKey(KeyCode.A) ||leftTrigger==true)
                 {
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    isMoving = true;

                 }
                 if (Input.GetKey(KeyCode.S) || downTrigger == true)
                 {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    isMoving = true;
                 }
                 if (Input.GetKey(KeyCode.D)||rightTrigger==true )
                 {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    isMoving = true;
                 }
                 if (Physics.Raycast(transform.position, transform.forward, cellSize) == false)
                 {
                    destPos = transform.position + transform.forward * cellSize;
                  }
                 else
                 {
                        destPos = transform.position;
                 }
            }
    }
    public void OnTriggerEnter(Collider other)//смерить
    {
        if (other.GetComponent<Explosion>() != null)
        {
            manger.GetComponent<GameRandomaiser>().PlayerDestroyed(); //сообщение о смерти
            Destroy(gameObject, 0.1f);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            manger.GetComponent<GameRandomaiser>().PlayerDestroyed();
            Destroy(gameObject, 0.1f);
        }
    }
}
