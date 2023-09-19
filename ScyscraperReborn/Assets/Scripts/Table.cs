using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Table : MonoBehaviour,Interactable
{
    public bool flipped;
    [SerializeField]
    private float jumpWidth;
    public bool occupied=false;
    public void Interact(GameObject interacter)
    {

        if (gameObject.transform.position.x > interacter.transform.position.x)
        {
            if (!flipped)
            {
                Flip(true);
            }
            else
            {
                JumpOver(true, interacter);
            }
        }
        else
        {
            if (!flipped)
            {
                Flip(false);
            }
            else
            {
                JumpOver(false,interacter);
            }
        }

    }
    public bool GetOccupation()
    {
        return occupied;
    }
    public void SetOccupation(bool occupied)
    {
        this.occupied = occupied;
    }
    public void Flip(bool right)
    {
        if (right)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        flipped = true;
    }
    void JumpOver(bool right,GameObject interacter)
    {
        if (right)
        {
            interacter.gameObject.transform.position += new Vector3(jumpWidth,0f,0f);
        }
        else
        {
            interacter.gameObject.transform.position += new Vector3(-jumpWidth, 0f, 0f);
        }

    }
}
