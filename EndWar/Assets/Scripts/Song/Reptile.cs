﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Reptile : Monster
{
    private void Start()
    {
        Debug.Log("Start");
        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();

        monster_Staus = Staus.idle;

        canAttack = false;
        idleMode = true;
        STUN = false;

        notDie = false;
        delay = 0f;
    }

    public override IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 5f * speed;
        monster_Staus = Staus.idle;
    }

    public void MonsterSetting()
    {
        Debug.Log("Setting");
        maxHp = 220;
        HP = maxHp;
        VIT = 11;
        ACT = 23;
        actSpeed = 5f;

        monster_Staus = Staus.idle;

        canAttack = false;
        //  attackMode = false;
        idleMode = true;
        STUN = false;

        notDie = false;
        delay = 0f;

        mNav.enabled = true;
        mNav.updatePosition = true;
        mNav.updateRotation = true;
        mNav.isStopped = false;
        mNav.stoppingDistance = 0f;
        mRigidbody.useGravity = true;

        target = null;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        coll.isTrigger = false;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public void OnEnable()
    {
        Debug.Log("Enable");
        MonsterSetting();
    }
}
