using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffRbScriptDelLater : MonoBehaviour
{
    [SerializeField] float bounceForce = 10f;// Start is called before the first frame update
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            playerRb.AddForce(transform.right*bounceForce,ForceMode.Impulse);
        }
    }
}
