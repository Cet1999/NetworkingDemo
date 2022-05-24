using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacterController_BombTag : PlayerCharacterController
{
    public Transform BombHoldingPosition;
    public float InvincibleTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InvincibleTime > 0)
        {
            InvincibleTime -= Time.deltaTime;
        }
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
        if (Character.InvincibleTime <= 0)
        {
            PlayerManagerRef.HoldingBomb = false;
            Character.PlayerManagerRef.HoldingBomb = true;
            InvincibleTime += 2;
        }
    }
}
