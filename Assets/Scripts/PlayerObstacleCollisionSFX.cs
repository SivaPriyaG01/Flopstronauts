using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObstacleCollisionSFX : MonoBehaviour
{
    AudioManager audioManager;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            audioManager.PlayOnCollision();
        }
    }

}
