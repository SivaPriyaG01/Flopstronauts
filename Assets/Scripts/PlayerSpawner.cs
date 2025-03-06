using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEditor;

public class PlayerSpawner : NetworkBehaviour
{
    private Transform spawnAreaCenter; // Found automatically
    [SerializeField] private GameObject playerPrefab; // Assign the player prefab in the Inspector
    [SerializeField] private float areaSize = 3f; // Size of the spawn area
    [SerializeField] private int maxPlayers = 20; // Maximum number of players
    [SerializeField] List<Material> playerMaterial;

    private List<Vector3> spawnPositions = new List<Vector3>();
    private int nextSpawnIndex = 0;

    public void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Find the spawn area dynamically
            GameObject spawnAreaObject = GameObject.Find("SpawnAreaCenter");

            if (spawnAreaObject == null)
            {
                Debug.LogError("SpawnAreaCenter not found in the scene! Make sure it exists.");
                return;
            }

            spawnAreaCenter = spawnAreaObject.transform;

            GenerateRandomSpawnPositions(maxPlayers);
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
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
        if (spawnPositions.Count == 0)
        {
            Debug.LogError("No spawn positions available!");
            return;
        }

        // Select a spawn position
        Vector3 spawnPos = spawnPositions[nextSpawnIndex];
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPositions.Count;


        // Instantiate the player prefab
        GameObject playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // Get the NetworkObject component and assign ownership
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(clientId);
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
