using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;

public class VoiceChat : NetworkBehaviour
{
    public bool isTalking = false;
    NetworkIdentity Identity;
    AudioSource AS;
    void Start()
    {
        Identity = GetComponent<NetworkIdentity>();
        AS = GetComponent<AudioSource>();
        //if (Identity.isLocalPlayer) SteamUser.StartVoiceRecording();
    }

    // Update is called once per frame
    void Update()
    {
        if (Identity.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (isTalking)
                {
                    isTalking = false;
                    SteamUser.StopVoiceRecording();
                }
                else
                {
                    isTalking = true;
                    SteamUser.StartVoiceRecording();
                }
            }
            uint Compressed;
            EVoiceResult ret = SteamUser.GetAvailableVoice(out Compressed);
            if (ret == EVoiceResult.k_EVoiceResultOK && Compressed > 1024)
            {
                //Debug.Log(Compressed);
                byte[] ByteBuffer = new byte[1024];
                ret = SteamUser.GetVoice(true, ByteBuffer, 1024, out uint ByteCount);
                if (ret == EVoiceResult.k_EVoiceResultOK && ByteCount > 0)
                {
                    Cmd_SendData(ByteBuffer, ByteCount);
                }
            }
        }
        else
        {
            isTalking = AS.isPlaying;
        }
    }

    [Command(channel = 2)]
    void Cmd_SendData(byte[] data, uint size)
    {
        Debug.Log("Command");
        VoiceChat[] players = FindObjectsOfType<VoiceChat>();

        for (int i = 0; i < players.Length; i++)
        {
            if (this == players[i])
            {
                continue;
            }
            Target_PlaySound(players[i].GetComponent<NetworkIdentity>().connectionToClient, data, size);
        }
    }

    [TargetRpc(channel = 2)]
    void Target_PlaySound(NetworkConnection conn, byte[] destBuffer, uint bytesWritten)
    {
        Debug.Log("Target");
        byte[] destBuffer2 = new byte[22050 * 2];
        uint bytesWritten2;
        EVoiceResult ret = SteamUser.DecompressVoice(destBuffer, bytesWritten, destBuffer2, (uint)destBuffer2.Length, out bytesWritten2, 22050);
        if (ret == EVoiceResult.k_EVoiceResultOK && bytesWritten2 > 0)
        {
            AS.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 22050, 1, 22050, false);
            
            float[] test = new float[22050];
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = (short)(destBuffer2[i * 2] | destBuffer2[i * 2 + 1] << 8) / 32768.0f;
            }
            AS.clip.SetData(test, 0);
            AS.Play();
        }
    }
    /*
    public void PlayVoice(byte[] ByteBuffer, uint ByteCount)
    {
        byte[] destBuffer = new byte[22050 * 2];
        EVoiceResult voiceResult = SteamUser.DecompressVoice(ByteBuffer, ByteCount, destBuffer, (uint)destBuffer.Length, out uint bytesWritten, 22050);
        if (voiceResult == EVoiceResult.k_EVoiceResultOK && bytesWritten > 0)
        {
            AS.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 22050, 1, 22050, false);
            float[] test = new float[22050];
            for (int i = 0; i < test.Length; ++i)
            {
                test[i] = (short)(destBuffer[i * 2] | destBuffer[i * 2 + 1] << 8) / 32768.0f;
            }
            AS.clip.SetData(test, 0);
            AS.Play();
        }
    }*/
}
