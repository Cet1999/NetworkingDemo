using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.SceneManagement;

public enum Role { Human, Demon, InfectedHuman };

public class PlayerObjectController : NetworkBehaviour
{

    //Player Data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;
    [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
    [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool Ready;

    [SyncVar] public GameObject ActivePlayerCharacter;

    private CustomNetworkManager manager;

    [Header("Demon Mode")]
    [SyncVar] public Role PlayerRole;
    public GameObject HumanCharacterPrefab;
    public GameObject DemonCharacterPrefab;
    [Header("BombTag Mode")]
    [SyncVar] public bool HoldingBomb;
    public GameObject Bomb_PlayerPrefab;

    private CustomNetworkManager Manager
    {
        get
        {
            if(manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    

    private void Update()
    {

    }

    [Command]
    public void CmdSpawnPlayerCharacter()
    {
        GameObject Character = null;
        if (GameManager.instance.GameMode == Mode.Demon)
        {
            if (PlayerRole == Role.Human)
            {
                Character = Instantiate(HumanCharacterPrefab, new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-15, 7)), Quaternion.identity);
            }
            else if (PlayerRole == Role.Demon)
            {
                Character = Instantiate(DemonCharacterPrefab, new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(7, 10)), Quaternion.identity);
            }
            Character.GetComponent<PlayerCharacterController>().PlayerManagerRef = this;
        }
        else if (GameManager.instance.GameMode == Mode.BombTag)
        {
            Character = Instantiate(Bomb_PlayerPrefab, new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-15, 7)), Quaternion.identity);
            Character.GetComponent<PlayerCharacterController>().PlayerManagerRef = this;
        }
        NetworkServer.Spawn(Character, connectionToClient);
        ActivePlayerCharacter = Character;
    }

    private void PlayerReadyUpdate(bool oldValue, bool newValue)
    {
        if (isServer)
        {
            this.Ready = newValue;
        }
        if (isClient)
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }

    [Command]
    private void CmdSetPlayerReady()
    {
        this.PlayerReadyUpdate(this.Ready, !this.Ready);
    }

    public void ChangeReady()
    {
        if (hasAuthority)
        {
            CmdSetPlayerReady();
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetPlayerName(SteamFriends.GetPersonaName().ToString());
        gameObject.name = "LocalGamePlayer";
        LobbyController.Instance.FindLocalPlayer();
        LobbyController.Instance.UpdateLobbyName();
    }

    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
        LobbyController.Instance.UpdateLobbyName();
        LobbyController.Instance.UpdatePlayerList();
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
        LobbyController.Instance.UpdatePlayerList();
    }

    [Command]
    private void CmdSetPlayerName(string PlayeName)
    {
        this.PlayerNameUpdate(this.PlayerName, PlayeName);
    }


    public void PlayerNameUpdate(string OldValue, string NewValue)
    {
        if (isServer) //Host
        {
            this.PlayerName = NewValue;
        }
        if (isClient) //Client
        {
            LobbyController.Instance.UpdatePlayerList();
        }
    }


    //Start Game
    public void CanStartGame(string SceneName)
    {
        if (hasAuthority)
        {
            CmdCanStartGame(SceneName);
        }
    }

    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }
}
