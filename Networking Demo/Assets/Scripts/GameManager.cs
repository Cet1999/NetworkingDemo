using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum Mode { BombTag, Demon };

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public Mode GameMode;
    CustomNetworkManager NetworkManager;

    public PlayerObjectController LocalPlayerManager;
    public PlayerObjectController DemonPlayerManager;

    [Header("BombTag")]
    public PlayerObjectController BombPlayerManager;
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
        //Find local game player
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
            //Demon Mode
            if (GameMode == Mode.Demon)
            {
                int PlayerCount = NetworkManager.GamePlayers.Count;
                foreach (PlayerObjectController player in NetworkManager.GamePlayers)
                {
                    player.PlayerRole = Role.Human;
                }
                int Demon = Random.Range(0, PlayerCount);
                NetworkManager.GamePlayers[Demon].PlayerRole = Role.Demon;
            }
            else if (GameMode == Mode.BombTag)
            {
                int PlayerCount = NetworkManager.GamePlayers.Count;
                int FirstBombPlayer = Random.Range(0, PlayerCount);
                NetworkManager.GamePlayers[FirstBombPlayer].HoldingBomb = true;
            }
        }
        SpawnLocalPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //General Methods
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

    //BombTag Mode Specific
    public PlayerObjectController GetBombHoldingCharacter()
    {
        if (BombPlayerManager != null)
        {
            return BombPlayerManager;
        }
        else
        {
            PlayerObjectController[] AllPlayerManagers = FindObjectsOfType<PlayerObjectController>();
            foreach (PlayerObjectController g in AllPlayerManagers)
            {
                if (g.HoldingBomb)
                {
                    BombPlayerManager = g;
                }
            }
            return BombPlayerManager;
        }
    }

    //Demon Mode Specific
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


    /*public override void OnStartServer() => CustomNetworkManager.OnServerReadied += SpawnPlayer;

    public void SpawnPlayer(NetworkConnection conn)
    {
        if ()
    }*/
}