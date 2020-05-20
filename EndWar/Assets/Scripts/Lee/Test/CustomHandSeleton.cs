using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class CustomHandSeleton : SteamVR_Behaviour_Skeleton
{
    PhotonView myPv;
  
    new void Awake()
    {
        myPv = GetComponent<PhotonView>();
        if (myPv.IsMine)
            base.Awake();
    }

    new void OnEnable()
    {
        if (myPv.IsMine)
            base.OnEnable();
    }

    new void OnDisable()
    {
        if (myPv.IsMine)
            base.OnDisable();
    }
}
