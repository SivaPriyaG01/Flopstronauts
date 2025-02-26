using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using Unity.Netcode;
using Unity.Mathematics;
using Unity.VisualScripting;

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
    [SerializeField] private float rotationSpeed = 100f;

    
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
        //Move();
        PlayerMovement();
        Jump();
    }

//     void Move()
// {
//     Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

//         if(inputVector!=Vector2.zero)
//         {
//         float horizontal = inputVector.x;
//         float vertical = inputVector.y;

//         Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

//         if(direction.magnitude>=0.1f)
//         {
//             float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
//             float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnSmoothVelocity, turnSmoothTime);
//             transform.rotation=Quaternion.Euler(0f,angle,0f);

//             //Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f)*Vector3.forward;
//             rb.MovePosition(rb.position + direction*moveSpeed*Time.deltaTime);
//             anim.SetFloat("Move",Mathf.Clamp(direction.magnitude,0f,1f));
            
//         }
//         }
//         else
//         {
//             anim.SetFloat("Move",0f);
//             turnSmoothVelocity=0f;
//         }
// }

    void PlayerMovement()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        
        if(inputVector != Vector2.zero)
        {
            float forwardInput = inputVector.y;
            float rotationInput = inputVector.x;

            if(Mathf.Abs(rotationInput)>0.1f)
            {
                float rotationAmount = rotationInput*rotationSpeed*Time.fixedDeltaTime;
                rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotationAmount, 0f));
            }

            Vector3 moveDirection = transform.forward * forwardInput * moveSpeed;
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
            anim.SetFloat("Move",Mathf.Abs(forwardInput));
        }
        else
        {
            anim.SetFloat("Move",0f);
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
