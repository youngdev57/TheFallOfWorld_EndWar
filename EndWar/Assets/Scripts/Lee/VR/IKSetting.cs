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
    public Transform pole;

    ViveManager vive;

    void Start()
    {
        vive = transform.parent.parent.GetComponent<ViveManager>();

        if (hand == WHATHAND.HEAD)
        {
            vive.myBody.Head = gameObject;
        }
        if (hand == WHATHAND.LEFT)
        {
            vive.leftIK.Target = wrist;
            vive.leftIK.Pole = pole;
        }
        if (hand == WHATHAND.RIGHT)
        {
            vive.rightIK.Target = wrist;
            vive.rightIK.Pole = pole;
        }
    }  
}
