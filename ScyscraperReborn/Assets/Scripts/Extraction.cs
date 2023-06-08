using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Extraction : MonoBehaviour, Interactable
{

    public void Interact(GameObject interacter)
    {
        SceneManager.LoadScene("EndScene");
    }
}
