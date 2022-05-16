using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject SpeakingIcon;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpeakingIcon.SetActive(GameManager.instance.LocalPlayerManager.gameObject.GetComponent<VoiceChat>().isTalking);
    }
}
