using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Titan : Monster
{
    public List<GameObject> PattenObj;
    // 0 : 불장판

    private int Page = 1;

    private float LadeTimer;
    private bool PattenUse;
    public bool invincibility;
    public bool PatternUsingOnlyOne;

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

    // 보스 Try 시작 함수
    public override void BossAttackTimer()
    {
        if (canAttack)
        {
            LadeTimer += Time.deltaTime;
        }
        else
        {
            PattenUse = false;
            invincibility = false;
            PatternUsingOnlyOne = false;
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
            // 1 페이즈
            case 1:
                PageChange(70);
                switch ((int)LadeTimer)
                {
                    case 26:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Attack_sec");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 10);
                            //target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT + ACT/10);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 42:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(27);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 60:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Attack_thi");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 9);
                            // target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT+ ACT / 9);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 82:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Shout");
                            invincibility = true;

                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                            StartCoroutine(Uninvincibility());
                        }
                        break;
                    case 114:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(27);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                        // 전멸기
                    case 150:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[1].SetActive(true);
                            PattenObj[1].transform.position = transform.position;
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                }
            PattenUse = false;
            break;
                // 2페이즈
            case 2:
                PageChange(30);
                switch ((int)LadeTimer)
                {
                    case 24:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(32);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 42:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Attack_thi");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 5);
                            //  target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT + ACT / 5);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 60:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Attack_sec");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 7);
                            //  target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT + ACT / 7);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 82:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Attack_thi");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 5);
                            //    target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT + ACT / 5);
                            PattenUse = true;
                            invincibility = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                            StartCoroutine(Uninvincibility());
                        }
                        break;
                    case 114:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(32);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                        // 전멸기
                    case 250:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[1].SetActive(true);
                            PattenObj[1].transform.position = transform.position;
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                }
                PattenUse = false;
                break;
                // 3 페이즈
            case 3:
                switch ((int)LadeTimer)
                {
                    case 24:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(43);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 42:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Attack_thi");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 2);
                            //   target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT + ACT / 2);
                            PattenUse = true;
                            invincibility = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                            StartCoroutine(Uninvincibility());
                        }
                        break;
                    case 60:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(43);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 82:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Attack_sec");
                            target.GetComponent<PlayerManager>().GetDamage(ACT + ACT / 4);
                            //    target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT + ACT / 4);
                            PattenUse = true;
                            invincibility = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                            StartCoroutine(Uninvincibility());
                        }
                        break;
                    case 114:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[0].SetActive(true);
                            PattenObj[0].transform.position = transform.position;
                            PattenObj[0].GetComponent<Fog>().setDamage(43);
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                    case 153:
                        if (PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = false;
                            mAnimator.SetTrigger("Attack_fou");
                            target.GetComponent<PlayerManager>().GetDamage(ACT * 2);
                            //    target.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, ACT*2);
                            PattenUse = true;
                            invincibility = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                            StartCoroutine(Uninvincibility());
                        }
                        break;
                    // 전멸기
                    case 200:
                        if (!PatternUsingOnlyOne)
                        {
                            PatternUsingOnlyOne = true;
                            mAnimator.SetTrigger("Shout");
                            PattenObj[1].SetActive(true);
                            PattenObj[1].transform.position = transform.position;
                            PattenUse = true;
                            delay = 0f;
                            StartCoroutine(NavStop());
                        }
                        break;
                }
                PattenUse = false;
                break;
        }
    }

    private void PageChange(int hp)
    {
        if (HP / maxHp * 100 <= hp)
        {
            Page++;
            LadeTimer = 0f;
            PattenUse = false;
            invincibility = false;
            PatternUsingOnlyOne = false;
        }
    }
    // 애니메이션
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
                if (!notDie)
                {
                    notDie = true;
                    mAnimator.SetTrigger("Die");
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    target.GetComponent<PlayerInfo>().ShowClear();
                    /*
                    foreach (GameObject player in players)
                    {
                        player.GetComponentInChildren<PhotonView>().RPC("ShowClear", RpcTarget.All);
                    }
                    */
                }
                break;
            case Staus.attack:
                mAnimator.SetTrigger("Attack_fir");
                StartCoroutine(NavStop());
                break;
        }
    }
    public override IEnumerator NavStop()
    {
        mNav.speed = 0;
        yield return new WaitForSeconds(3f);
        mNav.speed = 5f * speed;
        monster_Staus = Staus.idle;
    }
    public  IEnumerator Uninvincibility()
    {
        yield return new WaitForSeconds(4f);
        invincibility = false;
    }

    public void MonsterSetting()
    {
        Page = 1;
        LadeTimer = 0f;
        maxHp = 300;
        HP = maxHp;
        VIT = 37;
        ACT = 49;
        actSpeed = 5f;

        monster_Staus = Staus.idle;
        type = MonsterType.Boss;

        canAttack = false;
        idleMode = true;
        PattenUse = false;
        invincibility = false;
        PatternUsingOnlyOne = false;

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

        for (int x = 0; x < PattenObj.Count; x++)
        {
            PattenObj[0].SetActive(false);
        }
    }

    public void OnEnable()
    {
        MonsterSetting();
    }
}