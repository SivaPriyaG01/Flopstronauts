// using UnityEngine;
// using Unity.Netcode;

// public class PlayerRespawner : NetworkBehaviour
// {
//     private Transform startPosition; // Initial spawn position
//     private Vector3 lastCheckpoint; // Stores last checkpoint position

//     private void Start()
//     {
//         startPosition = GameObject.Find("SpawnAreaCenter").transform;
//         if(startPosition!=null)
//         {Debug.Log("start position setup complete");}
//         else{Debug.Log("start position not set");}
        
//         if (IsOwner) // Only the local player needs to track this
//         {
//             lastCheckpoint = startPosition.position; // Default respawn position
//         }
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (!IsOwner) return; // Only the local player processes checkpoints

//         if (other.CompareTag("Ground")) // If player falls
//         {
//             Respawn();
//             Debug.Log("Respawning");
//         }
//         else if (other.CompareTag("Checkpoint")) // Save checkpoint
//         {
//             lastCheckpoint = other.transform.position;
//             Debug.Log("Checkpoint reached");
//         }
//         else if(other.CompareTag("FinishLine"))
//         {
//             Debug.Log("You won");
//         }
//     }

//     private void Respawn()
//     {
//         transform.position = lastCheckpoint; // Move locally
//         RequestSyncRespawnServerRpc(lastCheckpoint); // Sync with server
//     }

//     [ServerRpc]
//     private void RequestSyncRespawnServerRpc(Vector3 respawnPosition)
//     {
//         SyncRespawnClientRpc(respawnPosition);
//     }

//     [ClientRpc]
//     private void SyncRespawnClientRpc(Vector3 respawnPosition)
//     {
//         if (!IsOwner) // Don't override local movement
//         {
//             transform.position = respawnPosition;
//         }
//     }
// }


using UnityEngine;
using Unity.Netcode;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerRespawner : NetworkBehaviour
{
    private Transform startPosition; // Initial spawn position
    private Vector3 lastCheckpoint; // Stores last checkpoint position
    private TextMeshProUGUI messages;

    private void Start()
    {
        StartCoroutine(FindSpawnArea());
        messages=GameObject.FindWithTag("Messages").GetComponent<TextMeshProUGUI>();
        DisplayMessages("Game Start!");
    }

    private IEnumerator FindSpawnArea()
    {
        yield return new WaitForSeconds(0.5f); // Small delay to allow objects to load

        GameObject spawnArea = GameObject.Find("SpawnAreaCenter");
        if (spawnArea != null)
        {
            startPosition = spawnArea.transform;
            Debug.Log("start position setup complete");
        }
        else
        {
            Debug.LogError("SpawnAreaCenter not found! Make sure it exists in the scene.");
            yield break; // Stop execution if not found
        }

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
            DisplayMessages("Respawning");
        }
        else if (other.CompareTag("Checkpoint")) // Save checkpoint
        {
            lastCheckpoint = other.transform.position;
            Debug.Log("Checkpoint reached");
            DisplayMessages("CheckPoint reached");
        }
        else if (other.CompareTag("FinishLine"))
        {
            Debug.Log("You won!");
            DisplayMessages("COURSE COMPLETED!! Return to Main Menu");
        }
    }

    private void Respawn()
    {
        // transform.position = lastCheckpoint; // Move locally
        // RequestSyncRespawnServerRpc(lastCheckpoint); // Sync with server
        CharacterController controller = GetComponent<CharacterController>();
    if (controller != null)
    {
        controller.enabled = false; // Disable before teleporting
    }

    transform.position = lastCheckpoint; // Move locally
    RequestSyncRespawnServerRpc(lastCheckpoint); // Sync with server

    if (controller != null)
    {
        controller.enabled = true; // Re-enable after teleporting
    }

    Debug.Log("Respawned at: " + lastCheckpoint);
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

    private void DisplayMessages(string msg)
    {
        messages.text = msg;
        StartCoroutine(DisappearMessage());
    }

    IEnumerator DisappearMessage()
    {
        yield return new WaitForSeconds(5);
        messages.text = "";
    }
}
