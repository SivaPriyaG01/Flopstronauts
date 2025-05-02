using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using TMPro;

public class PlayerUsernameDisplay : NetworkBehaviour
{
    NetworkVariable<FixedString64Bytes> displayName = new NetworkVariable<FixedString64Bytes>();
    [SerializeField] TMP_Text playerNameDisplayText;
    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner) return;

        if(LoginSignUpScript.PlayerSession.Username!=null)
        {
            displayName.Value=new FixedString64Bytes(LoginSignUpScript.PlayerSession.Username);
            playerNameDisplayText.text=displayName.ToString();
        }
        else
        {
            Debug.LogWarning("PlayerSession.Username is null");
            playerNameDisplayText.text="Unknown";
        }
        
    }
}
