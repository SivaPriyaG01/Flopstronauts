using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DelLaterMainMenuButtons : NetworkBehaviour
{
    [SerializeField] int maxConnections;
    [SerializeField] TMP_Text code;
    [SerializeField] TMP_InputField joinCode;
    [SerializeField] GameObject hostPanel;
    [SerializeField] GameObject joinPanel;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        if(!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch(AuthenticationException e)
            {
                Debug.Log(e);
            }
        }

        hostPanel.SetActive(false);
        joinPanel.SetActive(false);
    }

    // Update is called once per frame

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public async void CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        code.text = "Code: "+joinCode;

        var relayServerData = new RelayServerData(allocation,"dtls");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
    }

    public async void JoinRelay(string joinCode)
    {
        if(!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        try
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var relayServerData = new RelayServerData(joinAllocation,"dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public void EnterHostPlay()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started successfully. Loading GameScene...");
            
            // Step 4: Load the game scene using Netcode
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("Failed to start host!");
        }

    }

}
