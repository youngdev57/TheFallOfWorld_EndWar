using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ViveManager : MonoBehaviourPun
{
    public GameObject origin;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    [Space(10)]
    public BodyTracking myBody;

    [Space(10)]
    public IK leftIK;
    public IK rightIK;
}
