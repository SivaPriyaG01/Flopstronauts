using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FollowCameraScript : MonoBehaviour
{
    [SerializeField] float cameraOffsetY;
    [SerializeField] float cameraOffsetZ;
    [SerializeField] float cameraRotationX;
    float cameraRotationY;
    [SerializeField] Camera viewCam;
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
        var camPosX = transform.position.x;
        var camPosZ = transform.position.z + cameraOffsetZ;
        var camPosY = transform.position.y + cameraOffsetY;

        var cameraRotationY = transform.rotation.y;

        viewCam.transform.position = new Vector3(camPosX, camPosY, camPosZ);
        viewCam.transform.rotation = Quaternion.Euler(cameraRotationX,cameraRotationY,0f);
    }
}
