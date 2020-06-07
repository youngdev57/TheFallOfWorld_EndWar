using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Location    //플레이어가 기지에 있는지 야외에 있는지 판단할 enum
{
    Base,       //기지
    Outdoor     //기지를 제외한 모든 야외 (필드, 던전)
}

public class K_PlayerManager : MonoBehaviour
{
    public PlayerInven inven;   //플레이어가 들고 다닐 인벤토리 정보
    public int power;       //전투력
    public int story;       //스토리 진행 상황

    public Location location;

    public Weapon selectedWeapon;

    public Weapon mainWeapon;
    public Weapon subWeapon;

    public void ChangeWeaponToMain()    //현재 무기를 메인 무기로..
    {
        selectedWeapon = mainWeapon;
    }

    public void ChangeWeaponToSub()     //현재 무기를 서브 무기로..
    {
        selectedWeapon = subWeapon;
    }

    public void GetMainWeapon()     //메인 무기 정보 갱신
    {
        if(inven.mainIdx == -1)
        {
            mainWeapon = Weapon.None;
        }
        else
        {
            mainWeapon = (Weapon)inven.mainIdx;
        }
    }

    public void GetSubWeapon()      //서브 무기 정보 갱신
    {
        if (inven.subIdx == -1)
        {
            subWeapon = Weapon.None;
        }
        else
        {
            subWeapon = (Weapon)inven.subIdx;
        }
    }

    public void GetWeapon()         //현재 장착 무기 정보 반환
    {
        Item item = PlayerInven.allItemLists[(int)selectedWeapon - 1];
    }

    /** 웹 연동 함수 **/

    public void SaveStatus()
    {
        StartCoroutine(IE_Save());
    }

    public void LoadStatus()
    {
        StartCoroutine(IE_Load());
    }

    /** 웹 연결 코루틴 **/

    IEnumerator IE_Save()
    {
        WWWForm form = new WWWForm();

        form.AddField("gid", PhotonNetwork.NickName);
        form.AddField("power", power);
        form.AddField("story", story);

        Debug.Log("저장할 내용 - 파워 : " + power + ", 스토리 : " + story);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/saveStatus.do", form);

        yield return www;
    }

    IEnumerator IE_Load()
    {
        WWWForm form = new WWWForm();

        form.AddField("gid", PhotonNetwork.NickName);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/loadStatus.do", form);

        yield return www;

        Debug.Log("불러온 내용 : " + www.text);

        string[] bytes = www.text.Split(',');
        power = int.Parse(bytes[0]);
        story = int.Parse(bytes[1]);
    }


    /** 테스트용 키입력 이벤트 **/
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SaveStatus();
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            LoadStatus();
        }
    }
}
