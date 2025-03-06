using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class PlayerMaterialManager : NetworkBehaviour
{
    [SerializeField] private List<Material> playerMaterials; // Assign in Inspector
    private Renderer playerRenderer;

    // NetworkVariable to store the material index (syncs with all clients)
    private NetworkVariable<int> selectedMaterialIndex = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Get the selected material index from PlayerPrefs
            int savedMaterialIndex = PlayerPrefs.GetInt("SelectedMaterialIndex", 0);
            
            // Request the server to set this material index
            SetMaterialOnServerRpc(savedMaterialIndex);
        }

        // Apply the material when it changes (ensures all players see the same color)
        selectedMaterialIndex.OnValueChanged += (oldValue, newValue) =>
        {
            ApplyMaterial(newValue);
        };

        // Apply the material initially in case it was already set
        ApplyMaterial(selectedMaterialIndex.Value);
    }

    [ServerRpc]
    private void SetMaterialOnServerRpc(int index)
    {
        if (index >= 0 && index < playerMaterials.Count)
        {
            selectedMaterialIndex.Value = index; // This will sync with all clients
        }
    }

    private void ApplyMaterial(int index)
    {
        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInChildren<Renderer>(); // Finds the correct renderer in the hierarchy
        }

        if (playerRenderer != null && index >= 0 && index < playerMaterials.Count)
        {
            playerRenderer.material = playerMaterials[index];
        }
    }
}
