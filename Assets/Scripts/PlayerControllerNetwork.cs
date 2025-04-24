using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Unity.Collections;
using TMPro;

public class PlayerControllerNetwork : NetworkBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator anim;
    private Vector3 playerVelocity;
    private GameObject cam;
    private AudioSource audio;
    private TMP_Text playerNameDisplay;
    [SerializeField] AudioClip onCollisionClip;
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float rotationSpeed = 100f;

    private float gravityValue = 9.81f;
    private bool groundedPlayer;
    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.2f;

    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>(
    writePerm: NetworkVariableWritePermission.Owner);


    void Start()
    {
        // Ensure ownership check before assigning variables
        if (!IsOwner) return;

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing!");
        }

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing!");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning("Animator component is missing!");
        }

        cam = GameObject.FindWithTag("MainCamera");
        audio = GetComponent<AudioSource>();

        PlayerName.Value = LoginSignUpScript.PlayerSession.Username;
        playerNameDisplay.text=PlayerName.Value.ToString();
    }

    void Update()
    {
        if (!IsOwner) return;

        groundedPlayer = characterController.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f; // Reset velocity when on the ground
        }

        Move();
        PlayerJump();

        playerVelocity.y += -gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void Move()
    {
        if (playerInput == null) return; // Ensure input is valid

        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        float verticalInput = inputVector.y;
        float horizontalInput = inputVector.x;

        if (inputVector != Vector2.zero)
        {
            //Vector3 direction = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
            Vector3 direction = new Vector3(horizontalInput,0,verticalInput).normalized;
            if (direction.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.gameObject.transform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);

                if (anim != null)
                {
                    anim.SetFloat("Move", Mathf.Clamp(moveDirection.magnitude, 0f, 1f));
                }
            }
        }
        else
        {
            if (anim != null)
            {
                anim.SetFloat("Move", 0f);
            }
        }
    }

    void PlayerJump()
    {
        if (playerInput == null) return; // Ensure input is valid

        bool jumpPressed = playerInput.actions["Jump"].WasPerformedThisFrame();
        if (jumpPressed && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(2f * gravityValue * jumpHeight);
            if (anim != null)
            {
                anim.SetTrigger("Jump");
            }
        }
    }

    // private void OnTriggerEnter(Collider other) 
    // {
    //     if(other.gameObject.CompareTag("Obstacle"))
    //     {
    //         audio.PlayOneShot(onCollisionClip);
    //     }    
    // }
    
}

