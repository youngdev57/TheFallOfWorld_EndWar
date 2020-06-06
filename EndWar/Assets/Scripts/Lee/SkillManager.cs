using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class SkillManager : MonoBehaviourPun
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;   //컨트롤러 정보
    public SteamVR_Action_Boolean grabAction;       //그랩 액션
    [Space(5)]
    public Skill skill;
    public Projector skillProjector;
    public GameObject pointObj;
    public Transform pivot;

    RaycastHit _hit;
    public PhotonView myPv;

    void Start()
    {
        //skillProjector = transform.parent.Find("Range").GetComponent<Projector>();
        //pointObj = transform.parent.Find("Fx_Point").gameObject;
    }

    //범위 표시 오브젝트를 끄고 키는 함수를 만들것
    [PunRPC]
    public void RangeOn()
    {
        switch (skill.type)
        {
            case SkillType.RADIAL:
                skillProjector.enabled = true;
                break;
            case SkillType.POINT:
                pointObj.SetActive(true);
                break;
            case SkillType.TARGET:

                break;
        }
    }

    //오브젝트 끄기
    [PunRPC]
    public void RangeOff()
    {
        skillProjector.enabled = false;
        pointObj.SetActive(false);
        //targetObj Off
    }


    //스킬 준비(범위, 조준을 보여줌)
    [PunRPC]
    public void ShowRange()
    {
        if (skill.type == SkillType.RADIAL)
        {
            //범위 표시
            skillProjector.fieldOfView = skill.viewAngle;
            skillProjector.farClipPlane = skill.distance - 1f;

            skill.target = transform.position;
        }
        if (skill.type == SkillType.POINT)
        {
            Debug.DrawRay(pivot.position, pivot.forward);

            if (Physics.Raycast(pivot.position, pivot.forward, out _hit, skill.distance, skill.layerMask)) //레이어를 Ground로 설정
            {
                Vector3 point = _hit.point;
                point.y += .2f;
                pointObj.transform.position = point;
                pointObj.transform.GetChild(0).localScale = Vector3.one * skill.range;
                skill.target = point;
            }
        }
        if(skill.type == SkillType.TARGET)
        {
            if (true) //레이어를 Ground로 설정
            {
                /*skill.speed = _hit.transform;
                skill.target = skill.speed.position;*/
            }
            else
            {
                //skill.speed = null;
            }
        }
    }

    //준비된 스킬을 사용
    public void Shoot()
    {
        skill.Use();
    }

    void Update()
    {
        if (!myPv.IsMine)
            return;

        if (grabAction.GetState(handType))
        {
            myPv.RPC("RangeOn", RpcTarget.AllBuffered, null);
            myPv.RPC("ShowRange", RpcTarget.AllBuffered, null);
        }
        //잡는 버튼을 땔때
        if (grabAction.GetStateUp(handType))
        {
            myPv.RPC("RangeOff", RpcTarget.AllBuffered, null);
            Shoot();
        }
    }
}
