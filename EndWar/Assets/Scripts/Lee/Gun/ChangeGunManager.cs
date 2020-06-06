using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class ChangeGunManager : MonoBehaviourPun
{
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean touchPress;
    public SteamVR_Action_Vector2 touchValue;
    public GameObject mainWeapon;
    public GameObject secondaryWeapon;

    int select = 0;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        photonView.RPC("ChangeGun", RpcTarget.AllBuffered, 0);
    }

   
    void Update()
    {
        if (touchPress.GetStateDown(hand))
        {
            select += touchValue.axis.y > 0 ? -1 : 1;
            if (select < 0)
                select = 0;
            else if (select >= 2)
                select = 1;

            photonView.RPC("ChangeGun", RpcTarget.AllBuffered,select);
        }
    }

    [PunRPC]
    void ChangeGun(int index)
    {
        if (index == 0)
        {
            if (!mainWeapon.activeSelf)
            {
                mainWeapon.SetActive(true);
                secondaryWeapon.SetActive(false);
                anim.SetBool(mainWeapon.name, true);
                anim.SetBool(secondaryWeapon.name, false);
            }
        }
        else if(index == 1)
        {
            if (!secondaryWeapon.activeSelf)
            {
                secondaryWeapon.SetActive(true);
                mainWeapon.SetActive(false);
                anim.SetBool(secondaryWeapon.name, true);
                anim.SetBool(mainWeapon.name, false);
            }
        }
    }
}
