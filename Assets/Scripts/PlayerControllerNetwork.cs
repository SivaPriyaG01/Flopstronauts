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
    
    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponentInChildren<CharacterController>();
        anim=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
