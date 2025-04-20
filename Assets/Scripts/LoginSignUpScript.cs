using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;

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
    
    async void Awake()
	{
		try
		{
			await UnityServices.InitializeAsync();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
	}


    // Setup authentication event handlers if desired
    void SetupEvents() 
    {
        AuthenticationService.Instance.SignedIn += () => {
        // Shows how to get a playerID
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        // Shows how to get an access token
        Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
        Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
        Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    public async void Start()
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
