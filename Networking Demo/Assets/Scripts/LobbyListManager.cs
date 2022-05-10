using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LobbyListManager : MonoBehaviour
{
    public static LobbyListManager instance;

    //Lobbies
    public GameObject lobbyListMenu;
    public GameObject lobbyEntryPrefab;
    public GameObject scrollViewContent;

    public GameObject lobbiesButton, hostButton;

    public List<GameObject> listOfLobbies = new List<GameObject>();


    private void Awake()
    {
        if(instance == null) { instance = this; }
    }


    public void GetListOfLobbies()
    {
        lobbiesButton.SetActive(false);
        hostButton.SetActive(false);
        lobbyListMenu.SetActive(true);

        SteamLobby.Instance.GetListOfLobbies();
    }

    public void DisplayLobbies(List<CSteamID> lobbyIds, LobbyDataUpdate_t result)
    {
       for(int i=0; i < lobbyIds.Count; i++)
       {
            if (lobbyIds[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                GameObject createdLobbyItem = Instantiate(lobbyEntryPrefab);

                createdLobbyItem.GetComponent<LobbyEntryData>().lobbySteamID = (CSteamID)lobbyIds[i].m_SteamID;

                createdLobbyItem.GetComponent<LobbyEntryData>().lobbyName =
                    SteamMatchmaking.GetLobbyData((CSteamID)lobbyIds[i].m_SteamID, "name");

                createdLobbyItem.GetComponent<LobbyEntryData>().SetLobbyName();

                createdLobbyItem.transform.SetParent(scrollViewContent.transform);
                createdLobbyItem.transform.localScale = Vector3.one;

                listOfLobbies.Add(createdLobbyItem);
            }
       }
    }


    public void DestroyLobbies()
    {
        foreach(GameObject lobbyItem in listOfLobbies)
        {
            Destroy(lobbyItem);
        }
        listOfLobbies.Clear();
    }

}
