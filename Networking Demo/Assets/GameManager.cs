using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    PlayerObjectController LocalPlayerManager;
    CustomNetworkManager NetworkManager;
    
    void Start()
    {
        NetworkManager = FindObjectOfType<CustomNetworkManager>();
        PlayerObjectController[] AllPlayerManagers = FindObjectsOfType<PlayerObjectController>();
        foreach(PlayerObjectController g in AllPlayerManagers)
        {
            if (g.isLocalPlayer)
            {
                LocalPlayerManager = g;
            }
        }
        SpawnLocalPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnLocalPlayer()
    {
        if (LocalPlayerManager.connectionToClient.isReady)
        {
            LocalPlayerManager.CmdSpawnPlayerCharacter();
        }
        else
        {
            StartCoroutine(WaitForReady());
        }
    }

    IEnumerator WaitForReady()
    {
        while (!LocalPlayerManager.connectionToClient.isReady)
        {
            yield return new WaitForSeconds(0.25f);
        }
        SpawnLocalPlayer();
    }


    /*public override void OnStartServer() => CustomNetworkManager.OnServerReadied += SpawnPlayer;

    public void SpawnPlayer(NetworkConnection conn)
    {
        if ()
    }*/
}