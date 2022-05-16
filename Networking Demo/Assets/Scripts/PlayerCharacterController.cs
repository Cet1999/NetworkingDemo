using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacterController : NetworkBehaviour
{
    public NameTag NameTagRef;
    [SyncVar] public PlayerObjectController PlayerManagerRef;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePlayerName(string name)
    {
        NameTagRef.UpdateName(name);
    }
}
