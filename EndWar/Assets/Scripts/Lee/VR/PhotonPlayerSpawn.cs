using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class PhotonPlayerSpawn : MonoBehaviourPun
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    void Awake()
    {
        if (!photonView.IsMine)
            return;

        //ViveManager viveManager = SteamVR_Render.Top().origin.GetComponent<ViveManager>();
        PhotonNetwork.Instantiate(head.name, Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(leftHand.name, Vector3.zero, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(rightHand.name, Vector3.zero, Quaternion.identity, 0);
    }
}
