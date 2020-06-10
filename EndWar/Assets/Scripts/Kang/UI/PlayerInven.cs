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
    public Item helmet, armor, shoulder, glove, pants, shoes, acc;

    public int mainIdx;
    public int subIdx;      //메인,서브 무기 인덱스
    public int helmetIdx, armorIdx, shoulderIdx, gloveIdx, pantsIdx, shoesIdx, accIdx;

    public Item[] items;

    public static List<Item> allItemLists;

    void Start()
    {
        allItemLists = new List<Item>();
        InitAllItemLists();
    }

    public void OnBaseCamp()
    {
        StartCoroutine(WaitBaseInventory());
    }

    public void BringAllEquip()
    {
        BringMainWeapon();
        BringSubWeapon();
        BringHelmet();
        BringArmor();
        BringShoulder();
        BringGlove();
        BringPants();
        BringShoes();
        BringAcc();
    }

    public void BringMainWeapon()
    {
        if(baseInven.mainIdx != -1)
            mainWeapon = items[baseInven.mainIdx];
        mainIdx = baseInven.mainIdx;
    }

    public void BringSubWeapon()
    {
        if (baseInven.subIdx != -1)
            subWeapon = items[baseInven.subIdx];
        subIdx = baseInven.subIdx;
    }

    public void BringHelmet()
    {
        if (baseInven.helmetIdx != -1)
            helmet = items[baseInven.helmetIdx];
        helmetIdx = baseInven.helmetIdx;
    }

    public void BringArmor()
    {
        if (baseInven.armorIdx != -1)
            armor = items[baseInven.armorIdx];
        armorIdx = baseInven.armorIdx;
    }

    public void BringShoulder()
    {
        if (baseInven.shoulderIdx != -1)
            shoulder = items[baseInven.shoulderIdx];
        shoulderIdx = baseInven.shoulderIdx;
    }

    public void BringGlove()
    {
        if (baseInven.gloveIdx != -1)
            glove = items[baseInven.gloveIdx];
        gloveIdx = baseInven.gloveIdx;
    }

    public void BringPants()
    {
        if (baseInven.pantsIdx != -1)
            pants = items[baseInven.pantsIdx];
        pantsIdx = baseInven.pantsIdx;
    }

    public void BringShoes()
    {
        if (baseInven.shoesIdx != -1)
            shoes = items[baseInven.shoesIdx];
        shoesIdx = baseInven.shoesIdx;
    }

    public void BringAcc()
    {
        if (baseInven.accIdx != -1)
            acc = items[baseInven.accIdx];
        accIdx = baseInven.accIdx;
    }


    public void BringAllItem() //기지 인벤의 아이템 리스트의 아이템 들을 전부 불러옴
    {
        int arrayCnt = baseInven.itemList.Count;    //가지고 있던 장비의 개수
        items = new Item[arrayCnt];    //개수 만큼 배열을 재생성

        for(int i=0; i<arrayCnt; i++)
        {
            items[i] = baseInven.GetNth(i).Value;

            Debug.Log("BringAll-Item " + i + "번째 슬롯 : " + items[i].itemName + ", 아이템 코드 : " + items[i].itemId);
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
        BringAllItem();
        StringBuilder str = new StringBuilder();

        Debug.Log("items[0] 의 이름: " + items[0].itemId);
        for (int i = 0; i < 28; i++)
        {
            
            if(i >= items.Length)
            {
                str.Append("-1");
                Debug.Log(i + " : 아이템s 크기를 i가 넘어섬");
            } else
            {
                str.Append((int)items[i].itemId);
                Debug.Log(i + " : 정상적인 입장");
            }

            if (i != 27)
            {
                str.Append(",");
            }

            Debug.Log("메이크슬롯스트링 " + str);
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
        form.AddField("helmet", helmetIdx);
        form.AddField("armor", armorIdx);
        form.AddField("shoulder", shoulderIdx);
        form.AddField("glove", gloveIdx);
        form.AddField("pants", pantsIdx);
        form.AddField("shoes", shoesIdx);
        form.AddField("acc", accIdx);

        Debug.Log("저장할 내용 - 골드 : " + gold + ", 재료 : " + MakeGemsString() + ", 메인 : " + mainIdx + ", 서브 : " + subIdx +
            ", 슬롯 상태 : " + MakeSlotsString() + ", H-A-Sd-G-P-S-Ac : " + helmetIdx + ", " + armorIdx + ", " + shoulderIdx + ", " + gloveIdx
             + ", " + pantsIdx + ", " + shoesIdx + ", " + accIdx);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/saveInventory.do", form);

        yield return www;
    }

    IEnumerator IE_Load()
    {
        WWWForm form = new WWWForm();
        
        form.AddField("gid", PhotonNetwork.NickName);
       // PhotonNetwork.NickName = "TESTER";
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

        helmetIdx = int.Parse(bytes[5]);
        armorIdx = int.Parse(bytes[6]);
        shoulderIdx = int.Parse(bytes[7]);
        gloveIdx = int.Parse(bytes[8]);
        pantsIdx = int.Parse(bytes[9]);
        shoesIdx = int.Parse(bytes[10]);
        accIdx = int.Parse(bytes[11]);

        baseInven.Init();
        baseCraft.Init();

        baseInven.selectedEquip = Inventory.ChangeTarget.SubWeapon;
        baseInven.ChangeEquip(subIdx, 1);
        baseInven.selectedEquip = Inventory.ChangeTarget.MainWeapon;
        baseInven.ChangeEquip(mainIdx, 1);
    }

    void InitAllItemLists()
    {
        //피스톨
        allItemLists.Add(new Item("기본 피스톨", ItemType.Weapon, Equipment.LightPistol, 10, 0));
        allItemLists.Add(new Item("크리스탈 피스톨", ItemType.Weapon, Equipment.CrystalPistol, 15, 0));
        allItemLists.Add(new Item("아이언 피스톨", ItemType.Weapon, Equipment.IronPistol, 30, 0));
        allItemLists.Add(new Item("미네랄 피스톨", ItemType.Weapon, Equipment.MineralPistol, 45, 0));
        allItemLists.Add(new Item("코어 피스톨", ItemType.Weapon, Equipment.CorePistol, 75, 0));
        allItemLists.Add(new Item("소울젬 피스톨", ItemType.Weapon, Equipment.SoulGemPistol, 110, 0));
        allItemLists.Add(new Item("레드스톤 피스톨", ItemType.Weapon, Equipment.RedStonePistol, 160, 0));
        //헬멧
        allItemLists.Add(new Item("크리스탈 헬멧", ItemType.Weapon, Equipment.CrystalHelmet, 15, 0));
        allItemLists.Add(new Item("아이언 헬멧", ItemType.Weapon, Equipment.IronHelmet, 30, 0));
        allItemLists.Add(new Item("미네랄 헬멧", ItemType.Weapon, Equipment.MineralHelmet, 45, 0));
        allItemLists.Add(new Item("코어 헬멧", ItemType.Weapon, Equipment.CoreHelmet, 75, 0));
        allItemLists.Add(new Item("소울젬 헬멧", ItemType.Weapon, Equipment.SoulGemHelmet, 110, 0));
        allItemLists.Add(new Item("레드스톤 헬멧", ItemType.Weapon, Equipment.RedStoneHelmet, 160, 0));
        //갑옷
        allItemLists.Add(new Item("크리스탈 갑옷", ItemType.Weapon, Equipment.CrystalArmor, 15, 0));
        allItemLists.Add(new Item("아이언 갑옷", ItemType.Weapon, Equipment.IronArmor, 30, 0));
        allItemLists.Add(new Item("미네랄 갑옷", ItemType.Weapon, Equipment.MineralArmor, 45, 0));
        allItemLists.Add(new Item("코어 갑옷", ItemType.Weapon, Equipment.CoreArmor, 75, 0));
        allItemLists.Add(new Item("소울젬 갑옷", ItemType.Weapon, Equipment.SoulGemHelmet, 110, 0));
        allItemLists.Add(new Item("레드스톤 갑옷", ItemType.Weapon, Equipment.RedStoneHelmet, 160, 0));
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
