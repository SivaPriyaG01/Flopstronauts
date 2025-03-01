using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float followHeight = 15f;
    [SerializeField] float zOffset;
    [SerializeField] float xOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float playerXPos = player.position.x;
        float playerZPos = player.position.z;
        float playerYPos = player.position.y;

        transform.position = new Vector3(playerXPos - xOffset,playerYPos+followHeight,playerZPos - zOffset);
        //transform.LookAt(player,Vector3.up);
    }
}
