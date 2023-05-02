using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Interactable
{
    [SerializeField]
    private GameObject item;
    public void Interact(GameObject interacter)
    {
        Instantiate(item,gameObject.transform.position,gameObject.transform.rotation);
    }
}
