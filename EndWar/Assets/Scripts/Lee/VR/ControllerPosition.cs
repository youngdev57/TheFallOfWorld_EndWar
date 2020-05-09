using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControllerPosition : MonoBehaviourPun
{
    public int index = 1;

    ViveManager viveManager;
    void Awake()
    {
        photonView.RPC("GetParent", RpcTarget.AllBuffered, null);
        viveManager = transform.parent.GetComponent<ViveManager>();
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        switch (index)
        {
            case 1:
                transform.position = viveManager.head.transform.position;
                transform.rotation = viveManager.head.transform.rotation;
                break;
            case 2:
                transform.position = viveManager.leftHand.transform.position;
                transform.rotation = viveManager.leftHand.transform.rotation;
                break;
            case 3:
                transform.position = viveManager.rightHand.transform.position;
                transform.rotation = viveManager.rightHand.transform.rotation;
                break;
        }
    }

    [PunRPC]
    public void GetParent()
    {
        PhotonView[] p = FindObjectsOfType<PhotonView>();
        for (int i = 0; i < p.Length; i++)
        {
            if (p[i].gameObject.name == "Player")
                if(!p[i].transform.Find(gameObject.name))
                    transform.parent = p[i].transform;
        }
    }
}
