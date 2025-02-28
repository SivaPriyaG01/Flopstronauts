using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAwayPlayerScript : MonoBehaviour
{
    // [SerializeField] private float pushForce = 10f; // Strength of push

    // private void OnCollisionEnter(Collision collision)
    // {
    //     Rigidbody otherRb = collision.rigidbody; // Get Rigidbody of the other object
    //     if (otherRb != null)
    //     {
    //         Vector3 pushDirection = collision.transform.position - transform.position; // Direction away from this object
    //         pushDirection.y = 0; // Optional: Keep the force horizontal
    //         otherRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
    //     }
    // }

    // [SerializeField] private float pushForce = 10f; // Strength of push
    // [SerializeField] private float pushDuration = 0.2f; // How long the push effect lasts

    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     CharacterController playerController = hit.controller;

    //     if (playerController != null)
    //     {
    //         StartCoroutine(PushPlayer(playerController, hit));
    //     }
    // }

    // private IEnumerator PushPlayer(CharacterController playerController, ControllerColliderHit hit)
    // {
    //     Vector3 pushDirection = hit.transform.position - transform.position; // Direction away from this object
    //     pushDirection.y = 0; // Optional: Keep the force horizontal
    //     pushDirection.Normalize();

    //     float elapsedTime = 0f;
    //     while (elapsedTime < pushDuration)
    //     {
    //         playerController.Move(pushDirection * pushForce * Time.deltaTime);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }

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
        Vector3 pushDirection = hitTransform.position - transform.position; // Direction away from blade
        pushDirection.y = 0; // Keep the force horizontal
        pushDirection.Normalize();

        float elapsedTime = 0f;
        while (elapsedTime < pushDuration)
        {
            playerController.Move(pushDirection * pushForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
