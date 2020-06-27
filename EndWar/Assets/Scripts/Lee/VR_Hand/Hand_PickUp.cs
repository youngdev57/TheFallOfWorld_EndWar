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

    [Space(10)]
    public LayerMask layer;
    public GameObject skillCanvasUi;
    public GameObject player;
    public bool isCanvas;

    bool canPickup;
    PickedItem item;

    void Update()
    {
        if (!player.GetComponent<PhotonView>().IsMine && PlayerManager.isDie == true)
            return;

        if (action.GetStateDown(hand) && canPickup)
        {
            if(item != null)
            {
                player.GetComponent<PlayerItem>().GetItme((int)item.gemType);
                item.GetComponent<PickedItem>().PickOff();      //일정 시간 동안 주울 수 없게 함

                item.SetPick(false);
                item = null;
                canPickup = false;
            }
        }

        if (isCanvas == true && closeUi.GetStateDown(hand))
        {
            if(!skillCanvasUi.activeSelf)
                skillCanvasUi.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            item = other.GetComponent<PickedItem>();
            item.SetPick(true);
            canPickup = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            item = other.GetComponent<PickedItem>();
            item.SetPick(false);
            item = null;
            canPickup = false;
        }
    }
}
