using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRbCtrDelLater : MonoBehaviour
{
    Rigidbody rb;
    PlayerInput playerInput;
    Animator anim;
    Transform cam;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float playerSpeed = 10f;
    bool jumpPressed;
    float turnSmoothVelocity;
    float turnSmoothTime = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        anim=GetComponent<Animator>();
        cam = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector2 inputVector  = playerInput.actions["Move"].ReadValue<Vector2>();
        float horizontal = inputVector.x;
        float vertical = inputVector.y;

        if(inputVector!=Vector2.zero)
        {
        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

        if(direction.magnitude>0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z) + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(rb.transform.eulerAngles.y, targetAngle,ref turnSmoothVelocity,turnSmoothTime);

            Vector3 moveDirection = Quaternion.Euler(0f,angle,0f)*Vector3.forward;

            rb.MoveRotation(Quaternion.Euler(0f,angle,0f));
            rb.MovePosition(rb.position+moveDirection*playerSpeed*Time.deltaTime);
            anim.SetFloat("Move",Mathf.Clamp(moveDirection.magnitude, 0f, 1f));
        }
        }
        else
        {
            anim.SetFloat("Move",0f);    
        }
    }

    void Jump()
    {
        jumpPressed = playerInput.actions["Jump"].WasPressedThisFrame();
        if(jumpPressed)
        {
            rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }
}
