﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class Insect : Monster
{
    [PunRPC]
    public override void GetDamage(int Damage)
    {
        if (VIT < Damage)
        {
            Damage -= VIT;
        }
        else
        {
            Damage = 1;
        }
        HP -= Damage;
        mNav.stoppingDistance = 2f;
        mNav.speed = 5f * speed;
        canAttack = true;
        StopAllCoroutines();
    }

    [PunRPC]
    public override void PlayAnimation()
    {
        switch (monster_Staus)
        {
            case Staus.idle:
                mAnimator.SetTrigger("Idle");
                break;
            case Staus.walk:
                mAnimator.SetTrigger("Walk");
                break;
            case Staus.run:
                break;
            case Staus.die:
                if (!notDie)
                {
                    mAnimator.SetTrigger("Die");
                    notDie = true;
                }
                break;
            case Staus.attack:
                pv.RPC("AttackType", RpcTarget.All);
                switch (attackType)
                {
                    case 0:
                        mAnimator.SetTrigger("Attack_fir");
                        break;
                    case 1:
                        mAnimator.SetTrigger("Attack_sec");
                        break;
                }
                attackType = -1;
                StartCoroutine(NavStop());
                break;
            case Staus.hit:
                break;
        }
    }

    [PunRPC]
    public override void AttackType()
    {
        attackType = Random.Range(0, 2);
    }

    // 판단
    [PunRPC]
    public override void TargetPosition()
    {
        if (target == null)
        {
            if (idleMode)
            {
                StartCoroutine(SetTarget());
            }
        }
        else
        {
            if (target.gameObject.tag == "Player")
            {
                float dir = Vector3.Distance(transform.position, target.position);
                if (dir <= 2f)
                {
             //       attackMode = true;
                    mNav.isStopped = true;
                    mNav.velocity = Vector3.zero;
                    mNav.speed = 0f;
                    if (canAttack)
                    {
                        delay += Time.deltaTime;
                        if (delay >= actSpeed)
                        {
                            monster_Staus = Staus.attack;
                            delay = 0f;
                            StartCoroutine(DelayGetDamage(second));
                        }
                        else
                        {
                            monster_Staus = Staus.idle;
                        }
                    }
                }
                else
                {
                    //attackMode = false;
                    mNav.isStopped = false;
                    mNav.speed = 5f * speed;
                    monster_Staus = Staus.run;
                }
            }
            else
            {
                if (transform.position.x == target.position.x && transform.position.z == target.position.z)
                {
                    target = null;
                    idleMode = true;
                    monster_Staus = Staus.idle;
                }
                else
                {
                    mNav.speed = 3f * speed;
                    monster_Staus = Staus.walk;
                }
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!canAttack && other.gameObject.tag == "Player" && !notDie)
        {
            target = other.gameObject.transform;
            mNav.stoppingDistance = 2f;
            mNav.speed = 5f * speed;
            canAttack = true;
            StopAllCoroutines();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if ( canAttack && other.gameObject.tag == "Player" && !notDie)
          //  if (!attackMode && canAttack && other.gameObject.tag == "Player" && !notDie)
        {
            mNav.stoppingDistance = 0;
            if (!STUN)
            {
                monster_Staus = Staus.walk;
            }
            target = noneTarget;
            mNav.speed = 3f * speed;
            canAttack = false;
            idleMode = true;
        }
    }

    public override IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 5f * speed;
        monster_Staus = Staus.idle;
    }

    public void OnEnable()
    {
        maxHp = 315;
        HP = maxHp;
        VIT = 15;
        ACT = 31;
        actSpeed = 2.5f;

        monster_Staus = Staus.idle;

        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        canAttack = false;
     //   attackMode = false;
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
}
