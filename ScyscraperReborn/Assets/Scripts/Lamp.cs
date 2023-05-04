using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lamp : MonoBehaviour,Damagable
{
    private Rigidbody2D rb2d;
    private bool falling;
    [SerializeField]
    private int damage;
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    public void Damage(int dmg)
    {
        fall();
    }
    void fall()
    {
        falling = true;
        rb2d.gravityScale = 1;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer!=10&falling)
        {
            if (col.gameObject.layer==7|| col.gameObject.layer == 9)
            {
                col.gameObject.GetComponent<Damagable>().Damage(damage);
            }
            Destroy(gameObject);
        }
    }
}
