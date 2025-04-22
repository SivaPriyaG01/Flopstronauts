using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LoginSignUpScript : MonoBehaviour
{
    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;
    [SerializeField] TMP_InputField loginUsernameField;
    [SerializeField] TMP_InputField loginPasswordField;
    [SerializeField] TMP_InputField registerUsernameField;
    [SerializeField] TMP_InputField registerPasswordField;
    [SerializeField] GameObject SignUpPanel;
    [SerializeField] TMP_Text messages;
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

    public void Start()
    {
        SignUpPanel.SetActive(false);
        //SetupEvents();
        registerButton.onClick.AddListener(()=> OnRegisterClicked(registerUsernameField.text,registerPasswordField.text));
        loginButton.onClick.AddListener(()=> OnLoginClicked(loginUsernameField.text,loginPasswordField.text));
    }

    // Update is called once per frame
    
    public async void OnLoginClicked(string username, string password)
    {
        await SignInWithUsernamePasswordAsync(username, password);
        SceneManager.LoadScene("NewMainMenuScene");
    }

    public async void OnRegisterClicked(string username, string password)
    {
        await SignUpWithUsernamePasswordAsync(username, password);
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
    try
    {
        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        Debug.Log("SignUp is successful.");
    }
    catch (AuthenticationException ex)
    {
        // Compare error code to AuthenticationErrorCodes
        // Notify the player with the proper error message
        Debug.LogException(ex);
        messages.text = "Exception occures";
    }
    catch (RequestFailedException ex)
    {
        // Compare error code to CommonErrorCodes
        // Notify the player with the proper error message
        Debug.LogException(ex);
    }
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
{
    try
    {
        await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        Debug.Log("SignIn is successful.");
    }
    catch (AuthenticationException ex)
    {
        // Compare error code to AuthenticationErrorCodes
        // Notify the player with the proper error message
        Debug.LogException(ex);
        messages.text = ex.ToString();
    }
    catch (RequestFailedException ex)
    {
        // Compare error code to CommonErrorCodes
        // Notify the player with the proper error message
        Debug.LogException(ex);
    }
}
}
