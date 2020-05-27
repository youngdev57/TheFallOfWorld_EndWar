using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public enum WHATHAND { HEAD, LEFT, RIGHT}
public class IKSetting : MonoBehaviourPun
{
    public WHATHAND hand;
    public Transform wrist;

    ViveManager vive;

    void Start()
    {
        vive = transform.parent.GetComponent<ViveManager>();

        if (hand == WHATHAND.HEAD)
        {
            vive = transform.parent.parent.GetComponent<ViveManager>();
            vive.myBody.Head = gameObject;
        }
        if (hand == WHATHAND.LEFT)
        {
            if(photonView.IsMine)
                vive = transform.parent.parent.GetComponent<ViveManager>();

            vive.leftGrip.HandSkeleton = GetComponent<CustomHandSeleton>();
            vive.leftIK.Target = wrist;
        }
        if (hand == WHATHAND.RIGHT)
        {
            if (photonView.IsMine)
                vive = transform.parent.parent.GetComponent<ViveManager>();

            vive.rightIK.Target = wrist;
            vive.rightGrip.HandSkeleton = GetComponent<CustomHandSeleton>();
        }
    }  
}
