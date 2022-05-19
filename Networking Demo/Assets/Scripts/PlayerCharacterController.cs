using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCharacterController : NetworkBehaviour
{
    public NameTag NameTagRef;
    [SyncVar (hook = nameof(UpdatePlayerName))] public PlayerObjectController PlayerManagerRef;
    public GameObject Horns;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerManagerRef.PlayerRole == Role.Demon && collision.gameObject.tag == "PlayerCharacter")
        {
            Infect(collision.gameObject.GetComponent<PlayerCharacterController>());
        }
    }

    [Command]
    private void Infect(PlayerCharacterController Character)
    {
        Character.PlayerManagerRef.PlayerRole = Role.InfectedHuman;
        Character.GetInfected();
    }

    public void GetInfected()
    {
        if (GameManager.instance.LocalPlayerManager.PlayerRole == Role.Demon || GameManager.instance.LocalPlayerManager.PlayerRole == Role.InfectedHuman)
        {
            Horns.SetActive(true);
        }
    }
}
