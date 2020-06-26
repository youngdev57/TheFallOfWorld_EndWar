using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice;
using Photon.Voice.PUN;
using Photon.Pun;

public class VoiceManager : MonoBehaviour
{
    VoiceClient voiceClient;
    

    void Start()
    {
        PhotonVoiceNetwork photonVoiceNetwork = PhotonVoiceNetwork.Instance;
        photonVoiceNetwork.AutoConnectAndJoin = true;
        photonVoiceNetwork.AutoLeaveAndDisconnect = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
