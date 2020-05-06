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

        if (GetComponent<Camera>())
            GetComponent<Camera>().enabled = false;

        if (GetComponent<SteamVR_Behaviour_Pose>())
            GetComponent<SteamVR_Behaviour_Pose>().enabled = false;

    }
}
