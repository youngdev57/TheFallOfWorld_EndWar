using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Arachnid : Monster
{
    // 피격
    [PunRPC]
    public override void GetDamage(int Damage)
    {
        HP -= Damage;
        mNav.stoppingDistance = 2.5f;
        mNav.speed = 3f * speed;
        canAttack = true;
        StopAllCoroutines();
    }

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
                int type = Random.Range(0, 2);
                switch (type)
                {
                    case 0:
                        mAnimator.SetTrigger("Attack_fir");
                        break;
                    case 1:
                        mAnimator.SetTrigger("Attack_sec");
                        break;
                }
                StartCoroutine(NavStop());
                break;
        }
    }
    // 판단
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
                if (dir <= 2.5f)
                {
                    attackMode = true;
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
                    mNav.speed = 3f * speed;
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
                    mNav.speed = 2f * speed;
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
            mNav.stoppingDistance = 2.5f;
            mNav.speed = 3f * speed;
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
            mNav.speed = 2f * speed;
            canAttack = false;
            idleMode = true;
        }
    }

    public override IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 3f * speed;
        monster_Staus = Staus.idle;
    }

    public void OnEnable()
    {
        maxHp = 200;
        HP = maxHp;
        VIT = 10;
        ACT = 5;
        actSpeed = 1.5f;

        monster_Staus = Staus.idle;

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
