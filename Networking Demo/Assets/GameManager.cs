using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    PlayerObjectController LocalPlayerManager;
    void Start()
    {
        PlayerObjectController[] AllPlayerManagers = FindObjectsOfType<PlayerObjectController>();
        foreach(PlayerObjectController g in AllPlayerManagers)
        {
            if (g.isLocalPlayer)
            {
                LocalPlayerManager = g;
            }
        }
        LocalPlayerManager.CmdSpawnPlayerCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
