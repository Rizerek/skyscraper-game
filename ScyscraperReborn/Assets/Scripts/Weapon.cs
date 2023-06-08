using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField]
    private Transform shootPos;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float maxSpread;
    [SerializeField]
    private float coolDown;
    [SerializeField]
    private float spreadSpeed;
    [SerializeField]
    private float movingInaccuracy;
    [SerializeField]
    private float heatTime;
    [SerializeField]
    private bool automatic;
    [SerializeField]
    private int bulletDestroyTime;
    private float heat=0;
    private float timer;
    private float timer2;

    public int maxAmmo;
    public int allAmmo;
    public int ammo;
  //  public int mags;

    public Human owner;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
      //  mags--;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.fixedDeltaTime;
        timer2 += Time.fixedDeltaTime;
        
        if (owner.isMoving&&heat<movingInaccuracy)
        {
            heat = movingInaccuracy;
        }
    }
    public void Reload()
    {
        if (ammo < maxAmmo && allAmmo > 0 )
        {
            //mags--;
            if (allAmmo>=maxAmmo)
            {
                allAmmo -= (maxAmmo-ammo);
                ammo = maxAmmo;
                
            }
            else
            {
                ammo = allAmmo;
                allAmmo = 0;
            }
            
        }
    }
    public void Shoot()
    {
        if (automatic)
        {
            if (ammo > 0 && timer >= coolDown)
            {
                
                Quaternion rot = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, (gameObject.transform.rotation.eulerAngles.z + Random.Range(-heat, heat)));
                GameObject bulletObj  = Instantiate(bullet, shootPos.position, rot);
                bulletObj.GetComponent<Bullet>().SetDestroyTime(bulletDestroyTime);
                if (heat < maxSpread)
                {
                    heat += spreadSpeed;
                }
                timer = 0;
                ammo--;
            }
            if (heat > 0 && timer2 >= heatTime && !owner.shooting)
            {
                heat = 0;
                timer2 = 0;
            }

        }
        else
        {
            if (ammo > 0 && timer >= coolDown)
            {

                Quaternion rot = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, (gameObject.transform.rotation.eulerAngles.z + Random.Range(-heat, heat)));
                GameObject bulletObj = Instantiate(bullet, shootPos.position, rot);
                if (bulletObj.GetComponent<Bullet>()!=null)
                {
                    bulletObj.GetComponent<Bullet>().SetDestroyTime(bulletDestroyTime);
                }
               
                timer = 0;
                if (heat < maxSpread)
                {
                    heat += spreadSpeed;
                }
                ammo--;
            }
            if (heat > 0 && timer2 >= heatTime && !owner.shooting)
            {
                heat = 0;
                timer2 = 0;
            }
        }
    }
}
