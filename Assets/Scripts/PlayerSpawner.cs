// using UnityEngine;
// using Unity.Netcode;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine.SceneManagement;


// public class PlayerSpawner : NetworkBehaviour
// {
//     private Transform spawnAreaCenter; // Found automatically
//     [SerializeField] private GameObject playerPrefab; // Assign the player prefab in the Inspector
//     [SerializeField] private float areaSize = 3f; // Size of the spawn area
//     [SerializeField] private int maxPlayers = 20; // Maximum number of players
//     [SerializeField] List<Material> playerMaterial;

//     private List<Vector3> spawnPositions = new List<Vector3>();
//     private int nextSpawnIndex = 0;

//     public void Start()
//     {
        
//     }

//     public override void OnNetworkSpawn()
//     {
//          Debug.Log("PlayerSpawner: OnNetworkSpawn called.");
         
//          if (IsServer)
//          {
//             Debug.Log("PlayerSpawner: Running on Server.");         

//         NetworkManager.Singleton.SceneManager.OnLoadComplete += (ulong clientId, string sceneName, LoadSceneMode loadMode) =>
//         {
//             if (sceneName == "GameScene") // Ensure this only runs in the Game Scene
//             {
//                 GameObject spawnAreaObject = GameObject.Find("SpawnAreaCenter");

//                 if (spawnAreaObject == null)
//                 {
//                     Debug.LogError("SpawnAreaCenter not found in the scene! Make sure it exists.");
//                     return;
//                 }

//                 spawnAreaCenter = spawnAreaObject.transform;
//                 GenerateRandomSpawnPositions(maxPlayers);
//                 NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//             }
//         };
//     }
//     }

//     private void GenerateRandomSpawnPositions(int count)
//     {
//         spawnPositions.Clear();
//         for (int i = 0; i < count; i++)
//         {
//             Vector3 randomPos = spawnAreaCenter.position + new Vector3(
//                 Random.Range(-areaSize / 2, areaSize / 2),
//                 0,
//                 Random.Range(-areaSize / 2, areaSize / 2)
//             );
//             spawnPositions.Add(randomPos);
//         }
//     }

//     private void OnClientConnected(ulong clientId)
//     {
//         if (spawnPositions.Count == 0)
//         {
//             Debug.LogError("No spawn positions available! generating...");
//             GenerateRandomSpawnPositions(maxPlayers);
//             //return;
//         }

//         // Select a spawn position
//         Vector3 spawnPos = spawnPositions[nextSpawnIndex];
//         nextSpawnIndex = (nextSpawnIndex + 1) % spawnPositions.Count;


//         // Instantiate the player prefab
//         GameObject playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

//         // Get the NetworkObject component and assign ownership
//         NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
//         if (networkObject != null)
//         {
//             networkObject.SpawnWithOwnership(clientId);
//         }        
//     }

//     private void OnDestroy()
//     {
//         if (IsServer && NetworkManager.Singleton != null)
//         {
//             NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
//         }
//     }

// }

using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    private Transform spawnAreaCenter;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private float areaSize = 3f;
    [SerializeField] private int maxPlayers = 20;
    [SerializeField] List<Material> playerMaterial;

    private List<Vector3> spawnPositions = new List<Vector3>();
    private int nextSpawnIndex = 0;

    public void Start()
    {
        if (IsServer)
        {
            Debug.Log("PlayerSpawner: Running on Server."); 

            SetupSpawnArea();

            NetworkManager.Singleton.SceneManager.OnLoadComplete += (ulong clientId, string sceneName, LoadSceneMode loadMode) =>
            {
                Debug.Log($"Scene Loaded: {sceneName}");
                
                if (sceneName == "GameScene") // Ensure it runs only in the correct scene
                {
                    NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                }
            };

            // If host starts first, spawn them manually
            if (NetworkManager.Singleton.IsHost)
            {
                OnClientConnected(NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    private void SetupSpawnArea()
    {
        GameObject spawnAreaObject = GameObject.Find("SpawnAreaCenter");
        if (spawnAreaObject == null)
        {
            Debug.LogError("SpawnAreaCenter not found in the scene! Make sure it exists.");
            return;
        }

        spawnAreaCenter = spawnAreaObject.transform;
        GenerateRandomSpawnPositions(maxPlayers);
    }

    private void GenerateRandomSpawnPositions(int count)
    {
        spawnPositions.Clear();
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = spawnAreaCenter.position + new Vector3(
                Random.Range(-areaSize / 2, areaSize / 2),
                0,
                Random.Range(-areaSize / 2, areaSize / 2)
            );
            spawnPositions.Add(randomPos);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"OnClientConnected: Spawning player for Client ID: {clientId}");

        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No spawn positions available! Generating new positions...");
            GenerateRandomSpawnPositions(maxPlayers);
        }

        Vector3 spawnPos = spawnPositions[nextSpawnIndex];
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPositions.Count;

        if (playerPrefab == null)
        {
            Debug.LogError("PlayerSpawner: Player prefab is null!");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"Player spawned at: {spawnPos}");

        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            Debug.Log("Spawning player with ownership...");
            networkObject.SpawnWithOwnership(clientId);
        }
        else
        {
            Debug.LogError("NetworkObject component missing on Player Prefab!");
        }
    }

    private void OnDestroy()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}

