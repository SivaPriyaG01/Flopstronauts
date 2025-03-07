using UnityEngine;
using Unity.Netcode;

public class PlayerRespawner : NetworkBehaviour
{
    private Transform startPosition; // Initial spawn position
    private Vector3 lastCheckpoint; // Stores last checkpoint position

    private void Start()
    {
        startPosition = GameObject.Find("SpawnAreaCenter").transform;
        if(startPosition!=null)
        {Debug.Log("start position setup complete");}
        else{Debug.Log("start position not set");}
        
        if (IsOwner) // Only the local player needs to track this
        {
            lastCheckpoint = startPosition.position; // Default respawn position
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return; // Only the local player processes checkpoints

        if (other.CompareTag("Ground")) // If player falls
        {
            Respawn();
            Debug.Log("Respawning");
        }
        else if (other.CompareTag("Checkpoint")) // Save checkpoint
        {
            lastCheckpoint = other.transform.position;
            Debug.Log("Checkpoint reached");
        }
        else if(other.CompareTag("FinishLine"))
        {
            Debug.Log("You won");
        }
    }

    private void Respawn()
    {
        transform.position = lastCheckpoint; // Move locally
        RequestSyncRespawnServerRpc(lastCheckpoint); // Sync with server
    }

    [ServerRpc]
    private void RequestSyncRespawnServerRpc(Vector3 respawnPosition)
    {
        SyncRespawnClientRpc(respawnPosition);
    }

    [ClientRpc]
    private void SyncRespawnClientRpc(Vector3 respawnPosition)
    {
        if (!IsOwner) // Don't override local movement
        {
            transform.position = respawnPosition;
        }
    }
}
