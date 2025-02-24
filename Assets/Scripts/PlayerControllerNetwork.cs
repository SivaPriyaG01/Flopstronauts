using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerControllerNetwork : NetworkBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator anim;
    private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;
    private float gravityValue = 9.81f;
    private bool groundedPlayer;
    private float turnSmoothTime=0.1f;
    private float turnSmoothVelocity;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = characterController.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; // Reset velocity when on the ground
        }
        
            PlayerMove();
            PlayerJump();

        playerVelocity.y += -gravityValue*Time.deltaTime;
        characterController.Move(playerVelocity*Time.deltaTime);
        
    }

    void PlayerMove()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();

        if(inputVector!=Vector2.zero)
        {
        float horizontal = inputVector.x;
        float vertical = inputVector.y;

        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

        if(direction.magnitude>=0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation=Quaternion.Euler(0f,angle,0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f)*Vector3.forward;
            characterController.Move(moveDirection*playerSpeed*Time.deltaTime);
            anim.SetFloat("Move",Mathf.Clamp(moveDirection.magnitude,0f,1f));
            
        }
        }
        else
        {
            anim.SetFloat("Move",0f);
        }
    }

    void PlayerJump()
    {
        bool jumpPressed = playerInput.actions["Jump"].WasPressedThisFrame();
        if(jumpPressed && groundedPlayer)
        {
            playerVelocity.y=Mathf.Sqrt(-2f * gravityValue * jumpHeight);
            anim.SetTrigger("Jump");
        }
    }
}
