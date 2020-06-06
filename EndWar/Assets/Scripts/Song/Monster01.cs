using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum Staus
{
    idle, walk, run, die, attack
}

public class Monster01 : Monster
{
    public Staus monster_Staus;

    internal Animator mAnimator;
    private Rigidbody mRigidbody;
    private NavMeshAgent mNav;
    public Transform target;
    public Transform noneTarget;
    
    public bool canAttack;
    private bool idleMode;
    private bool attackMode;
    private bool notDie;

    private float delay;

    void Start()
    {
        monster_Staus = Staus.idle;

        mNav = GetComponent<NavMeshAgent>();
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();

        canAttack =  false;
        attackMode = false;
        idleMode = true;

        notDie = true;
        delay = 0f;

        maxHp = 200;
        HP = maxHp;
        VIT = 10;
        ACT = 5;
        actSpeed = 2.5f;
    }

    void Update()
    {

        PlayAnimation();
        OnMove();
        TargetPosition();
        Die();
    }

    // 애니메이션
    [PunRPC]
    private void PlayAnimation()
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
                if(notDie)
                    mAnimator.SetTrigger("Die");
                notDie = false;
                break;
            case Staus.attack:
                int type = Random.Range(0,3);
                switch (type)
                {
                    case 0:
                        mAnimator.SetTrigger("Hook");
                        break;
                    case 1:
                        mAnimator.SetTrigger("Chop");
                        break;
                    case 2:
                        mAnimator.SetTrigger("Spin");
                        break;
                }
                StartCoroutine(NavStop());
                break;
        }
    }

    private void Die()
    {
        if (HP <= 0)
        {
            monster_Staus = Staus.die;
            mRigidbody.velocity = Vector3.zero;
            mNav.updatePosition = false;
            mNav.updateRotation = false;
            mNav.isStopped = true;
            GetComponent<CapsuleCollider>().isTrigger = true;
            StartCoroutine(ActiveFalse());
        }
    }

    // 이동
    private void OnMove()
    {
        if (target != null)
        {
            mNav.SetDestination(target.position);
        }
    }

    // 판단
    private void TargetPosition()
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
                if (dir <= 10)
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
                    mNav.speed = 5f;
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
                    mNav.speed = 3f;
                    monster_Staus = Staus.walk;
                }
            }
        }
    }

    /*
    // 어그로 수치 계산
    public void Aggro()
    {
        target = GameManager.GM.Players[0].transform;
        for (int x = 0; x > GameManager.GM.Players.Length; x++)
        {
            if (GameManager.GM.Players[x].GetComponent<Player>().aggro > target.gameObject.GetComponent<Player>().aggro)
            {
                target = GameManager.GM.Players[x].transform;
            }
        }
    }
    */

    // 플레이어 인식
    private void OnTriggerEnter(Collider other)
    {
        if (!canAttack && other.gameObject.tag == "Player")
        {
            target = other.gameObject.transform;
            mNav.stoppingDistance = 10;
            mNav.speed = 5f;
            canAttack = true;
            StopAllCoroutines();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!attackMode && canAttack && other.gameObject.tag == "Player")
        {
            mNav.stoppingDistance = 0;
            monster_Staus = Staus.walk;
            target = noneTarget;
            mNav.speed = 3f;
            canAttack = false;
            idleMode = true;
        }
    }

    // 피격
    [PunRPC]
    public void GetDamage(int Damage)
    {
        HP -= Damage;
        //Aggro();
        mNav.stoppingDistance = 10;
        mNav.speed = 5f;
        canAttack = true;
        StopAllCoroutines();
    }

    // 몬스터 목표 지점 설정
    public void SetNoneTarget()
    {
        float x = Random.Range(-50f, 50f);
        float z = Random.Range(-50f, 50f);

        noneTarget.position = new Vector3(transform.parent.position.x + x, transform.parent.position.y, transform.parent.position.z + z);
        target = noneTarget;
    }
    private IEnumerator SetTarget()
    {
        idleMode = false;
        yield return new WaitForSeconds(Random.Range(5f,9f));
        SetNoneTarget();
    }

    private IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 5f;
        monster_Staus = Staus.idle;
    }

    private IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(6f);
        gameObject.transform.parent.gameObject.SetActive(false);
        StopAllCoroutines();
    }

    public void OnEnable()
    {
        HP = maxHp;
        notDie = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        mNav.updatePosition = true;
        mNav.updateRotation = true;
        mNav.isStopped = false;
        GetComponent<CapsuleCollider>().isTrigger = false;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        monster_Staus = Staus.idle;

        canAttack = false;
        attackMode = false;
        idleMode = true;
        
        target = null;
    }
}
