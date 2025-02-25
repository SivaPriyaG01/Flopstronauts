using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotateProps1Script : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] Vector3 torque = new Vector3 (0f,1f,0f);
    [SerializeField] Vector3 force = new Vector3 (0f,45f,0f);
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(torque);
        //rb.AddForce(force,ForceMode.Impulse);
    }
}
