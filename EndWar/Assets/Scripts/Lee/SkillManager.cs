using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public Skill selectedSkill;

    //스킬 준비(범위, 조준을 보여줌)
    public void Charge()
    {
        selectedSkill.ShowRange();
    }

    //준비된 스킬을 사용
    public void Shoot()
    {
        selectedSkill.Shoot();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Charge();
        }
        if (Input.GetKeyUp(KeyCode.A))
            Shoot();
    }
}
