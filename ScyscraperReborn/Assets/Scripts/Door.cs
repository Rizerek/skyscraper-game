using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField]
    private Transform secondDoor;
    public void Interact(GameObject interacter)
    {
        interacter.transform.position = secondDoor.position+new Vector3(0,-0.5f,0);
    }
}
