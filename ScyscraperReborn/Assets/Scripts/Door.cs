using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField]
    private Transform secondDoor;
    [SerializeField]
    private int lvlNumber;
    [SerializeField]
    private LevelManager levelManager;
    private bool occupied = false;
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }
    public void Interact(GameObject interacter)
    {
        interacter.transform.position = secondDoor.position+new Vector3(0,-0.5f,0);
        
    }
    public bool GetOccupation()
    {
        return occupied;
    }
    public void SetOccupation(bool occupied)
    {
        this.occupied = occupied;
    }
}
