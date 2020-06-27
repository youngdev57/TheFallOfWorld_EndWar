using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;


public class Titan : Monster
{
    private int Page = 1;

    private float LadeTimer = 0f;
    private bool PattenUse = false;

    // 보스 Try 시작 함수
    public override void BossAttackTimer()
    {
        if (canAttack)
        {
            LadeTimer += Time.deltaTime;
        }
        else
        {
            LadeTimer = 0f;
            HP = maxHp;
            Page = 1;
        }
    }

    // 보스 패턴
    public override void BossPatten()
    {
        switch (Page)
        {
            case 1:
                PageChange(70);
                if (LadeTimer == 24f)
                {
                    mAnimator.SetTrigger("Attack_sec");
                    target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT * 2);
                    PattenUse = true;
                    delay = 0f;
                    StartCoroutine(NavStop());
                }
                else if(LadeTimer == 42f)
                {
                    mAnimator.SetTrigger("Attack_sec");
                    target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT * 2);
                    PattenUse = true;
                    delay = 0f;
                    StartCoroutine(NavStop());
                }
                else
                {
                    PattenUse = false;
                }
                break;
        }
    }

    private void PageChange(int hp)
    {
        if (HP / maxHp * 100 <= hp)
        {
            Page++;
            LadeTimer = 0f;
        }
    }

    // 피격
    [PunRPC]
    public override void GetDamage(int Damage)
    {
        if ()
        {

        }

        if (VIT < Damage)
        {
            Damage -= VIT;
        }
        else
        {
            Damage = 0;
        }
        HP -= Damage;
        mNav.stoppingDistance = 6.5f;
        mNav.speed = 5f * speed;
        canAttack = true;
        StopAllCoroutines();
    }

    // 애니메이션
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
                mAnimator.SetTrigger("Run");
                break;
            case Staus.die:
                if (notDie)
                    mAnimator.SetTrigger("Die");
                notDie = false;
                break;
            case Staus.attack:
                mAnimator.SetTrigger("Attack_fir");
                StartCoroutine(NavStop());
                break;
        }
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
                if (dir <= 6.5f)
                {
                    attackMode = true;
                    mNav.isStopped = true;
                    mNav.velocity = Vector3.zero;
                    mNav.speed = 0f;
                    // 평타
                    if (canAttack && !PattenUse)
                    {
                        delay += Time.deltaTime;
                        if (delay >= actSpeed)
                        {
                            monster_Staus = Staus.attack;
                            delay = 0f;
                            target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT);
                        }
                        else
                        {
                            monster_Staus = Staus.idle;
                        }
                    }
                }
                else
                {
                    attackMode = false;
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
        if (!canAttack && other.gameObject.tag == "Player")
        {
            target = other.gameObject.transform;
            mNav.stoppingDistance = 6.5f;
            mNav.speed = 5f * speed;
            canAttack = true;
            StopAllCoroutines();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (!attackMode && canAttack && other.gameObject.tag == "Player")
        {
            mNav.stoppingDistance = 0;
            monster_Staus = Staus.walk;
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
        LadeTimer = 0f;
        maxHp = 200;
        HP = maxHp;
        VIT = 10;
        ACT = 5;
        actSpeed = 5f;

        monster_Staus = Staus.idle;
        type = MonsterType.Boss;

        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();

        canAttack = false;
        attackMode = false;
        idleMode = true;

        notDie = true;
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
