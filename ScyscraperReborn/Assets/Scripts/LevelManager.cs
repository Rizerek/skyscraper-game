using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> interactables;
    //TODO : autododawanie interactabli do listy    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<GameObject> GetInteractables()
    {
        return interactables;
    }
}
