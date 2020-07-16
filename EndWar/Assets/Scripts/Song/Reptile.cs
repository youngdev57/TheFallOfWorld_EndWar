using System.Collections;
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
    // 피격
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
                   // attackMode = true;
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
               //     attackMode = false;
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
       //     if (!attackMode && canAttack && other.gameObject.tag == "Player" && !notDie)
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
