using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinishLineReached : MonoBehaviour
{
    List<string> playerOrder = new List<string>();

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("player"))
        {
            var player = collision.gameObject;
            var playerName = player.GetComponent<PlayerUsernameDisplay>().ReturnUsername();
            playerOrder.Add(playerName);
        }
    }


}
