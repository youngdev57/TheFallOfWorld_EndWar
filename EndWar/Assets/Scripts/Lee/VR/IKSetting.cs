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
            vive.leftIK.Pole = transform;
        }
        if (hand == WHATHAND.RIGHT)
        {
            if (photonView.IsMine)
                vive = transform.parent.parent.GetComponent<ViveManager>();

            vive.rightGrip.HandSkeleton = GetComponent<CustomHandSeleton>();

            vive.rightIK.Target = wrist;
            vive.rightIK.Pole = transform;
        }
    }  
}
