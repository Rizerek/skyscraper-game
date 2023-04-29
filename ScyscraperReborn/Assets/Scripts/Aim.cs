using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    public GameObject player;
    public float range;
    [SerializeField]
    private float sens;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 playerPos = player.transform.position+new Vector3(0f, 0.8f,0f);
        Vector2 pos = transform.position;
        Vector2 cursorPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))*sens + pos;
        Vector2 playerToCursor = cursorPos - playerPos;
        float angle = Vector2.SignedAngle(Vector2.right, playerToCursor);

        if (Vector2.Distance(cursorPos, playerPos) > range)
        {
            gameObject.transform.position = new Vector2(range*Mathf.Cos(Mathf.Deg2Rad*angle), range * Mathf.Sin(Mathf.Deg2Rad * angle))+playerPos;
        }
        else if(Vector2.Distance(cursorPos, playerPos) < 1)
        {
            gameObject.transform.position = new Vector2(1 * Mathf.Cos(Mathf.Deg2Rad * angle), 1 * Mathf.Sin(Mathf.Deg2Rad * angle)) + playerPos;
        }
        else
        {
            transform.position = cursorPos;
        }    
    }
}
