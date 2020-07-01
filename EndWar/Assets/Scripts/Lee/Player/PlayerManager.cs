using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    public Transform MPBOX;     //MP통

    public Slider hP_Slider;
    public Slider backHpBar;

    //public Slider mP_Slider;

    public int p_HP = 100;    //최대 HP
    public int p_MP = 3;      //스킬 사용 횟수
    public int p_DEF = 0;     //방어력

    [Space(10)]
    public GameObject dieEffect;
    public GameObject teleportEffect;
    public GameObject DieText;
    public static bool isDie = false;

    MPBar[] mpArray;

    int currHP;
    int currDEF;

    bool isbackHpHit = false;

    float healTime = 0;
    bool isHeal = false;

    IEnumerator healCoroutine;

    internal PhotonManager photonManager;

    //-------------------------------------Start
    void Start()
    {
        if (!photonView.IsMine)
        {
            hP_Slider.transform.parent.gameObject.SetActive(false);
            return;
        }

        healCoroutine = HealCoroutine();
        mpArray = MPBOX.GetComponentsInChildren<MPBar>();
        for (int i = 0; i < mpArray.Length; i++)
        {
            if (!(i + 1 >= mpArray.Length))
                mpArray[i].nextMpBar = mpArray[i + 1];
            else
                mpArray[i].nextMpBar = null;
        }

        Init();
    }

    void Init()
    {
        currHP = p_HP;
        currDEF = p_DEF;

        hP_Slider.maxValue = currHP;
        hP_Slider.value = currHP;

        backHpBar.maxValue = currHP;
        backHpBar.value = currHP;
    }

    //-------------------------------------Update
    void Update()
    {
        if (!photonView.IsMine || isDie == true)
            return;

        SliderHPBar();
        AutoHeal();

        if (currHP <= 0)
            photonView.RPC("IsDie", RpcTarget.All, true);

        if (Input.GetKeyDown(KeyCode.A))
        {
            currHP -= 10;
            healTime = 5f;
            isbackHpHit = true;
        }
    }

    IEnumerator HealCoroutine()
    {
        while (isHeal)
        {
            currHP += 10;

            if(currHP >= p_HP)
            {
                currHP = p_HP;
                isHeal = false;
                StopCoroutine(healCoroutine);
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    void AutoHeal()
    {
        if (currHP < p_HP)
        {
            if (healTime <= 0)
            {
                isHeal = true;
                StartCoroutine(healCoroutine);
            }
            else
            {
                healTime -= Time.deltaTime;
                isHeal = false;
                StopCoroutine(healCoroutine);
                healCoroutine = HealCoroutine();
            }
        }
    }

    void SliderHPBar()
    {
        hP_Slider.value = Mathf.Lerp(hP_Slider.value, currHP, 0.5f); // 체력 자연스럽게 내려가기
        if (isbackHpHit)//뒤따라오는 체력 게이지
        {
            backHpBar.value = Mathf.Lerp(backHpBar.value, hP_Slider.value, Time.deltaTime * 10f);
            if (hP_Slider.value >= backHpBar.value - 0.01f)
            {
                isbackHpHit = false;
                backHpBar.value = hP_Slider.value;
            }
        }
    }

    public void UseSkill()
    {
        for (int i = mpArray.Length-1; i >= 0; i--)
        {
            MPBar mp = mpArray[i];
            if (mp.isUseSkill == true)
            {
                for (int j = 0; j < mpArray.Length; j++)
                {
                    MPBar temp = mpArray[j];
                    if (temp.useSkillCoroutine != null)
                        StopCoroutine(temp.useSkillCoroutine);
                    temp.OffCoroutine();
                }

                mp.isUseSkill = false;
                if (i != mpArray.Length-1)
                {
                    mp.slider.value = mp.nextMpBar.slider.value;
                    mp.nextMpBar.slider.value = 0f;
                }
                else
                {
                    mp.slider.value = 0f;
                }

                mp.useSkillCoroutine = mp.ChargingCoroutine(5f);
                StartCoroutine(mp.useSkillCoroutine);
                return;
            }
        }
        //mp바 애니메이션
    }

    public bool IsUseSkill()
    {
        for (int i = mpArray.Length - 1; i >= 0; i--)
        {
            MPBar mp = mpArray[i];
            if (mp.isUseSkill == true)
            {
                return true;
            }
        }
        return false;
    }

    [PunRPC]
    void IsDie(bool boolean)
    {
        dieEffect.SetActive(boolean);
        teleportEffect.SetActive(boolean);
        if(photonView.IsMine)
            DieText.SetActive(boolean);

        isDie = boolean;
        currHP = p_HP;

        if(boolean == true)     //죽으면 기지보내기~ (기지에서 쓰지 마시오 ㅡㅡ)
        {
            photonManager.destination = 0;
            photonManager.SendMessage("LeaveRoom");
            isDie = false;
            Debug.Log("죽었나요?");
        }
    }

    [PunRPC]
    public void GetDamage(int damage)
    {
        int defense = p_DEF / 10;
        healTime = 5f;

        if (!(damage - defense == 0)) 
            currHP -= damage - defense;   //데미지 - 방어지수가 0이 아니면 데미지입음

        isbackHpHit = true;
        Debug.Log("아프다~~~~~ 행~복~~해~~~줘~~어~~~~" + this.gameObject.name + ", 원래 데미지: " + damage + ", 방어력 계산 후 데미지: " + (damage - defense));
    }
}
