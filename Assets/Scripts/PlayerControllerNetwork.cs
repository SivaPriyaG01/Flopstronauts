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
    [SerializeField] private float jumpHeight = 100f;
    [SerializeField] private float rotationSpeed = 100f;
    private float gravityValue = 9.81f;
    private bool groundedPlayer;
    
    
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
        bool jumpPressed = playerInput.actions["Jump"].WasPressedThisFrame();
        if(jumpPressed && groundedPlayer)
        {
            playerVelocity.y=Mathf.Sqrt(-2f * gravityValue * jumpHeight);
            anim.SetTrigger("Jump");
        }
    }
}
