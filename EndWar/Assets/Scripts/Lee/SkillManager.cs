using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public Skill skill;
    public Projector skillProjector;
    public GameObject pointObj;

    RaycastHit _hit;

    //스킬 준비(범위, 조준을 보여줌)
    public void Charge()
    {
        if(skill.type == SkillType.RADIAL)
        {
            //범위 표시
            skillProjector.enabled = true;
            skillProjector.fieldOfView = skill.viewAngle;
            skillProjector.farClipPlane = skill.distance - 1f;

            skill.target = transform.position;
        }
        else
        {
            skillProjector.enabled = false;
        }

        if (skill.type == SkillType.POINT)
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hit, skill.distance, skill.layerMask)) //레이어를 Ground로 설정
            {
                pointObj.SetActive(true);
                pointObj.transform.position = _hit.point;
                skill.target = _hit.point;
            }
        }
        else
        {
            pointObj.SetActive(false);
        }
    }

    //준비된 스킬을 사용
    public void Shoot()
    {
        Instantiate(skill.gameObject, skill.target, Quaternion.identity);
    }

    void Update()
    {
        Vector3 _distance = transform.forward * skill.distance;
        Debug.DrawRay(transform.position, _distance, Color.red);

        if (Input.GetKey(KeyCode.A))
        {
            Charge();
        }
        if (Input.GetKeyUp(KeyCode.A))
            Shoot();
    }
}
