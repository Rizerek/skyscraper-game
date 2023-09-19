using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy2
{
    public override void Attack()
    {
        runAtTarget = true;
    }

}
