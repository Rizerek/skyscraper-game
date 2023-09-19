using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : Enemy2
{
    public override void Attack()
    {
        direction = player.transform.position.x;
        RotatingEnemy();
        StartCoroutine(timer(timeBetweenAttacks, Shoot));
    }


}
