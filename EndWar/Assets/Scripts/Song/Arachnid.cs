using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arachnid : Monster
{
    private void Start()
    {
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
                AttackType();
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
    
    public override void AttackType()
    {
        attackType = Random.Range(0, 2);
    }

    public override IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 3f * speed;
        monster_Staus = Staus.idle;
    }

    public void MontserSetting()
    {
        maxHp = 80;
        HP = maxHp;
        VIT = 2;
        ACT = 3;
        actSpeed = 2.5f;

        monster_Staus = Staus.idle;

        canAttack = false;
        //attackMode = false;
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
        MontserSetting();
    }
}
