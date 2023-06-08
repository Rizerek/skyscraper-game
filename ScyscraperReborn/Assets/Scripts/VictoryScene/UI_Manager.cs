using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Button continueBttn;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager.defeat)
        {
            text.text = "Defeat :(";
        }
        else
        {
            text.text = "Victory!!";
        }
        continueBttn.onClick.AddListener(Continue);
    }

    // Update is called once per frame
    void Continue()
    {
        SceneManager.LoadScene("SampleScene");

    }
  
}
