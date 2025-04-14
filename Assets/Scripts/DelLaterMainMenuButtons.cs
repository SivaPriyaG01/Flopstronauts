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

    public async void JoinRelay()
    {

    }
}
