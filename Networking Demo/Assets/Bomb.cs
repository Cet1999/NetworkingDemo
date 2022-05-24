using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bomb : MonoBehaviour
{
    PlayerCharacterController_BombTag BombHoldingPlayerCharacterRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BombHoldingPlayerCharacterRef == null)
        {
            BombHoldingPlayerCharacterRef = GameManager.instance.GetBombHoldingCharacter().ActivePlayerCharacter.GetComponent<PlayerCharacterController_BombTag>();
        }
        transform.position += (BombHoldingPlayerCharacterRef.BombHoldingPosition.position - transform.position) / 5;
    }
}
