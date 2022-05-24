using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacterController : NetworkBehaviour
{
    public NameTag NameTagRef;
    [SyncVar(hook = nameof(UpdatePlayerName))] public PlayerObjectController PlayerManagerRef;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayerName(PlayerObjectController Old, PlayerObjectController New)
    {
        NameTagRef.UpdateName(New.PlayerName);
    }
}
