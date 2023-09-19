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
        Walking,
        Shooting
    }
    enum Type
    {
        gunner,
        rusher
    }




    private int hp;
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int meleeDamage;
    [SerializeField]
    private float meleeAttackCooldown;
    [SerializeField]
    private float meleeAttackRange;
    [SerializeField]
    private float meleeChargeTime;
    private bool meleeAttacking;
    private Vector3 startPos;
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private GameObject closestTable;
    [SerializeField]
    public bool angry;
    public Vector3 direction;
    private GameObject player;
    [SerializeField]
    private float angrySpeed;
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private State state;
    [SerializeField]
    private Type type;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private float chaseRange;

    [SerializeField]
    private float timeBetweenShots;
    [SerializeField]
    private float timeBetweenRounds;
    [SerializeField]
    private int roundNumber;
    [SerializeField]
    private float aggroBufferTime;

    public SpriteRenderer visual;

    //TODO: SPRAWDZANIE NA JAKIM LEVELU JEST PRZECIWNIK
    [SerializeField]
    private int currentLvl;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        hp = maxHp;
        startPos = gameObject.transform.position;
        state = State.Walking;
        direction = startPos + new Vector3(UnityEngine.Random.Range(-4, 4), 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (angry)
        {
            if (player.transform.position.x > gameObject.transform.position.x)
            {
                weapon.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                weapon.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if (direction.x > gameObject.transform.position.x)
            {
                weapon.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                weapon.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
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
                if (type == Type.rusher)
                {
                    if (player.transform.position.x>gameObject.transform.position.x)
                    {
                        direction = player.transform.position+new Vector3(-2f,0f,0f);
                    }
                    else
                    {
                        direction = player.transform.position + new Vector3(2f, 0f, 0f);
                    }
                    if (Math.Abs(transform.position.x - player.transform.position.x)<= meleeAttackRange&& Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < 2)
                    {
                        StartCoroutine(MeleeAttack());
                    }
                    
                }
                if (Math.Abs(transform.position.x - direction.x) < 0.01f)//tutaj chodzi o dokladnosc
                {
                    if (type == Type.gunner)
                    {
                        //here when closest table is defined as enemy error occurs
                        try
                        {
                            if (player.transform.position.x > closestTable.transform.position.x)
                            {
                                closestTable.GetComponent<Table>().Flip(true);
                            }
                            else
                            {
                                closestTable.GetComponent<Table>().Flip(false);
                            }
                        }
                        catch
                        {
                            Debug.Log("cant flip myself");
                        }
                        
                        state = State.Shoot;
                    }
                    
                    
                }
                transform.position = new Vector3(Vector3.MoveTowards(transform.position, direction, angrySpeed * .001f).x, gameObject.transform.position.y, 0f);
            }
            if (state == State.Shoot)
            {
                state = State.Shooting;
                Shoot();
                
            }
           
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {

            StartCoroutine(GoAngry());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GetCalm();
        }
        //if(moving) isMoving=true;
        //else isMoving =false;

       
        
    }
    IEnumerator MeleeAttack()
    {
        if (!meleeAttacking)
        {
            meleeAttacking = true;
            yield return new WaitForSeconds(meleeChargeTime);
            if (Math.Abs(transform.position.x - player.transform.position.x) <= meleeAttackRange&& Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < 2)
            {
                player.GetComponent<Player>().Damage(meleeDamage);
                Debug.Log("melee");
            }
            yield return new WaitForSeconds(meleeAttackCooldown);
            meleeAttacking = false;
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
        
        StartCoroutine(ShootInterval());
    }
    IEnumerator ShootInterval()
    {
        //if (chaseRange< Math.Abs(transform.position.x - player.transform.position.x))
        //{
            if (state == State.Shooting)
            {


                shooting = true;
                for (int i = 0; i < roundNumber; i++)
                {
                    weapon.Shoot();
                    yield return new WaitForSeconds(timeBetweenShots);
                }
                shooting = false;
                yield return new WaitForSeconds(timeBetweenRounds);
                StartCoroutine(ShootInterval());
            }
       // }
        
        
    }
    private void GoIdle(int interval)
    {
        state = State.Idle; 
        Timer aTimer = new Timer();
        aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        aTimer.Interval = interval;
        aTimer.Enabled = true;
    }
    public IEnumerator GoAngry()
    {
        if (!angry)
        {
            Color prevColor = visual.color;
            visual.color = Color.black;
            angry = true;
            yield return new WaitForSeconds(aggroBufferTime);
            visual.color = prevColor;
            FindTable();
            DecideWhatToDo();
        }
       
    }
    private void DecideWhatToDo()
    {
        if (type == Type.gunner)
        {
           // if (chaseRange < Math.Abs(transform.position.x - player.transform.position.x))
          //  {
                if (Math.Abs(transform.position.x - closestTable.transform.position.x) < Math.Abs(transform.position.x - player.transform.position.x))
                {
                    if (player.transform.position.x > closestTable.transform.position.x)
                    {
                        direction = closestTable.transform.position + new Vector3(-1.2f, 0f, 0f);
                    }
                    else
                    {
                        direction = closestTable.transform.position + new Vector3(1.2f, 0f, 0f);
                    }
                    state = State.Running;
                }
                else
                {
                    state = State.Shoot;
                }
          //  }

            
        }
        else if(type == Type.rusher)
        {
            direction = player.transform.position;
            state = State.Running;
        }
        
    }
    public void Damage(int dmg)
    {
        if (!angry)
        {
            StartCoroutine(GoAngry());
        }
        hp -= dmg;
        if (hp <= 0)
        {
            levelManager.removeFromEnemyList(gameObject, currentLvl);
            Destroy(gameObject);
        }
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
                else if(Math.Abs(closestTable.transform.position.x-gameObject.transform.position.x)> Math.Abs(interactable.transform.position.x - gameObject.transform.position.x)&& Math.Abs(interactable.transform.position.y - gameObject.transform.position.y)<2)
                {
                    
                    closestTable = interactable;
                }
                
            }
        }
        if (Math.Abs(closestTable.transform.position.y - gameObject.transform.position.y) > 2)
        {
            closestTable = gameObject;
            state = State.Shoot;
        }
    }
}
