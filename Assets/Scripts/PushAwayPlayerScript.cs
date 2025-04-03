using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAwayPlayerScript : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f; // Strength of push
    [SerializeField] private float pushDuration = 0.2f; // How long the push effect lasts

    private void OnTriggerStay(Collider other)
    {
        CharacterController playerController = other.GetComponent<CharacterController>();

        if (playerController != null)
        {
            StartCoroutine(PushPlayer(playerController, other.transform));
        }
    }

    private IEnumerator PushPlayer(CharacterController playerController, Transform hitTransform)
    {
        Vector3 pushDirection = transform.forward; // Direction away from blade
        // pushDirection.y = 0; // Keep the force horizontal
        // pushDirection.Normalize();

        float elapsedTime = 0f;
        while (elapsedTime < pushDuration)
        {
            playerController.Move(pushDirection * pushForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Vector3 PushDirectionByObstacle(string obstacle)
    // {
    //     switch (obstacle)
    //     {
    //         case "Blade":
    //         return transform.forward;
    //         case "Spikes":
    //         return transform.up;
    //         default:
    //         return transform.up;
    //     }
    // }
}
