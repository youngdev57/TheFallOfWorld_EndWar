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

        PhotonNetwork.Instantiate(head.name, ViveManager.Instance.head.transform.position, ViveManager.Instance.head.transform.rotation, 0);
        PhotonNetwork.Instantiate(leftHand.name, ViveManager.Instance.leftHand.transform.position, ViveManager.Instance.leftHand.transform.rotation, 0);
        PhotonNetwork.Instantiate(rightHand.name, ViveManager.Instance.rightHand.transform.position, ViveManager.Instance.rightHand.transform.rotation, 0);
    }
}
