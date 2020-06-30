using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum MobLocation     //몬스터의 소환 위치 (필드인지 던전인지)
{
    Field,
    Dungeon
}

public enum MonsterType
{
    Boss, Nomal
}

public  class Monster : MonoBehaviour
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
    public MonsterType type = MonsterType.Nomal;        //몬스터 타입 기본값 : 일반 

    public Staus monster_Staus;

    internal Animator mAnimator;
    internal Rigidbody mRigidbody;
    internal NavMeshAgent mNav;
    public Transform noneTarget;
    public Collider coll;
    public Transform target;
    internal PhotonView pv;

    public bool canAttack;
    internal bool idleMode;
    internal bool attackMode;
    internal bool notDie;

    public float delay;
    public float attackType;

    internal float speed = 1; //본인의 기본 속도를 저장     // 추가한 사람 : 이상재

    public virtual void GetDamage(int Damage)
    {

    }

    [PunRPC]
    public virtual void AttackType()
    {
        attackType = Random.Range(0, 3);
    }

    public virtual void BossAttackTimer()
    {

    }
    public virtual void BossPatten()
    {

    }

    void Update()
    {
        PlayAnimation();
        OnMove();
        TargetPosition();
        Die();
        if (type == MonsterType.Boss)
        {
            BossAttackTimer();
            BossPatten();
        }
    }

    // 애니메이션
    [PunRPC]
    public virtual void PlayAnimation()
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
                if (pv.IsMine)
                {
                    pv.RPC("AttackType", RpcTarget.All);
                }
                switch (attackType)
                {
                    case 0:
                        mAnimator.SetTrigger("Attack_fir");
                        break;
                    case 1:
                        mAnimator.SetTrigger("Attack_sec");
                        break;
                    case 2:
                        mAnimator.SetTrigger("Attack_thi");
                        break;
                }
                attackType = -1;
                StartCoroutine(NavStop());
                break;
        }
    }

    public virtual void Die()
    {
        if (HP <= 0)
        {
            monster_Staus = Staus.die;
            mRigidbody.velocity = Vector3.zero;
            mNav.updatePosition = false;
            mNav.updateRotation = false;
            mNav.isStopped = true;
            coll.isTrigger = true;
            mRigidbody.useGravity = false;
            StartCoroutine(ActiveFalse());
        }
    }

    // 이동
    public virtual void OnMove()
    {
        if (target != null)
        {
            mNav.SetDestination(target.position);
        }
    }

    // 판단
    public virtual void TargetPosition()
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
                if (dir <= 2)
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

    // 플레이어 인식
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!canAttack && other.gameObject.tag == "Player")
        {
            target = other.gameObject.transform;
            mNav.stoppingDistance = 2;
            mNav.speed = 5f * speed;
            canAttack = true;
            StopAllCoroutines();
        }
    }
    public virtual void OnTriggerExit(Collider other)
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

    // 피격시 플레이어 인식
    public virtual void PlayerTarget(Transform tr)
    {
        if (target == null || target.gameObject.tag != "Player")
        {
            target = tr;
        }
    }

    //이상재, 추가본----------------------

    //상태이상 효과
    [PunRPC]
    public virtual void GetAbility(Skillability abilityType, float seconde, float index = 0, int dotDamage = 0)
    {
        switch (type)
        {
            case MonsterType.Boss:
                switch (abilityType)
                {
                    case Skillability.DOT:
                        StartCoroutine(SetStatusEffect(dotDamage, seconde));
                        break;
                }
                break;
            case MonsterType.Nomal:
                switch (abilityType)
                {
                    case Skillability.DOT:
                        StartCoroutine(SetStatusEffect(dotDamage, seconde));
                        break;
                    case Skillability.MEZ:

                        break;
                    case Skillability.SLOW:
                        StartCoroutine(SetStatusEffect(index, seconde));
                        break;
                    case Skillability.STUN:
                        StartCoroutine(SetStatusEffect(seconde));
                        break;
                }
                break;
        }
    }

    //도트 데미지
    public virtual IEnumerator SetStatusEffect(int damage, float se)
    {
        Debug.Log("dot");
        float seconds = 0;
        while (seconds >= se)
        {
            HP -= damage;
            yield return new WaitForSeconds(.1f);
            seconds += .1f;
        }
    }

    //슬로우
    public virtual IEnumerator SetStatusEffect(float slowing, float se)
    {
        Debug.Log("slow");
        speed = 1f - slowing;
        float tempSpeed = mNav.speed;
        mNav.speed *= speed;
        yield return new WaitForSeconds(se);
        speed = 1f;
        mNav.speed = tempSpeed;
    }

    //스턴
    public virtual IEnumerator SetStatusEffect(float se)
    {
        Debug.Log("stun");
        monster_Staus = Staus.idle;
        SetNavStopped(false);
        canAttack = false;
        yield return new WaitForSeconds(se);
        SetNavStopped(true);
        canAttack = true;
    }

    public virtual void SetNavStopped(bool isTrue)
    {
        mNav.updatePosition = isTrue;
        mNav.updateRotation = isTrue;
        mNav.isStopped = !isTrue;
    }
    //이상재, 추가본----------------------

    // 몬스터 목표 지점 설정
    public void RespawnerOff()
    {
        if (location == MobLocation.Dungeon)
        {
            transform.parent.GetComponent<MonsterRespawn>().enabled = false;
        }
    }

    public void SetNoneTarget()
    {
        float x = Random.Range(-50f, 50f);
        float z = Random.Range(-50f, 50f);

        noneTarget.position = new Vector3(transform.parent.position.x + x, transform.parent.position.y, transform.parent.position.z + z);
        target = noneTarget;
    }
    public IEnumerator SetTarget()
    {
        idleMode = false;
        yield return new WaitForSeconds(Random.Range(5f, 9f));
        SetNoneTarget();
    }

    public virtual IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 5f * speed;
        monster_Staus = Staus.idle;
    }

    public IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(6f);
        this.gameObject.SetActive(false);
        mNav.enabled = false;
        StopAllCoroutines();
    }
}
