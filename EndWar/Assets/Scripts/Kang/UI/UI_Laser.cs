using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Photon.Pun;

public class UI_Laser : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean action;

    public GameObject laserPrefab;
    public GameObject effectPrefab;
    public GameObject effect;
    public GameObject laser;
    private Transform laserTr;
    private Vector3 hitPoint;

    bool isPull = false;
    bool onItemInfo = false;

    float rayDist = 20;

    public PlayerInven pInven;
    public K_PlayerManager kPm;

    UI_ItemInfo slotItemInfo;
    UI_CraftInfo craftInfo;

    void Start()
    {
        //  레이저 생성
        if (!transform.parent.GetComponent<PhotonView>().IsMine)
            return;
        laser = Instantiate(laserPrefab);
        laserTr = laser.transform;
        laser.transform.SetParent(this.transform);
        laser.transform.localPosition = Vector3.zero;
        laser.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void LaserOn()
    {
        if(laser != null)
            laser.SetActive(true);
    }

    public void LaserOff()
    {
        if (laser != null)
            laser.SetActive(false);
    }
    
    void Update()
    {
        if (!transform.parent.GetComponent<PhotonView>().IsMine)
            return;

        RaycastHit stayHit;

        if (Physics.Raycast(controllerPose.transform.position, transform.forward, out stayHit, rayDist, 1 << LayerMask.NameToLayer("UI")))
        {
            if (!onItemInfo && stayHit.transform.GetComponent<UI_ItemInfo>() != null)
            {
                onItemInfo = true;
                slotItemInfo = stayHit.transform.GetComponent<UI_ItemInfo>();
                slotItemInfo.ShowItemInfo();
            }

            if (!onItemInfo && stayHit.transform.GetComponent<UI_CraftInfo>() != null)
            {
                onItemInfo = true;
                craftInfo = stayHit.transform.GetComponent<UI_CraftInfo>();
                craftInfo.ShowItemInfo();
            }
        }
        else
        {
            if (onItemInfo)
            {
                onItemInfo = false;
                if (slotItemInfo != null)
                    slotItemInfo.HideItemInfo();

                if(craftInfo != null)
                    craftInfo.HideItemInfo();
            }
        }

        if (action.GetState(handType))
        {
            isPull = true;

            RaycastHit hit;

            if(Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, rayDist, 1 << LayerMask.NameToLayer("UI")))
            {
                LaserColorChange(hit);
            }
        }

        if(action.GetStateUp(handType))
        {
            isPull = false;

            RaycastHit hit;

            if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit, 20, 1 << LayerMask.NameToLayer("UI")))
            {
                ConfirmAction(hit);
            }

            laser.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        }
    }

    // 트리거 액션과 함께 레이저 색깔 변경 (트리거 당겼는지 유저가 확실히 보이게)
    private void LaserColorChange(RaycastHit hit)
    {
        laser.GetComponentInChildren<MeshRenderer>().material.color = Color.blue; //레이저 파란색으로
    }

    private void ConfirmAction(RaycastHit hit)
    {
        hit.collider.GetComponent<Button>().onClick.Invoke();

        ShowEffect().transform.position = hit.point;
        effect.GetComponent<ParticleSystem>().Play();

        laser.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    private GameObject ShowEffect()
    {
        if(effect == null)
        {
            effect = Instantiate(effectPrefab);
        }

        return effect;
    }
}
