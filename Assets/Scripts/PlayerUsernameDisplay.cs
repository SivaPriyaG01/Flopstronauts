using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

public class PlayerUsernameDisplay : NetworkBehaviour
{
    NetworkVariable<FixedString64Bytes> displayName = new NetworkVariable<FixedString64Bytes>(LoginSignUpScript.PlayerSession.Username);
    [SerializeField] TMP_Text playerNameDisplayText;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner) return;
        playerNameDisplayText.text=displayName.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
