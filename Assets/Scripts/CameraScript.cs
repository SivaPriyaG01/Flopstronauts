using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float cameraOffsetY;
    [SerializeField] float cameraOffsetZ;
    //[SerializeField] float cameraRotationX;
    //float cameraRotationY;
    //[SerializeField] Camera viewCam;
    // Start is called before the first frame update
    void Start()
    {
        //viewCam.transform.rotation = Quaternion.Euler(cameraRotationX,0f,0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraFollow();
    }

    private void CameraFollow()
    {
        var camPosX = Player.position.x;
        var camPosZ = Player.position.z + cameraOffsetZ;
        var camPosY = Player.position.y + cameraOffsetY;

        // var cameraRotationY = transform.rotation.y;

        //transform.position = new Vector3(camPosX, camPosY, camPosZ);
        transform.LookAt(Player);
        //viewCam.transform.rotation = Quaternion.Euler(cameraRotationX,cameraRotationY,0f);
    }
}
