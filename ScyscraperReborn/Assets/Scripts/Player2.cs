using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player2 : Human
{
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
    private bool m_wasCrouching = false;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    public float runSpeed = 40f;
    public float horizontalMove = 0f;


    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private GameObject weaponSlot1;
    [SerializeField]
    private GameObject weaponSlot2;
    [SerializeField]
    private GameObject grenadeSlot;

    [SerializeField]
    private GameObject interactPos;

    public LayerMask interactable;

    [SerializeField]
    private TMP_Text ammoText;

    private GameObject closestInteractable;

    [SerializeField]
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        weapon.gameObject.transform.right = aim.position - transform.position;
        //etap1
        if (Input.GetButton("Fire1"))
        {
            ammoText.SetText(weapon.ammo + "/" + (weapon.allAmmo));
            weapon.Shoot();

        }
        else
        {
            shooting = false;
        }
        //etap2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponSlot1.SetActive(true); weaponSlot2.SetActive(false); grenadeSlot.SetActive(false);
            weapon = weaponSlot1.GetComponent<Weapon>();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponSlot2.SetActive(true); weaponSlot1.SetActive(false); grenadeSlot.SetActive(false);
            weapon = weaponSlot2.GetComponent<Weapon>();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            grenadeSlot.SetActive(true); weaponSlot1.SetActive(false); weaponSlot2.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
            ammoText.SetText(weapon.ammo + "/" + (weapon.allAmmo));
        }
        //etap3
        foreach (GameObject interactable in levelManager.GetInteractables())
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Mathf.Abs(closestInteractable.transform.position.x - gameObject.transform.position.x) > Mathf.Abs(interactable.transform.position.x - gameObject.transform.position.x))
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
        //etap4
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D col = Physics2D.OverlapBox(interactPos.transform.position, new Vector2(1.88f, 2.5f), 0f, interactable);
            if (col != null)
            {
                GameObject colGameObject = col.gameObject;
                colGameObject.GetComponent<Interactable>().Interact(gameObject);
            }
        }
        //etap5
        if (!crouch)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            Vector3 targetVelocity = new Vector2(horizontalMove * Time.fixedDeltaTime * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
        if (horizontalMove != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
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
        //etap7
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapBox(m_CeilingCheck.position, new Vector2(1.13f, 0.8f), 0f, notPlayer))
            {
                crouch = true;
            }
        }
        if (crouch)
        {
            if (!m_wasCrouching)
            {
                m_wasCrouching = true;

            }
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;
            if (m_wasCrouching)
            {
                m_wasCrouching = false;
            }
        }
    }


}
