using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum Staus
    {
        idle, walk, run, die, attack
    }

    public int maxHp;               // 최대체력
    public int HP;                      // 현재체력
    public int VIT;                      // 방어력
    public int ACT;                     // 공격력
    public float actSpeed;         // 공격속도

    public Transform target;
    
    public virtual void GetDamage(int Damage)
    {

    }
    // 피격시 플레이어 인식
    public virtual void PlayerTarget(Transform tr)
    {
        if (target == null || target.gameObject.tag != "Player")
        {
            target = tr;
        }
    }

    public virtual void GetAbility(int abilityType, float seconde, int damage, float index = 0) { }
}
