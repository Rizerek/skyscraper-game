using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy2 : Human,Damagable
{
    enum State
    {
        Passive,
        Alert,
        Angry
    }
    [SerializeField]
    private int maxHp;
    [SerializeField]
    private int hp;
    [SerializeField]
    private State state;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    protected float timeBetweenAttacks;
    [SerializeField]
    private float shootRange;
    private LevelManager levelManager;
    private bool walk=true;
    private Vector3 startPos;
    public float direction;
    private bool behindCover = false;
    protected GameObject player;
    private GameObject closestTable;
    protected bool runAtTarget = false;
    private bool timerRunning;
    private bool isShooting;
    [SerializeField]
    private int currentLvl;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        player = GameObject.Find("Player");
        startPos = gameObject.transform.position;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Alert();
            Debug.Log("alert");
        }

        if (Input.GetKeyDown("space"))
        {
            Angry();
            Debug.Log("angry");
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        

        

        if (state == State.Passive)
        {

        }
        else if(state == State.Alert)
        {
            
        }
        else if (state == State.Angry)
        {
            
        }
        
        if (walk)
        {
            RotatingEnemy();
            if (Math.Abs(transform.position.x - direction)<0.01f)
            {
                if (state==State.Passive)
                {
                    StartCoroutine(timer(2, SetIdleDirection));
                }
                if (state==State.Alert)
                {
                    if (closestTable!=null&&closestTable!=player)
                    {
                        closestTable.GetComponent<Interactable>().Interact(gameObject);
                        
                    }
                }
                walk = false;
            }
            else
            { 
                move(direction);
            }
            
        }
        if (runAtTarget)
        {
            direction = player.transform.position.x;
            RotatingEnemy();
            if (state == State.Angry)
            {
                if (Math.Abs(transform.position.x - player.transform.position.x) < shootRange-(0.3*shootRange)&& timerRunning == false && isShooting == false)
                {
                   StartCoroutine(timer(timeBetweenAttacks, Shoot));
                }
                else
                {
                    move(direction);
                }
            }
             
        }
    }
    public void Angry()
    {
        
        walk = false;
        state = State.Angry;
        timerRunning = false;
        Attack();
    }
    public abstract void Attack();
    protected void RotatingEnemy()
    {
        if (direction > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
    protected void Shoot()
    {
        isShooting = true;
        Debug.Log(Math.Abs(transform.position.x - player.transform.position.x));
        runAtTarget = false;
        
        if (Math.Abs(transform.position.x - player.transform.position.x) < shootRange)
        {
            
            weapon.Shoot();
            StartCoroutine(timer(timeBetweenAttacks, Shoot));
        }
        else
        {
            Attack();
        }
        isShooting = false;
    }
    public void Alert()
    {

        if (state != State.Angry&&state!=State.Alert)
        {
            walk = false;
            state = State.Alert;
            if (!behindCover) FindCover();
        }
    }
    void FindCover()
    {
        
        direction = FindClosestTable().x;
        walk = true;

        behindCover = true;
    }
    Vector3 FindClosestTable()
    {
        closestTable = player;
        foreach (GameObject interactable in levelManager.GetInteractables())
        {
            Debug.Log("A");
            if (interactable.CompareTag("Cover"))
            {
                if (Math.Abs(closestTable.transform.position.x - gameObject.transform.position.x) > Math.Abs(interactable.transform.position.x - gameObject.transform.position.x)  && interactable.GetComponent<Interactable>().GetOccupation() == false)
                {

                    
                    closestTable = interactable;
                    
                }

            }
            
        }
        if (closestTable!=player)
        {
            closestTable.GetComponent<Interactable>().SetOccupation(true);
        }
        
        if (closestTable==player)
        {
            return gameObject.transform.position;
        }
        else if (closestTable.transform.position.x > player.transform.position.x)
        {
            return (closestTable.transform.position + new Vector3(1.5f, 0f, 0f));
        }
        else if(closestTable.transform.position.x <= player.transform.position.x)
        {
            return (closestTable.transform.position + new Vector3(-1.5f, 0f, 0f));
        }
        else
        {
            return gameObject.transform.position;
        }
        


    }
    void move(float direction)//direction == desired x postion
    {
        transform.position = new Vector3(Vector3.MoveTowards(transform.position, new Vector3(direction, transform.position.y,0f), speed  *Time.fixedDeltaTime).x, gameObject.transform.position.y, 0f);
    }
    protected IEnumerator timer(float time,Action function)
    {
        timerRunning = true;
        yield return new WaitForSeconds(time);
        function();
        timerRunning = false;
        
    }
    void SetIdleDirection()
    {
        if (state==State.Passive)
        {
            direction = startPos.x + UnityEngine.Random.Range(-4, 4);
            walk = true;
        }

    }
    public void Damage(int dmg)
    {
        hp -= dmg;
        direction = player.transform.position.x;
        RotatingEnemy();

        Angry();
        if (hp <= 0)
        {
            levelManager.removeFromEnemyList(gameObject, currentLvl);
            Destroy(gameObject);
        }
    }


}
