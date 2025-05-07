using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    private Dictionary<ulong, bool> playerQualifications = new Dictionary<ulong, bool>();
    [SerializeField] private int playersNeededToQualify = 2; // You can set dynamically based on player count

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }
    }

    void OnClientConnected(ulong clientId)
    {
        if (!playerQualifications.ContainsKey(clientId))
            playerQualifications[clientId] = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void QualifyPlayerServerRpc(ulong clientId)
    {
        if (!IsServer) return;

        playerQualifications[clientId] = true;
        Debug.Log($"Player {clientId} qualified!");

        int qualifiedCount = 0;
        foreach (var kvp in playerQualifications)
        {
            if (kvp.Value) qualifiedCount++;
        }

        if (qualifiedCount >= playersNeededToQualify)
        {
            LoadNextRound();
        }
    }

    void LoadNextRound()
    {
        // Broadcast to all players
        NetworkManager.SceneManager.LoadScene("GameScene1", LoadSceneMode.Single);
    }
}
