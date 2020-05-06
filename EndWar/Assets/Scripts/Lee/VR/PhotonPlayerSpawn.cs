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
        Transform _head = PhotonNetwork.Instantiate(head.name,ViveManager.Instance.head.transform.position, ViveManager.Instance.head.transform.rotation, 0).transform;
        Transform _leftHand = PhotonNetwork.Instantiate(leftHand.name, ViveManager.Instance.leftHand.transform.position, ViveManager.Instance.leftHand.transform.rotation, 0).transform;
        Transform _rightHand = PhotonNetwork.Instantiate(rightHand.name, ViveManager.Instance.rightHand.transform.position, ViveManager.Instance.rightHand.transform.rotation, 0).transform;

        _head.parent = this.transform;
        _leftHand.parent = this.transform;
        _rightHand.parent = this.transform;
    }
}
