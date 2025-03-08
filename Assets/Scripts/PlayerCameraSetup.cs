// using UnityEngine;
// using Cinemachine;
// using Unity.Netcode;

// public class PlayerCameraSetup : NetworkBehaviour
// {
//     private void Start()
//     {
//         if (IsOwner) // Ensure only the local player assigns the camera
//         {
//             AssignCamera();
//         }
//     }

//     private void AssignCamera()
//     {
//         CinemachineFreeLook vCam = FindObjectOfType<CinemachineFreeLook>();  // Find the camera in the scene

//         if (vCam != null)
//         {
//             vCam.Follow = transform;  // Set the camera to follow the player
//             vCam.LookAt = transform;  // Set the camera to look at the player
//         }
//         else
//         {
//             Debug.LogError("Cinemachine Virtual Camera not found in the scene!");
//         }
//     }
// }


// using UnityEngine;
// using Cinemachine;
// using Unity.Netcode;

// public class PlayerCameraSetup : NetworkBehaviour
// {
//     public CinemachineFreeLook vCam;
    
//     private void OnNetworkSpawn()
//     {
//         if (IsOwner) // Ensure only the local player assigns the camera
//         {
//             AssignCamera();
//         }
//     }

//     private void AssignCamera()
//     {
//          vCam = FindObjectOfType<CinemachineFreeLook>();  // Find the camera in the scene
         

//         if (vCam != null)
//         {
//             vCam.Follow = transform;  // Set the camera to follow the player
//             vCam.LookAt = transform;  // Set the camera to look at the player
//             Debug.Log("Camera assigned to player: " + gameObject.name);
//         }
//         else
//         {
//             Debug.LogError("Cinemachine Virtual Camera not found in the scene!");
//         }
//     }
// }

using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class PlayerCameraSetup : NetworkBehaviour
{
    public CinemachineFreeLook vCam;

    private void Start()
    {
        if (!IsOwner) return; // Only assign camera for the local player

        AssignCamera();
    }

    private void AssignCamera()
    {
        // Find the Cinemachine FreeLook Camera by tag
        GameObject camObj = GameObject.FindWithTag("VirtualCamera");

        if (camObj != null)
        {
            vCam = camObj.GetComponent<CinemachineFreeLook>();

            if (vCam != null)
            {
                vCam.Follow = transform;
                vCam.LookAt = transform;
                vCam.gameObject.SetActive(true);
                Debug.Log("Camera assigned to player: " + gameObject.name);
            }
            else
            {
                Debug.LogError("CinemachineFreeLook component not found on the camera object!");
            }
        }
        else
        {
            Debug.LogError("Cinemachine FreeLook Camera not found! Make sure it is tagged 'VirtualCamera'.");
        }
    }
}

