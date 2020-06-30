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
    public int mainWeapon;
    public int secondaryWeapon;

    [Space(10)]
    public GameObject[] Guns;   //0 기본총, 1 기본 레이저 총, 2 조금 쌘 레이저총, 3 따발총, 4 빨간 총 5, 매그넘?권총, 6 리볼버

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
                if (!Guns[mainWeapon].activeSelf)
                {
                    Guns[mainWeapon].SetActive(true);
                    Guns[secondaryWeapon].SetActive(false);
                    anim.SetBool(Guns[mainWeapon].name, true);
                    anim.SetBool(Guns[secondaryWeapon].name, false);
                }
                break;
            case 1:
                if (!Guns[secondaryWeapon].activeSelf)
                {
                    Guns[secondaryWeapon].SetActive(true);
                    Guns[mainWeapon].SetActive(false);
                    anim.SetBool(Guns[secondaryWeapon].name, true);
                    anim.SetBool(Guns[mainWeapon].name, false);
                }
                break;
            case 2:
                Guns[mainWeapon].SetActive(false);
                Guns[secondaryWeapon].SetActive(false);
                anim.SetBool(Guns[mainWeapon].name, false);
                anim.SetBool(Guns[secondaryWeapon].name, false);
                break;
        }
    }
}
