using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class PlayerCameraSetup : NetworkBehaviour
{
    private void Start()
    {
        if (IsOwner) // Ensure only the local player assigns the camera
        {
            AssignCamera();
        }
    }

    private void AssignCamera()
    {
        CinemachineFreeLook vCam = FindObjectOfType<CinemachineFreeLook>();  // Find the camera in the scene

        if (vCam != null)
        {
            vCam.Follow = transform;  // Set the camera to follow the player
            vCam.LookAt = transform;  // Set the camera to look at the player
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera not found in the scene!");
        }
    }
}
