using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringActionScript : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f; // Force applied to the player
    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>(); // Get the Animator if the object has one
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Play the spring animation
                if (anim != null)
                {
                    anim.Play();
                }

                // Apply jump force
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z); // Reset vertical velocity
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
