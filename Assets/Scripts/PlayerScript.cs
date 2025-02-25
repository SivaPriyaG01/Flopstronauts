using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Mathematics;

public class PlayerScript : NetworkBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator anim;
    private float turnSmoothTime=0.2f;
    private float turnSmoothVelocity;
    private float currentXInput=0f;
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float jumpHeight = 20f;
    [SerializeField] float turnSpeed = 8f;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        Move();
        Jump();
    }

    void Move()
{
    Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
    float xAxisInput= inputVector.x;
    float zAxisInput = inputVector.y;

    

    if (inputVector != Vector2.zero)
    {
        Vector3 direction = (transform.forward*zAxisInput + transform.right*xAxisInput).normalized;
        currentXInput = Mathf.SmoothDamp(currentXInput,xAxisInput,ref turnSmoothVelocity, turnSmoothTime);
        if (direction.magnitude >= 0.1f)
        {
            
            float targetAngle = Mathf.Atan2(currentXInput, direction.z) * Mathf.Rad2Deg;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
            anim.SetFloat("Move", Mathf.Clamp(direction.magnitude, 0f, 1f));
        }
    }
    else
    {
        anim.SetFloat("Move", 0f);
        turnSmoothVelocity = 0f; // Reset turn smoothing when not moving
    }
}


    void Jump()
    {
        bool jumpPressed = playerInput.actions["Jump"].WasPressedThisFrame();

        if(jumpPressed)
        {
            rb.AddForce(transform.up*jumpHeight*Time.deltaTime, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }
}
