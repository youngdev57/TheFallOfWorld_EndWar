using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class TrackObjectManager : MonoBehaviourPun
{
    public PhotonView myPv;

    void Awake()
    {
        if (myPv.IsMine)
            return;

        if (GetComponent<Camera>())
        {
            GetComponent<Camera>().enabled = false;
            return;
        }

        if (GetComponent<SteamVR_TrackedObject>())
            GetComponent<SteamVR_TrackedObject>().enabled = false;

        /*if (GetComponent<SteamVR_Behaviour_Pose>())
            GetComponent<SteamVR_Behaviour_Pose>().enabled = false;

        if (GetComponent<SteamVR_Behaviour_Skeleton>())
            GetComponent<SteamVR_Behaviour_Skeleton>().enabled = false;*/

    }
}
