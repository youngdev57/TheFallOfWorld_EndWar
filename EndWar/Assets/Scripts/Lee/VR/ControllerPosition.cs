using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControllerPosition : MonoBehaviourPun
{
    public int index = 1;

    void Awake()
    {
        photonView.RPC("GetParent", RpcTarget.AllBuffered, null);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        switch (index)
        {
            case 1:
                transform.position = ViveManager.Instance.head.transform.position;
                transform.rotation = ViveManager.Instance.head.transform.rotation;
                break;
            case 2:
                transform.position = ViveManager.Instance.leftHand.transform.position;
                transform.rotation = ViveManager.Instance.leftHand.transform.rotation;
                break;
            case 3:
                transform.position = ViveManager.Instance.rightHand.transform.position;
                transform.rotation = ViveManager.Instance.rightHand.transform.rotation;
                break;
        }
    }

    [PunRPC]
    public void GetParent()
    {
        PhotonView[] p = GameObject.FindObjectsOfType<PhotonView>();
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].gameObject.name == "Player" && p[i].IsMine)
                transform.parent = p[i].transform;
        }
    }
}
