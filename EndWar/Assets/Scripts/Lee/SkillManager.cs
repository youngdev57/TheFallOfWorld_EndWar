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
    public PlayerManager player;
    public Skill skill;
    public Projector skillProjector;
    public GameObject pointObj;
    public Transform pivot;

    RaycastHit _hit;
    public PhotonView myPv;

    bool isPoint;
    void Start()
    {
        //skillProjector = transform.parent.Find("Range").GetComponent<Projector>();
        //pointObj = transform.parent.Find("Fx_Point").gameObject;
    }

    public void RangeOn()
    {
        switch (skill.type)
        {
            case SkillType.RADIAL:
                skillProjector.enabled = true;
                break;
            case SkillType.POINT:
                if(isPoint)
                    pointObj.SetActive(true);
                else
                    pointObj.SetActive(false);
                break;
            case SkillType.NONTARGET:
                skill.NonTargeting();
                break;
        }
    }

    //오브젝트 끄기
    public void RangeOff()
    {
        skillProjector.enabled = false;
        pointObj.SetActive(false);
        //targetObj Off
    }


    //스킬 준비(범위, 조준을 보여줌)
    public void ShowRange()
    {
        isPoint = true;
        skill.rotation = Quaternion.identity;
        if (skill.type == SkillType.RADIAL)
        {
            //범위 표시
            skillProjector.fieldOfView = skill.viewAngle;
            skillProjector.farClipPlane = skill.distance - 1f;

            skill.target = player.transform.position;
            skill.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
        }
        if (skill.type == SkillType.POINT)
        {
            Debug.DrawRay(pivot.position, pivot.forward);

            if (Physics.Raycast(pivot.position, pivot.forward, out _hit, skill.distance, skill.layerMask)) //레이어를 Ground로 설정
            {
                isPoint = true;
                Vector3 point = _hit.point;
                point.y += .2f;
                pointObj.transform.position = point;
                pointObj.transform.GetChild(0).localScale = Vector3.one * skill.range;
                skill.target = point;
            }
            else
            {
                isPoint = false;
            }
        }
        if(skill.type == SkillType.NONTARGET)
        {
            skill.target = transform.position;
            skill.rotation = transform.rotation;
        }
    }

    //준비된 스킬을 사용
    public void Shoot()
    {
        skill.Use();
    }

    void Update()
    {
        if (!myPv.IsMine || PlayerManager.isDie == true)
            return;
        if ( player.IsUseSkill() )
        {
            if (grabAction.GetState(handType))
            {
                RangeOn();
                ShowRange();
            }
            //잡는 버튼을 땔때
            if (grabAction.GetStateUp(handType) && isPoint)
            {
                RangeOff();
                Shoot();
                player.UseSkill();
            }
        }
    }
}
