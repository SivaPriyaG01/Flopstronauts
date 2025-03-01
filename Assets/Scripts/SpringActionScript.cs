using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringActionScript : MonoBehaviour
{
    // [SerializeField] private float jumpForce = 15f; // Force applied to the player
    // private Animation anim;

    // private void Start()
    // {
    //     anim = GetComponent<Animation>(); // Get the Animator if the object has one
    // }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player")) // Make sure the player has the "Player" tag
    //     {
    //         Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

    //         if (playerRb != null)
    //         {
    //             // Play the spring animation
    //             if (anim != null)
    //             {
    //                 anim.Play();
    //             }

    //             // Apply jump force
    //             playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z); // Reset vertical velocity
    //             playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //         }
    //     }
    // }
    private Animator anim;
    [SerializeField] private float pushForce = 10f; // Strength of push
    [SerializeField] private float pushDuration = 0.2f; // How long the push effect lasts

    void Start()
    {
        anim = GetComponent<Animator>();  
    }

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
            anim.SetTrigger("Action");
            yield return null;
        }
    }
}
