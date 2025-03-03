using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour
{
    private Transform spawnAreaCenter; // This will be found automatically
    [SerializeField] private float areaSize = 3f; // Size of the spawn area
    [SerializeField] private int maxPlayers = 20; // Max number of players

    private List<Vector3> spawnPositions = new List<Vector3>();
    private int nextSpawnIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Try to find the spawn area by name or tag
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

        Vector3 spawnPos = spawnPositions[nextSpawnIndex];
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPositions.Count;

        Transform playerTransform = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.transform;
        playerTransform.position = spawnPos;
    }

    private void OnDestroy()
    {
        if (IsServer && NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
