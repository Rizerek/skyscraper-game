using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 startPos;
    public int dmg;
    [SerializeField]
    private LayerMask player;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(startPos, transform.position) > 35)
            Destroy(gameObject);
        gameObject.transform.position += transform.right * Time.deltaTime*50;

    }
     void OnTriggerEnter2D(Collider2D collision)
     {

        if (collision.gameObject.layer == 7)
        {
            collision.gameObject.GetComponent<Damagable>().Damage(dmg);

        }
        if (collision.gameObject.layer!= 9&&collision.gameObject.layer!= 12)
        {
            Destroy(gameObject);
        }
      }
}
