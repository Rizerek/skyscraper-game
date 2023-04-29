using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Timers;

public class Enemy : Human,Damagable
{
    enum State
    {
        Idle,
        Running,
        Shoot,
        Walking
    }

    [SerializeField]
    private int hp;
    [SerializeField]
    private int maxHp;
    private Vector3 startPos;
    [SerializeField]
    private LevelManager levelManager;
    private GameObject closestTable;
    [SerializeField]
    private bool angry;
    [SerializeField]
    private Vector3 direction;
    private GameObject player;
    [SerializeField]
    private float angrySpeed;
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private State state;
    [SerializeField]
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        hp = maxHp;
        startPos = gameObject.transform.position;
        state = State.Walking;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x>gameObject.transform.position.x)
        {
            weapon.gameObject.transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            weapon.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (!angry)
        {
            if (state==State.Walking)
            {
                if (Math.Abs(transform.position.x - direction.x) < 0.01f)
                {
                    direction = startPos + new Vector3(UnityEngine.Random.Range(-4, 4), 0f, 0f);
                    GoIdle(UnityEngine.Random.Range(1500, 2500));
                }
                transform.position = new Vector3(Vector3.MoveTowards(transform.position, direction, normalSpeed*0.001f).x, gameObject.transform.position.y, 0f);
            }
        }
        else
        {
            if (state==State.Running)
            {
                if (Math.Abs(transform.position.x - direction.x) < 0.01f)
                {
                    if (player.transform.position.x > closestTable.transform.position.x)
                    {

                        closestTable.GetComponent<Table>().Flip(true);
                       
                    }
                    else
                    {

                        closestTable.GetComponent<Table>().Flip(false);
                    }
                    state = State.Shoot;
                }
                transform.position = new Vector3(Vector3.MoveTowards(transform.position, direction, angrySpeed * .001f).x, gameObject.transform.position.y, 0f);
            }
            if (state == State.Shoot)
            {
                Shoot();
            }
           
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GoAngry();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GetCalm();
        }
        //if(moving) isMoving=true;
        //else isMoving =false;

        if (hp<=0)
        {
            Destroy(gameObject);
        }
        
    }
    void Shoot()
    {
        //do dodania: kiedy enemy sie rusza strzela nieprecyzyjniej
        /*
          if (player.gameObject.GetComponent<Player>().horizontalMove!=0&&heat<movingInaccuracy)
        {
            heat = movingInaccuracy;
        }
        */
       
        weapon.Shoot();
    }
    private void GoIdle(int interval)
    {
        state = State.Idle;
        Timer aTimer = new Timer();
        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        aTimer.Interval = interval;
        aTimer.Enabled = true;
    }
    private void GoAngry()
    {
        angry = true;
        FindTable();
        DecideWhatToDo();
    }
    private void DecideWhatToDo()
    {
        if (Math.Abs(transform.position.x - closestTable.transform.position.x)<Math.Abs(transform.position.x -player.transform.position.x))
        {

            
            if (player.transform.position.x>closestTable.transform.position.x)
            {

                direction = closestTable.transform.position + new Vector3(-1.2f, 0f, 0f);
            }
            else
            {
                
                direction = closestTable.transform.position + new Vector3(1.2f, 0f, 0f);
            }

            state = State.Running;
        }
    }
    public void Damage(int dmg)
    {
        hp -= dmg;
    }
    private void GetCalm()
    {
        angry = false;
        state = State.Walking;
    }
    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        if (!angry)
        {
            state = State.Walking;
        }
        ((Timer) source).Stop();
        Debug.Log("aaa");
    }
    private void FindTable()
    {

        foreach (GameObject interactable in levelManager.GetInteractables())
        {
            if (interactable.GetComponent<Table>() != null)
            {
                
                if (closestTable ==null)
                {
                    closestTable = interactable;
                }
                else if(Math.Abs(closestTable.transform.position.x-gameObject.transform.position.x)> Math.Abs(interactable.transform.position.x - gameObject.transform.position.x))
                {
                    
                    closestTable = interactable;
                } 
            }
        }
      
    }
}
