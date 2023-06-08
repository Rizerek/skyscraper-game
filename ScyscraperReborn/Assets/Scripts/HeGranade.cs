using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeGranade : MonoBehaviour,Grenade
{
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float timeTillExplosion;
    [SerializeField]
    private int dmg;
    [SerializeField]
    private LayerMask enemy;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d.AddForce(transform.right * throwForce, ForceMode2D.Impulse);
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer>timeTillExplosion)
        {
                Collider2D[] col = Physics2D.OverlapCircleAll(gameObject.transform.position, 2f);
                foreach (Collider2D c in col)
                {
                    if (c.gameObject.layer == 7|| c.gameObject.layer ==9)
                    {
                        Debug.Log(c.gameObject.name);
                        c.gameObject.GetComponent<Damagable>().Damage(dmg);
                    }
                }
            
           
            Destroy(gameObject);
        }
    }
    /*
    public void Throw(GameObject thrower, GameObject throwPos)
    {
        
        
    }
    */
    
}
