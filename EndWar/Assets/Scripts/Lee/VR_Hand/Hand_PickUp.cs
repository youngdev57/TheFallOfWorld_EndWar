using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;
using System;

public class Hand_PickUp : MonoBehaviourPun
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean closeUi;
    public SteamVR_Action_Boolean action;
    public LayerMask layer;
    public GameObject skillCanvasUi;
    public GameObject player;

    bool canPickup;
    PickedItem item;

    void Update()
    {
        if (action.GetStateDown(hand) && canPickup)
        {
            if(item != null)
            {
                player.GetComponent<PlayerItem>().GetItme((int)item.gemType);
                PhotonNetwork.Destroy(item.gameObject);
            }
        }

        if (closeUi.GetStateDown(hand))
        {
            if(!skillCanvasUi.activeSelf)
                skillCanvasUi.SetActive(true);
            else
                skillCanvasUi.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            item = other.GetComponent<PickedItem>();
            other.GetComponent<PhotonView>().RPC("SetPick", RpcTarget.AllBuffered, true);
            canPickup = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            item = null;
            other.GetComponent<PhotonView>().RPC("SetPick", RpcTarget.AllBuffered, false);
            canPickup = false;
        }
    }
}
