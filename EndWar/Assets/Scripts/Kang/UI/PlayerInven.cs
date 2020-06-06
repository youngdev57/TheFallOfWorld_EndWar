using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text;

public class PlayerInven : MonoBehaviour
{
    public static Inventory baseInven;
    public static Craft baseCraft;

    public int gold;

    public int[] gems;      //재료 개수 배열

    public Item mainWeapon;
    public Item subWeapon;

    public int mainIdx;
    public int subIdx;

    public Item[] items;

    public static List<Item> allItemLists;

    void Start()
    {
        allItemLists = new List<Item>();
        InitAllItemLists();

        StartCoroutine(WaitBaseInventory());
    }

    public void BringAllWeapon()
    {
        BringMainWeapon();
        BringSubWeapon();
    }

    public void BringMainWeapon()
    {
        mainWeapon = items[baseInven.mainIdx];
        mainIdx = baseInven.mainIdx;
    }

    public void BringSubWeapon()
    {
        subWeapon = items[baseInven.subIdx];
        subIdx = baseInven.subIdx;
    }


    public void BringAllItem() //기지 인벤의 아이템 리스트의 아이템 들을 전부 불러옴
    {
        int arrayCnt = baseInven.itemList.Count;    //가지고 있던 장비의 개수
        items = new Item[arrayCnt];    //개수 만큼 배열을 재생성

        for(int i=0; i<arrayCnt; i++)
        {
            items[i] = baseInven.GetNth(i).Value;

            Debug.Log(i + "번째 슬롯 : " + items[i].itemName + ", 아이템 코드 : " + items[i].itemId);
        }
    }

    public void BringAllGem()   //제작대의 재료 수를 전부 불러옴
    {
        for(int i=0; i<6; i++)
        {
            gems[i] = baseCraft.oreCnts[i];
        }
    }

    public Item GetItem(int idx)   //idx 번째 아이템을 반환
    {
        Item tempItem = items[idx];
        return tempItem;
    }

    /** 웹 연동 함수 **/
    public void SaveInven()
    {
        StartCoroutine(IE_Save());
    }

    public void LoadInven()
    {
        StartCoroutine(IE_Load());
    }

    public string MakeGemsString()
    {
        BringAllGem();
        StringBuilder str = new StringBuilder();

        for (int i = 0; i < gems.Length; i++)
        {
            str.Append(gems[i]);
            if(i != 5)
                str.Append(",");
        }

        return str.ToString();
    }

    public string MakeSlotsString()
    {
        StringBuilder str = new StringBuilder();

        for (int i = 0; i < items.Length; i++)
        {
            str.Append((int)items[i].itemId);
            if (i != items.Length - 1)
                str.Append(",");
        }

        return str.ToString();
    }

    /** 웹 연동 코루틴 **/
    IEnumerator IE_Save()
    {
        WWWForm form = new WWWForm();

        form.AddField("gid", PhotonNetwork.NickName);
        form.AddField("gold", gold);
        form.AddField("ingre", MakeGemsString());
        form.AddField("main_weapon", mainIdx);
        form.AddField("sub_weapon", subIdx);
        form.AddField("slot", MakeSlotsString());


        Debug.Log("저장할 내용 - 골드 : " + gold + ", 재료 : " + MakeGemsString() + ", 메인 : " + mainIdx + ", 서브 : " + subIdx +
            ", 슬롯 상태 : " + MakeSlotsString());

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/saveInventory.do", form);

        yield return www;
    }

    IEnumerator IE_Load()
    {
        WWWForm form = new WWWForm();
        
        form.AddField("gid", "TESTER");
        PhotonNetwork.NickName = "TESTER";
        Debug.Log(PhotonNetwork.NickName + " 누구야");

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/loadInventory.do", form);

        yield return www;

        Debug.Log("불러온 내용 : " + www.text);

        string[] bytes = www.text.Split('^');
        gold = int.Parse(bytes[0]);

        string[] bytesGem = bytes[1].Split(',');
        for(int i=0; i<bytesGem.Length; i++)
        {
            gems[i] = int.Parse(bytesGem[i]);
        }

        baseCraft.BringAllGems();

        if (bytes[2] == "-1")
        {
            mainIdx = -1;
        } else
        {
            mainIdx = int.Parse(bytes[2]);
            Debug.Log("로드 시 메인 인덱스 : " + mainIdx);
        }

        if(bytes[3] == "-1")
        {
            subIdx = -1;
        } else
        {
            subIdx = int.Parse(bytes[3]);
        }

        string[] bytesSlot = bytes[4].Split(',');
        int itemLastIdx = -1;
        for(int i=0; i<bytesSlot.Length; i++)
        {
            if(bytesSlot[i] == "-1")
            {
                itemLastIdx = i - 1;
                break;
            }
        }

        if(itemLastIdx == -1)
        {
            //저장된 아이템이 아무것도 없음
            Debug.Log("불러올 아이템이 아무것도 없음");
        } else
        {
            int[] intSlot = new int[itemLastIdx + 1];
            items = new Item[intSlot.Length];

            for (int i = 0; i < intSlot.Length; i++)
            {
                int tempIdx = int.Parse(bytesSlot[i]);
                intSlot[i] = int.Parse(bytesSlot[i]);
                items[i] = allItemLists[tempIdx - 1];
            }

            baseInven.BringAllItems(intSlot);

            Debug.Log("슬롯 크기 : " + intSlot.Length + ", 슬롯 내용 : " + intSlot);
        }

        baseInven.Init();
        baseCraft.Init();

        baseInven.selectedWeapon = Inventory.ChangeTarget.SubWeapon;
        baseInven.ChangeWeapon(subIdx, 1);
        baseInven.selectedWeapon = Inventory.ChangeTarget.MainWeapon;
        baseInven.ChangeWeapon(mainIdx, 1);
    }

    void InitAllItemLists()
    {
        allItemLists.Add(new Item("기본 피스톨", ItemType.Weapon, Weapon.LightPistol, 10, 0));
        allItemLists.Add(new Item("크리스탈 피스톨", ItemType.Weapon, Weapon.CrystalPistol, 15, 0));
        allItemLists.Add(new Item("아이언 피스톨", ItemType.Weapon, Weapon.IronPistol, 30, 0));
        allItemLists.Add(new Item("미네랄 피스톨", ItemType.Weapon, Weapon.MineralPistol, 45, 0));
        allItemLists.Add(new Item("코어 피스톨", ItemType.Weapon, Weapon.CorePistol, 75, 0));
    }

    IEnumerator WaitBaseInventory()
    {
        if(Inventory.GetInstance() == null)
        {
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(WaitBaseInventory());
        } else
        {
            baseInven = Inventory.GetInstance();
            baseInven.pInven = this;
            StartCoroutine(WaitBaseCraft());
        }
    }

    IEnumerator WaitBaseCraft()
    {
        if(Craft.GetInstance() == null)
        {
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(WaitBaseCraft());
        } else
        {
            baseCraft = Craft.GetInstance();
            baseCraft.pInven = this;
            LoadInven();
        }
    }
}
