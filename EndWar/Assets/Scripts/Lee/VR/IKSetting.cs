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
        if (hand == WHATHAND.HEAD)
        {
            vive = transform.parent.GetComponent<ViveManager>();
            vive.myBody.Head = gameObject;
        }
        if (hand == WHATHAND.LEFT)
        {
            vive = transform.parent.parent.GetComponent<ViveManager>();
            vive.leftIK.Target = wrist;
            vive.leftGrip.HandSkeleton = GetComponent<CustomHandSeleton>();
        }
        if (hand == WHATHAND.RIGHT)
        {
            vive = transform.parent.parent.GetComponent<ViveManager>();
            vive.rightIK.Target = wrist;
            vive.rightGrip.HandSkeleton = GetComponent<CustomHandSeleton>();
        }
    }  
}
