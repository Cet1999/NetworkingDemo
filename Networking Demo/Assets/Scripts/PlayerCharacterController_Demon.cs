using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacterController_Demon : PlayerCharacterController
{
    public GameObject Horns;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManagerRef.PlayerRole == Role.InfectedHuman && !Horns.activeSelf)
        {
            if (GameManager.instance.LocalPlayerManager.PlayerRole == Role.Demon || GameManager.instance.LocalPlayerManager.PlayerRole == Role.InfectedHuman)
            {
                Horns.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasAuthority)
        {
            if (PlayerManagerRef.PlayerRole == Role.Demon && collision.gameObject.tag == "PlayerCharacter")
            {
                Infect(collision.gameObject.GetComponent<PlayerCharacterController_Demon>());
            }
        }
    }

    [Command]
    private void Infect(PlayerCharacterController_Demon Character)
    {
        Character.PlayerManagerRef.PlayerRole = Role.InfectedHuman;
    }

    public void GetInfected()
    {

    }
}
