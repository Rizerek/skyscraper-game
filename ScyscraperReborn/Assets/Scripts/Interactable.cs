using UnityEngine;

interface Interactable
{
    public bool GetOccupation();
    public void SetOccupation(bool occupied);
    public void Interact(GameObject interacter);
}

