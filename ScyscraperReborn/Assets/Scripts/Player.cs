using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class Player : Human,Damagable
{
    [SerializeField]
    private int hp;
    [SerializeField]
    public Transform aim;
    [SerializeField]
    private bool crouch = false;
    [SerializeField]
    private Transform m_CeilingCheck;
    [SerializeField]
    [Range(0, .3f)]
    public float m_MovementSmoothing = .05f;
    [SerializeField]
    private LayerMask notPlayer;
    [SerializeField]
    private Collider2D m_CrouchDisableCollider;
    [SerializeField]
    private GameObject m_CrouchDisableObject;
    [SerializeField]
    private bool m_wasCrouching = false;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private float runSpeed;
    [SerializeField]
    private float defaultRunSpeed = 40f;
    public float horizontalMove = 0f;
    [SerializeField]
    float wallDecreaseRatio = 2;


    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private List<GameObject> weaponList;

    [SerializeField]
    private GameObject interactPos;

    public LayerMask interactable;

    [SerializeField]
    private TMP_Text ammoText;

    private GameObject closestInteractable;
     
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private GameObject hand;

    private GameManager gameManager;

    // TODO : kiedy ma sie blisko drzwi i table czasem nie da sie przeskoczyc przez table bo podswietla tylko drzwi (nalezy podniesc punkt drzwi)
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        runSpeed = defaultRunSpeed;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        weapon.gameObject.transform.right = (aim.position - transform.position-hand.gameObject.transform.localPosition).normalized;
        
        InputsHandle();
        ClosestInteractableHighlighting();
        //TODO kucanie refaktoryzacja itp.
        //etap5
       // if (!crouch)
       // {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            Vector3 targetVelocity = new Vector2(horizontalMove * Time.fixedDeltaTime * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
      //  }
        if (horizontalMove != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        //etap7
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
          //  if (Physics2D.OverlapBox(m_CeilingCheck.position, new Vector2(1.13f, 0.8f), 0f, notPlayer))
          //  {
          //      crouch = true;
          //  }
        }
        if (crouch)
        {
            m_CrouchDisableObject.SetActive(false);
            runSpeed = defaultRunSpeed / 2;
            if (!m_wasCrouching)
            {
                m_wasCrouching = true;

            }
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            m_CrouchDisableObject.SetActive(true);
            runSpeed = defaultRunSpeed;
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;
            if (m_wasCrouching)
            {
                m_wasCrouching = false;
            }
        }
    }
    public void Damage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            gameManager.PlayerDied();
        }
    }
    void InputsHandle()
    {
        //etap1
        if (Input.GetButton("Fire1"))
        {
            MakeSound();
            weapon.Shoot();
            RefreshAmmoText();
        }
        else
        {
            shooting = false;
        }
        //etap2
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
            RefreshAmmoText();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            SetActiveWeapon(2);
        }
        //etap4 E -> skakanie nad sto
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D col = Physics2D.OverlapBox(interactPos.transform.position, new Vector2(1.88f, 2.5f), 0f, interactable);
            if (col != null)
            {
                GameObject colGameObject = col.gameObject;
                colGameObject.GetComponent<Interactable>().Interact(gameObject);
            }
        }
        //etap6
        if (Input.GetKeyDown(KeyCode.S))
        {
            crouch = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            crouch = false;
        }
    }
    void MakeSound()
    {
        RaycastHit2D[] lHits = Physics2D.RaycastAll(interactPos.transform.position, Vector2.left, weapon.GetLoudness(), (1 << 7) | (1 << 13));
        RaycastHit2D[] rHits = Physics2D.RaycastAll(interactPos.transform.position, Vector2.right, weapon.GetLoudness(), (1 << 7) | (1 << 13));
        Debug.DrawRay(interactPos.transform.position, Vector2.left * weapon.GetLoudness(), Color.green);
        Debug.DrawRay(interactPos.transform.position, Vector2.right * weapon.GetLoudness(), Color.green);
        foreach (RaycastHit2D hit in lHits)
        {
            if (hit.collider.gameObject.layer == 13)
            {
                float distance = Math.Abs(transform.position.x - hit.collider.transform.position.x);
               // Debug.Log(distance);
                lHits = Physics2D.RaycastAll(new Vector2(hit.collider.gameObject.transform.position.x, transform.position.y), Vector2.left, (weapon.GetLoudness() - distance) / wallDecreaseRatio, (1 << 7) | (1 << 13));
                Debug.DrawRay(new Vector2(hit.collider.gameObject.transform.position.x, transform.position.y), Vector2.left * ((weapon.GetLoudness() - distance) / wallDecreaseRatio), Color.red);
                foreach (RaycastHit2D hit2 in lHits)
                {
                    if (hit2.collider.gameObject == hit.collider.gameObject)
                    {
                        continue;
                    }
                    if (hit2.collider.gameObject.layer == 13)
                    {
                        Debug.Log("sciana");
                        break;
                    }

                  //  Debug.Log(hit2.collider.gameObject);
                    if (hit2.collider.gameObject.layer == 7)
                    {
                        AlertOrAngryEnemy(hit.collider.gameObject);
                        //StartCoroutine(hit2.collider.gameObject.GetComponent<Enemy>().GoAngry());
                        if (hit2.collider.gameObject.GetComponent<Enemy2>()!=null)
                        {
                            hit2.collider.gameObject.GetComponent<Enemy2>().Alert();
                        }
                    }
                }
                break;
            }
           // Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.layer == 7)
            {
                AlertOrAngryEnemy(hit.collider.gameObject);
                //StartCoroutine(hit.collider.gameObject.GetComponent<Enemy>().GoAngry());   po zmianie na enemy2 fragment kodu nie potrzebny
                if (hit.collider.gameObject.GetComponent<Enemy2>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy2>().Alert();
                }
            }
        }
        foreach (RaycastHit2D hit in rHits)
        {
            if (hit.collider.gameObject.layer == 13)
            {
                float distance = Math.Abs(transform.position.x - hit.collider.transform.position.x);
               //Debug.Log(distance);
                lHits = Physics2D.RaycastAll(new Vector2(hit.collider.gameObject.transform.position.x, transform.position.y), Vector2.right, (weapon.GetLoudness() - distance) / wallDecreaseRatio, (1 << 7) | (1 << 13));
                Debug.DrawRay(new Vector2(hit.collider.gameObject.transform.position.x, transform.position.y), Vector2.right * ((weapon.GetLoudness() - distance) / wallDecreaseRatio), Color.red);
                foreach (RaycastHit2D hit2 in lHits)
                {
                    if (hit2.collider.gameObject == hit.collider.gameObject)
                    {
                        continue;
                    }
                    if (hit2.collider.gameObject.layer == 13)
                    {
                        Debug.Log("sciana");
                        break;
                    }

                    Debug.Log(hit2.collider.gameObject);
                    if (hit2.collider.gameObject.layer ==7)
                    {
                        AlertOrAngryEnemy(hit.collider.gameObject);
                        //  StartCoroutine(hit2.collider.gameObject.GetComponent<Enemy>().GoAngry());
                    }
                }
                break;
            }
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.layer == 7)
            {
                AlertOrAngryEnemy(hit.collider.gameObject);

               // StartCoroutine(hit.collider.gameObject.GetComponent<Enemy>().GoAngry());
            }
        }
    }
    void AlertOrAngryEnemy(GameObject enemy)
    {
        if (Mathf.Abs(closestInteractable.transform.position.x - enemy.transform.position.x) < (weapon.GetLoudness()/2))
        {
            enemy.GetComponent<Enemy2>().Angry();
        }
        else
        {
            enemy.GetComponent<Enemy2>().Alert();
        }
    }
    void SetActiveWeapon(int weaponSlot)
    {
        foreach (GameObject weapon in weaponList)
        {
            weapon.SetActive(false);
        }
        weaponList[weaponSlot].SetActive(true);
        weapon = weaponList[weaponSlot].GetComponent<Weapon>();
        RefreshAmmoText();
    }
    void RefreshAmmoText()
    {
        ammoText.SetText(weapon.ammo + "/" + (weapon.allAmmo));
    }
    void ClosestInteractableHighlighting()
    {
        //etap3
        foreach (GameObject interactable in levelManager.GetInteractables())
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Mathf.Abs(closestInteractable.transform.position.x - gameObject.transform.position.x) > Mathf.Abs(interactable.transform.position.x - gameObject.transform.position.x) && Mathf.Abs(gameObject.transform.position.y-interactable.transform.position.y)<2)
                {
                    closestInteractable.GetComponent<SpriteRenderer>().color = Color.white;
                    closestInteractable = interactable;
                }
            }
        }
        if (Mathf.Abs(closestInteractable.transform.position.x - gameObject.transform.position.x) < 1.88f)
        {
            closestInteractable.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            closestInteractable.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public bool GetCrouch()
    {
        return crouch;
    }


}
