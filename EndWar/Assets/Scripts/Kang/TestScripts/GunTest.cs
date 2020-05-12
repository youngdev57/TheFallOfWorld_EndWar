using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class GunTest : MonoBehaviourPunCallbacks
{
    public GameObject rightHand;

    private SteamVR_Input_Sources handType;
    private SteamVR_Action_Boolean grapAction;

    public Transform muzzleTr;

    float delay = 0.5f;
    

    void Start()
    {
        handType = SteamVR_Input_Sources.RightHand;
        grapAction = SteamVR_Actions.default_Grap;
    }

    void Fire()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(muzzleTr.position, muzzleTr.forward);

        if(Physics.Raycast(ray, out hit, 5000f))
        {
            if(hit.collider.attachedRigidbody)
            {
                Debug.Log("Hit : " + hit.collider.gameObject.name);
            }
        }
    }
    
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if(grapAction.GetLastState(handType))
        {
            Fire();
        }
    }
}
