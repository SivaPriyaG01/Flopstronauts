using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class HostDisconnectionHandler : NetworkBehaviour
{
   private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            // If the host disconnects, send all players back to the main menu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
