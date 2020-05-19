using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;

public class ControllerPosition : MonoBehaviourPun
{
    public int index = 1;
    public PhotonView myPv;

    ViveManager viveManager;
    void Awake()
    {
        FindParent();
        viveManager = SteamVR_Render.Top().origin.GetComponent<ViveManager>();
        myPv = transform.parent.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!myPv.IsMine)
            return;

        switch (index)
        {
            case 0:
                transform.position = viveManager.origin.transform.position;
                transform.rotation = viveManager.head.transform.rotation;
                break;
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

    public void FindParent()
    {
        ControllerMovement[] obj = FindObjectsOfType<ControllerMovement>();
        for (int i = 0; i < obj.Length; i++)
        {
            if (!obj[i].transform.Find(gameObject.name))
                transform.parent = obj[i].transform;
        }
    }
}
