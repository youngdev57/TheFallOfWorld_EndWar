using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum WHATHAND { HEAD, LEFT, RIGHT}
public class IKSetting : MonoBehaviour
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
