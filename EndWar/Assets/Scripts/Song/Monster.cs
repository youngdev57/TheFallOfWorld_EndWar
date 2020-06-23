﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MobLocation     //몬스터의 소환 위치 (필드인지 던전인지)
{
    Field,
    Dungeon
}

public abstract class Monster : MonoBehaviour
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

    public MobLocation location = MobLocation.Field;    //몬스터 위치 기본값 : 필드

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

    public abstract void GetAbility(Skillability abilityType, float seconde, float index = 0, int dotDamage = 0);

    //슬로우
    public abstract IEnumerator SetStatusEffect(float slowing, float se);

    //스턴
    public abstract IEnumerator SetStatusEffect(float se);

    public abstract void SetNavStopped(bool isTrue);
}
