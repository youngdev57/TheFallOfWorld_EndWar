using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerSpawn : MonoBehaviourPun
{
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;

    void Awake()
    {
        if (!photonView.IsMine)
            return;

        ViveManager viveManager = GetComponent<ViveManager>();
        PhotonNetwork.Instantiate(head.name, viveManager.head.transform.position, viveManager.head.transform.rotation, 0);
        PhotonNetwork.Instantiate(leftHand.name, viveManager.leftHand.transform.position, viveManager.leftHand.transform.rotation, 0);
        PhotonNetwork.Instantiate(rightHand.name, viveManager.rightHand.transform.position, viveManager.rightHand.transform.rotation, 0);
    }
}
