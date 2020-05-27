using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class LookAtHandForword : MonoBehaviourPun
{
    [Space(5)]
    public Transform pivot;

    float speed = 0f;
    PhotonView myPv;

    void Start()
    {
        myPv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!myPv.IsMine)
            return;
        transform.rotation = Quaternion.Euler(new Vector3(0, pivot.eulerAngles.y, 0));
    }
}
