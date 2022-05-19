using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject SpeakingIcon;
    public TextMeshProUGUI RoleAssignment;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpeakingIcon.SetActive(GameManager.instance.LocalPlayerManager.gameObject.GetComponent<VoiceChat>().isTalking);
        RoleAssignment.text = "Demon: " + GameManager.instance.GetDemonCharacter().PlayerName;
    }
}
