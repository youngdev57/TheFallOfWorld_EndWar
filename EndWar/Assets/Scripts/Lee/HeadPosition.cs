using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeadPosition : MonoBehaviourPun
{
    public PhotonView myPv;
    public Transform head;

    void Update()
    {
        if (!myPv.IsMine) return;

        transform.position = head.position;
        transform.rotation = head.rotation;
    }
}
