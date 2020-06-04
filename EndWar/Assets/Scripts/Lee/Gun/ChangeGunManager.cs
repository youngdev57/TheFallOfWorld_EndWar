using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class ChangeGunManager : MonoBehaviourPun
{
    public SteamVR_Action_Boolean touchPress;
    public SteamVR_Action_Vector2 touchValue;
    public GameObject mainWeapon;
    public GameObject secondaryWeapon;

    int select = 0;

    void Start()
    {
        
    }

   
    void Update()
    {
        if (touchPress.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            select += touchValue.axis.y > 0 ? -1 : 1;
            if (select < 0)
                select = 0;
            else if (select >= 2)
                select = 1;


        }
    }

    void ChangeGun(int index)
    {
        if (index == 0)
        {
            if (!mainWeapon.activeSelf)
            {
                mainWeapon.SetActive(true);
                secondaryWeapon.SetActive(false);
            }
        }
    }
}
