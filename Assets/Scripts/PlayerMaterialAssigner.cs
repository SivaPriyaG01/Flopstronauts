using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
//using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMaterialAssigner : MonoBehaviour
{
    [SerializeField] private Button[] colorButtons;

    private void Start()
    {      
        
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i; // Capture index in local scope
            colorButtons[i].onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("SelectedMaterialIndex", index); // Store selected color
                PlayerPrefs.Save();
                //SceneManager.LoadScene("GameScene"); // Load the next scene where the player spawns
            });
        }
    }

    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
