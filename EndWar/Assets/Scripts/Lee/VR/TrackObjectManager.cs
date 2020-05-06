using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class TrackObjectManager : MonoBehaviourPun
{
    SteamVR_TrackedObject myTrack;
    void Start()
    {
        if (transform.parent.GetComponent<PhotonView>().IsMine)
            return;

        myTrack = GetComponent<SteamVR_TrackedObject>();
        myTrack.enabled = false;
    }
}
