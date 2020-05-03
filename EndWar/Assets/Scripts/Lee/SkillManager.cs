using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillManager : MonoBehaviourPun
{

    public Skill skill;
    public Projector skillProjector;
    public GameObject pointObj;

    RaycastHit _hit;
    PhotonView myPv;

    void Start()
    {
        myPv = GetComponent<PhotonView>();
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
            Transform camRay = Camera.main.transform;

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
            Transform camRay = Camera.main.transform;

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
