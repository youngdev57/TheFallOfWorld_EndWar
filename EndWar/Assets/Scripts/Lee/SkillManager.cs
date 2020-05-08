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

    RaycastHit _hit;
    public PhotonView myPv;

    void Start()
    {
        myPv = transform.GetComponent<PhotonView>();
        skillProjector = transform.parent.Find("Range").GetComponent<Projector>();
        pointObj = transform.parent.Find("Fx_Point").gameObject;
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
    public void ShowRange()
    {
        if(skill.type == SkillType.RADIAL)
        {
            //범위 표시
            skillProjector.fieldOfView = skill.viewAngle;
            skillProjector.farClipPlane = skill.distance - 1f;

            skill.target = transform.position;
        }
        if (skill.type == SkillType.POINT)
        {
            Transform camRay = this.transform;

            Debug.DrawRay(camRay.position, camRay.forward);

            if (Physics.Raycast(camRay.position, camRay.forward, out _hit, skill.distance, skill.layerMask)) //레이어를 Ground로 설정
            {
                Vector3 point = _hit.point;
                point.y += .2f;
                pointObj.transform.position = point;
                skill.target = point;
            }
        }
        if(skill.type == SkillType.TARGET)
        {
            Transform camRay = this.transform;

            if (Physics.Raycast(camRay.position, camRay.forward, out _hit, skill.distance, skill.layerMask)) //레이어를 Ground로 설정
            {
                skill.targeting = _hit.transform;
                skill.target = skill.targeting.position;
            }
            else
            {
                skill.targeting = null;
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


        if (grabAction.GetLastState(handType))
        {
            myPv.RPC("RangeOn", RpcTarget.AllBuffered, null);
            ShowRange();
        }
        //잡는 버튼을 땔때
        if (grabAction.GetLastStateUp(handType))
        {

            myPv.RPC("RangeOff", RpcTarget.AllBuffered, null);
            Shoot();

        }
        if (Input.GetKey(KeyCode.E))
        {
            myPv.RPC("RangeOn", RpcTarget.AllBuffered, null);
            ShowRange();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            myPv.RPC("RangeOff", RpcTarget.AllBuffered, null);
            Shoot();
        }
    }
}
