using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    

    [SerializeField]
    private List<GameObject> interactables;
    [SerializeField]
    public List<GameObject> enemies;
    [SerializeField]
    private float aggroDistance;
    [SerializeField]
    private float backAggroDistance;
    [SerializeField]
    private GameObject extractionZone;
    private bool objectiveComplete;
    private GameObject playerObj;
    private Player player;
    private GameManager gameManager;
    private int currentLvl;

    //TODO : autododawanie interactabli do listy    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerObj = GameObject.Find("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        player = playerObj.GetComponent<Player>();
        StartCoroutine(EnemyAggroSystem());
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count==0&&!objectiveComplete)
        {
            objectiveComplete = true;
            extractionZone.SetActive(true);
        }
        //czy kuca
            //tak to sprawdz czy patrzy w jego strone
                //nie return
                //tak sprawdz odleglosc
            //nie sprawdz odleglosc z przodu i z tylu
    }
    IEnumerator EnemyAggroSystem()
    {
        foreach (GameObject enemyObj in enemies)
        {
            Enemy2 enemy = enemyObj.GetComponent<Enemy2>();
            bool lookingRight = enemy.direction > enemyObj.transform.position.x;
            bool playerOnRight = enemyObj.transform.position.x < playerObj.transform.position.x;
            float leftDistance = enemyObj.transform.position.x - playerObj.transform.position.x;
            float rightDistance = playerObj.transform.position.x - enemyObj.transform.position.x;
            if (Mathf.Abs(player.transform.position.y - enemy.transform.position.y) < 2)
            {
                if (lookingRight)//patrzy w prawo
                {
                    if (playerOnRight && rightDistance < aggroDistance)//gracz jest po prawej stronie i odleglosc mniejsza od aggroDistance
                    {
                        Debug.Log("wrrrr");
                        enemy.Angry();
                    }
                    else if (!playerOnRight && leftDistance < backAggroDistance && !player.GetCrouch())//gracz jest po lewej stronie i odleglosc mniejsza od backAggroDistance i player nie kuca
                    {
                        Debug.Log("wrrrr");
                        enemy.Angry();
                    }
                }
                else//patrzy w lewo
                {
                    if (!playerOnRight && leftDistance < aggroDistance)
                    {
                        Debug.Log("wrrrr");
                        enemy.Angry();
                    }
                    else if (playerOnRight && rightDistance < backAggroDistance && !player.GetCrouch())
                    {
                        Debug.Log("wrrrr");
                        enemy.Angry();
                    }

                }
            }
            
        }

        yield return new WaitForSeconds(.01f);
        StartCoroutine(EnemyAggroSystem());
    }
    public void removeFromEnemyList(GameObject gameObject,int lvl)
    {
        enemies.Remove(gameObject);
    }
    public List<GameObject> GetInteractables()
    {
        return interactables;
    }
}
