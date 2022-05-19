using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public PlayerObjectController LocalPlayerManager;
    public PlayerObjectController DemonPlayerManager;
    CustomNetworkManager NetworkManager;
    
    void Start()
    {
        if (!instance)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }

        NetworkManager = FindObjectOfType<CustomNetworkManager>();
        if (LocalPlayerManager == null)
        {
            PlayerObjectController[] AllPlayerManagers = FindObjectsOfType<PlayerObjectController>();
            foreach (PlayerObjectController g in AllPlayerManagers)
            {
                Debug.Log(g.isLocalPlayer);
                if (g.isLocalPlayer)
                {
                    LocalPlayerManager = g;
                }
            }
        }
        if (LocalPlayerManager.isServer)
        {
            Debug.Log("This is Server!");
            int PlayerCount = NetworkManager.GamePlayers.Count;
            foreach(PlayerObjectController player in NetworkManager.GamePlayers)
            {
                player.PlayerRole = Role.Human;
            }
            int Demon = Random.Range(0, PlayerCount);
            NetworkManager.GamePlayers[Demon].PlayerRole = Role.Demon;
        }
        SpawnLocalPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public PlayerObjectController GetDemonCharacter()
    {
        if (DemonPlayerManager != null)
        {
            return DemonPlayerManager;
        }
        else
        {
            PlayerObjectController[] AllPlayerManagers = FindObjectsOfType<PlayerObjectController>();
            foreach (PlayerObjectController g in AllPlayerManagers)
            {
                if (g.PlayerRole == Role.Demon)
                {
                    DemonPlayerManager = g;
                }
            }
            return DemonPlayerManager;
        }
        return null;
    }

    public void SpawnLocalPlayer()
    {
        if (LocalPlayerManager.connectionToServer.isReady)
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