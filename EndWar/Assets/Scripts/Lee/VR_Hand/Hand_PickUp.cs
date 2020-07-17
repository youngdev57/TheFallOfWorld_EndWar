using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;

public class Hand_PickUp : MonoBehaviour
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
    bool isPickup;
    PickedItem item;

    void Update()
    {
        if (PlayerManager.isDie == true)
            return;

        if (action.GetStateDown(hand) && canPickup)
        {
            if(item != null)
            {
                item.SetPick(false);
                item = null;
                isPickup = true;
                canPickup = false;
            }
        }

        if(isPickup == true)
            item.SetPick(false);

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

        if (isPickup == true && other.gameObject.layer == LayerMask.NameToLayer("ItemBox"))
        {
            player.GetComponent<PlayerItem>().GetItme((int)item.gemType);
            item.GetComponent<PickedItem>().PickOff();      //일정 시간 동안 주울 수 없게 함
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            item = other.GetComponent<PickedItem>();
            item = null;
            isPickup = false;
            canPickup = false;
        }
    }
}
