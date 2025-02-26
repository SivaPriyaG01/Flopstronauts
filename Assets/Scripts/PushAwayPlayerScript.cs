using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAwayPlayerScript : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f; // Strength of push

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody; // Get Rigidbody of the other object
        if (otherRb != null)
        {
            Vector3 pushDirection = collision.transform.position - transform.position; // Direction away from this object
            pushDirection.y = 0; // Optional: Keep the force horizontal
            otherRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        }
    }
}
