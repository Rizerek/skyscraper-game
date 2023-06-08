using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour,Damagable
{
    [SerializeField]
    private GameObject parent;
    public int hp; 
    public void Damage(int dmg)
    {
        hp -= dmg;
        if (hp<=0)
        {
            parent.GetComponent<Damagable>().Damage(1000000);
        }
    }
}
