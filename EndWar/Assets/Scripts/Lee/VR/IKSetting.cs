using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum WHATHAND { HEAD, LEFT, RIGHT}
public class IKSetting : MonoBehaviour
{
    public WHATHAND hand;
    public Transform wrist;

    ViveManager vive;

    void Start()
    {
        vive = SteamVR_Render.Top().origin.GetComponent<ViveManager>();

        if (hand == WHATHAND.HEAD)
        {
            vive.myBody.Head = gameObject;
        }
        if (hand == WHATHAND.LEFT)
        {
            vive.leftIK.Target = wrist;
            //vive.leftGrip.HandSkeleton = GetComponent<CustomHandSeleton>();
            vive.leftGrip.HandSkeleton = GetComponent<SteamVR_Behaviour_Skeleton>();
            //GetComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)vive.leftHand.GetComponent<SteamVR_TrackedObject>().index);
        }
        if (hand == WHATHAND.RIGHT)
        {
            vive.rightIK.Target = wrist;
            //vive.rightGrip.HandSkeleton = GetComponent<CustomHandSeleton>();
            vive.rightGrip.HandSkeleton = GetComponent<SteamVR_Behaviour_Skeleton>();
            //GetComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)vive.rightHand.GetComponent<SteamVR_TrackedObject>().index);
        }
    }  
}
