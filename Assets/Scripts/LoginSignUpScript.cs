using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSignUpScript : MonoBehaviour
{
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] TMP_InputField loginEmailField;
    [SerializeField] TMP_InputField loginPasswordField;
    [SerializeField] TMP_InputField registerEmailField;
    [SerializeField] TMP_InputField registerPasswordField;
    [SerializeField] GameObject SignUpPanel;
    // Start is called before the first frame update
    void Start()
    {
        SignUpPanel.SetActive(false);
    }

    // Update is called once per frame
    
    public void OnLoginClicked()
    {
        SceneManager.LoadScene("NewMainMenuScene");
    }

    public void OnRegisterClicked()
    {

    }
}
