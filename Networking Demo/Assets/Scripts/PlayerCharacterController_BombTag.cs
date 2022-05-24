using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacterController_BombTag : PlayerCharacterController
{
    public Transform BombHoldingPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasAuthority && collision.gameObject.tag == "PlayerCharacter")
        {
            if (PlayerManagerRef.HoldingBomb)
            {
                PassBomb(collision.gameObject.GetComponent<PlayerCharacterController_BombTag>());
            }
        }
    }

    [Command]
    private void PassBomb(PlayerCharacterController_BombTag Character)
    {
        PlayerManagerRef.HoldingBomb = false;
        Character.PlayerManagerRef.HoldingBomb = true;
    }
}
