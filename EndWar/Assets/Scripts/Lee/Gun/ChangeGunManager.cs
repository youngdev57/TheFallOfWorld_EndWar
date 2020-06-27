using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class ChangeGunManager : MonoBehaviourPun
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean touchPress;
    public SteamVR_Action_Boolean pickUpPress;
    public SteamVR_Action_Vector2 touchValue;
    public GameObject mainWeapon;
    public GameObject secondaryWeapon;

    int select = 0;
    Animator anim;
    GameObject pickUpObj;
    void Start()
    {
        if (photonView.IsMine)
        {
            anim = GetComponent<Animator>();
            pickUpObj = transform.parent.parent.GetComponentInChildren<Hand_PickUp>(true).gameObject;
            photonView.RPC("ChangeGun", RpcTarget.AllBuffered, 0);
        }
    }

   
    void Update()
    {
        if (photonView.IsMine && touchPress.GetStateDown(hand))
        {
            select += touchValue.axis.y > 0 ? -1 : 1;
            if (select < 0)
                select = 0;
            else if (select >= 2)
                select = 1;

            if(pickUpObj.activeSelf == true)
            {
                pickUpObj.SetActive(false);
            }

            photonView.RPC("ChangeGun", RpcTarget.AllBuffered, select);
        }

        if (pickUpPress.GetStateDown(hand))
        {
            photonView.RPC("ChangeGun", RpcTarget.AllBuffered, 2);
            pickUpObj.SetActive(true);
        }
    }

    [PunRPC]
    void ChangeGun(int index)
    {
        switch (index)
        {
            case 0:
                if (!mainWeapon.activeSelf)
                {
                    mainWeapon.SetActive(true);
                    secondaryWeapon.SetActive(false);
                    anim.SetBool(mainWeapon.name, true);
                    anim.SetBool(secondaryWeapon.name, false);
                }
                break;
            case 1:
                if (!secondaryWeapon.activeSelf)
                {
                    secondaryWeapon.SetActive(true);
                    mainWeapon.SetActive(false);
                    anim.SetBool(secondaryWeapon.name, true);
                    anim.SetBool(mainWeapon.name, false);
                }
                break;
            case 2:
                mainWeapon.SetActive(false);
                secondaryWeapon.SetActive(false);
                anim.SetBool(mainWeapon.name, false);
                anim.SetBool(secondaryWeapon.name, false);
                break;
        }
    }
}
