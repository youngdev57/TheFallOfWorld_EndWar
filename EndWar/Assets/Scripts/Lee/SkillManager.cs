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
        if(skill.type == SkillType.RADIAL)
        {
            //범위 표시
            skillProjector.fieldOfView = skill.viewAngle;
            skillProjector.farClipPlane = skill.distance - 1f;

            skill.target = transform.position;
        }
        if (skill.type == SkillType.POINT)
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hit, skill.distance, skill.layerMask)) //레이어를 Ground로 설정
            {
                pointObj.transform.position = _hit.point;
                skill.target = _hit.point;
            }
        }
    }

    //준비된 스킬을 사용
    [PunRPC]
    public void Shoot()
    {
        Instantiate(skill.gameObject, skill.target, Quaternion.identity);
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetKey(KeyCode.A))
        {
            photonView.RPC("RangeOn",RpcTarget.AllBuffered, null);
            photonView.RPC("ShowRange", RpcTarget.AllBuffered, null);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            photonView.RPC("RangeOff", RpcTarget.AllBuffered, null);
            photonView.RPC("Shoot", RpcTarget.AllBuffered, null);
        }
    }
}
