﻿using System.Collections;
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
            gameObject.SetActive(false);
        }

        if (GetComponent<SteamVR_Behaviour_Skeleton>())
            GetComponent<SteamVR_Behaviour_Skeleton>().enabled = false;
    }
}
