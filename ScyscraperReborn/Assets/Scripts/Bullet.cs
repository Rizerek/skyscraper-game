using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 startPos;
    public int dmg;
    [SerializeField]
    private bool players;
    private int destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(startPos, transform.position) > destroyTime)
            Destroy(gameObject);
        gameObject.transform.position += transform.right * Time.deltaTime*50;

    }
    public void SetDestroyTime(int destTime)
    {
        destroyTime = destTime;
    }
     void OnTriggerEnter2D(Collider2D collision)
     {
        if (collision.gameObject.layer == 9 && !players)
        {
            collision.gameObject.GetComponent<Damagable>().Damage(dmg);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 7&&players)
        {
            if (collision.gameObject.name.Contains("head"))
            {
                collision.gameObject.GetComponent<Damagable>().Damage(dmg*2);
            }
            else
            {
                collision.gameObject.GetComponent<Damagable>().Damage(dmg);
            }
            

        }
        //if(collision.gameObject.layer == 9&&!players)
        if (collision.gameObject.layer!= 9)
        {
            
            if (!players && collision.gameObject.layer == 7)
            {
                //HIT MY FELLOW ENEMY
            }
            else
            {
                if (collision.gameObject.layer == 12 )
                {
                    if (collision.gameObject.GetComponent<Collider2D>().isTrigger != true)
                    {
                        Destroy(gameObject);
                    }

                }
                else
                {
                    Destroy(gameObject);
                }
               
            }
            
        }
      }
    
}
