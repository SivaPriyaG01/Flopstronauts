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
    private Transform cam;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float rotationSpeed = 100f;
    private float gravityValue = 9.81f;
    private bool groundedPlayer;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.2f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        anim=GetComponent<Animator>();
        cam = GameObject.Find("ThirdPersonFollowCam").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;

        groundedPlayer = characterController.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; // Reset velocity when on the ground
        }
            //PlayerMove();
            Move();
            PlayerJump();

        playerVelocity.y += -gravityValue*Time.deltaTime;
        characterController.Move(playerVelocity*Time.deltaTime);
        }


    void Move()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        float verticalInput = inputVector.y;
        float horizontalInput = inputVector.x;

        if(inputVector != Vector2.zero)
        {
            Vector3 direction = (transform.forward*verticalInput + transform.right*horizontalInput).normalized;

            if(direction.magnitude>0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref turnSmoothVelocity,turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
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



    void PlayerMove()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        float forwardMovement = inputVector.y;
        float rotationInput = inputVector.x;

        // Move forward and backward
        Vector3 moveDirection = transform.forward * forwardMovement * playerSpeed * Time.deltaTime;
        characterController.Move(moveDirection);

        // Rotate left/right in place
        if (Mathf.Abs(rotationInput) > 0.1f) // Prevents minor accidental rotation
        {
            float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
        }

        // Animation handling
        anim.SetFloat("Move", Mathf.Abs(forwardMovement));
    }

    void PlayerJump()
    {
        bool jumpPressed = playerInput.actions["Jump"].WasPerformedThisFrame();
        if(jumpPressed && groundedPlayer)
        {
            playerVelocity.y=Mathf.Sqrt(2f * gravityValue * jumpHeight);
            anim.SetTrigger("Jump");
        }
    }
}
