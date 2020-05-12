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
    public GameObject muzzleEffect;
    public GameObject bulletEffect;

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

        StartCoroutine(FireEffect());

        if(Physics.Raycast(ray, out hit, 5000f))
        {
            if(hit.collider.attachedRigidbody)
            {
                Debug.Log("Hit : " + hit.collider.gameObject.name);
            }
        }

        
    }

    IEnumerator FireEffect()
    {
        bulletEffect.transform.localPosition = muzzleTr.localPosition;
        bulletEffect.SetActive(true);
        muzzleEffect.SetActive(true);
        bulletEffect.GetComponent<Rigidbody>().AddForce(muzzleTr.forward * 100f);

        yield return new WaitForSeconds(0.15f);

        muzzleEffect.SetActive(false);

        yield return new WaitForSeconds(0.15f);

        bulletEffect.SetActive(false);
        bulletEffect.GetComponent<Rigidbody>().velocity = Vector3.zero;
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
