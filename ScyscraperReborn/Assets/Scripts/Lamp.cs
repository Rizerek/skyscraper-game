using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lamp : MonoBehaviour,Damagable
{
    public void Damage(int dmg)
    {
        fall();
    }
    void fall()
    {
        Debug.Log("spadam");
    }
}
