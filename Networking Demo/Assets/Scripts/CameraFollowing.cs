using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public GameObject LocalPlayerCharacter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(LocalPlayerCharacter == null)
        {
            LocalPlayerCharacter = GameManager.instance.LocalPlayerManager.ActivePlayerCharacter;
        }
        else
        {
            //idk
            Vector3 pos = new Vector3();

            //movement
            pos.x = LocalPlayerCharacter.transform.position.x;
            pos.z = LocalPlayerCharacter.transform.position.z + -10f;
            pos.y = LocalPlayerCharacter.transform.position.y + 13f;

            transform.position += (pos - transform.position) / 10;
        }
    }
}
