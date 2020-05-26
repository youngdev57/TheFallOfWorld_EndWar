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
        if (SteamVR_Render.Top())
            viveManager = SteamVR_Render.Top().origin.GetComponent<ViveManager>();

        FindParent();
        myPv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!myPv.IsMine)
            return;

        switch (index)
        {
            case 0:
                transform.position = viveManager.origin.transform.position;
                transform.rotation = viveManager.origin.transform.rotation;
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
        Transform obj = null;
        switch (index)
        {
            case 2:
                obj = viveManager.leftHand.transform;
                break;
            case 3:
                obj = viveManager.rightHand.transform;
                break;
        }

        if (obj != null && !obj.Find(gameObject.name))
            transform.parent = obj;
    }
}
