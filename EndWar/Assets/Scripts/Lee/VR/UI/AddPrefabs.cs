using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//스킬 아이콘에 들어갈 스크립트
public class AddPrefabs : MonoBehaviour
{
    public Skill skill;

    void Start()
    {
        if(skill == null)
            Debug.LogError("Do not Setting Skill ", this);
    }

    public Skill GetSkill()
    {
        return skill;
    }
}
