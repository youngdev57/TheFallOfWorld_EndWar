using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Text;
using TMPro;

public class PlayerInven : MonoBehaviour
{
    public static Inventory baseInven;
    public static Craft baseCraft;
    public static MailPost basePost;

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

    public K_PlayerManager KPM;

    public int mainDamage;
    public int subDamage;
    public int playerDEF;

    public int mainPistol;
    public int subPistol;

    void Start()
    {
        allItemLists = new List<Item>();
        InitAllItemLists();
    }

    public void OnBaseCamp()
    {
        StartCoroutine(WaitBaseInventory());
    }

    public void AddGold(int gold, bool inBase)
    {
        this.gold += gold;
        SaveInven(inBase);   //돈 추가하고 저장~
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
    public void SaveInven(bool loadItems = true)
    {
        StartCoroutine(IE_Save(loadItems));
    }

    public void LoadInven()
    {
        StartCoroutine(IE_Load());
    }

    public string MakeGemsString(bool loadBase = true)
    {
        if(loadBase)
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

    public string slotString = "";

    public string MakeSlotsString(bool loadBase = true)
    {
        if(loadBase)
        {
            BringAllItem();

            StringBuilder str = new StringBuilder();
            
            for (int i = 0; i < 28; i++)
            {

                if (i >= items.Length)
                {
                    str.Append("-1");
                }
                else
                {
                    str.Append((int)items[i].itemId);
                }

                if (i != 27)
                {
                    str.Append(",");
                }
            }

            slotString = str.ToString();
            Debug.Log("메이크 슬롯 스트링 : " + slotString);

            return str.ToString();
        }
        else
        {
            return slotString;
        }
    }

    /** 웹 연동 코루틴 **/
    IEnumerator IE_Save(bool loadItems)
    {
        WWWForm form = new WWWForm();

        form.AddField("gid", PhotonNetwork.NickName);
        form.AddField("gold", gold);
        form.AddField("ingre", MakeGemsString(loadItems));
        form.AddField("main_weapon", mainIdx);
        form.AddField("sub_weapon", subIdx);
        form.AddField("slot", MakeSlotsString(loadItems));
        form.AddField("helmet", helmetIdx);
        form.AddField("armor", armorIdx);
        form.AddField("shoulder", shoulderIdx);
        form.AddField("glove", gloveIdx);
        form.AddField("pants", pantsIdx);
        form.AddField("shoes", shoesIdx);
        form.AddField("acc", accIdx);

        Debug.Log("저장할 내용 - 골드 : " + gold + ", 재료 : " + MakeGemsString(loadItems) + ", 메인 : " + mainIdx + ", 서브 : " + subIdx +
            ", 슬롯 상태 : " + MakeSlotsString(loadItems) + ", H-A-Sd-G-P-S-Ac : " + helmetIdx + ", " + armorIdx + ", " + shoulderIdx + ", " + gloveIdx
             + ", " + pantsIdx + ", " + shoesIdx + ", " + accIdx);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/saveInventory.do", form);

        yield return www;
    }

    List<int> blankPoints;

    IEnumerator IE_Load()
    {
        WWWForm form = new WWWForm();
        
        form.AddField("gid", PhotonNetwork.NickName);

        WWW www = new WWW("http://ec2-15-165-174-206.ap-northeast-2.compute.amazonaws.com:8080/_EndWar/loadInventory.do", form);

        yield return www;

        Debug.Log("Load : " + www.text);

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
        }

        if(bytes[3] == "-1")
        {
            subIdx = -1;
        } else
        {
            subIdx = int.Parse(bytes[3]);
        }

        string[] bytesSlot = bytes[4].Split(',');

        int itemLastIdx = -1;   //아이템 몇개 가지고 있는지 배열 크기 설정용
        int tradeBlankCnt = 0;  //웹 거래 아이템 등록 때문에 비어있는 슬롯 (-2로 표시) 카운트용

        List<int> compSlot = new List<int>();

        blankPoints = new List<int>();

        for(int i=0; i<bytesSlot.Length; i++)
        {
            if(bytesSlot[i] == "-1")
            {
                itemLastIdx = i - 1;
                break;
            }
            else if(bytesSlot[i] == "-2")
            {
                tradeBlankCnt++;    //-2라면 거래로 인한 슬롯 부재.. 거래슬롯부재 카운터 증가
                blankPoints.Add(i);    //블랭크 포인트 (거래로인한 부재슬롯 위치)  각 장착 슬롯보다 앞선것만 따져서 장착인덱스 감산해야하기때문
            } else
            {
                compSlot.Add(int.Parse(bytesSlot[i])); 
            }
        }

        int[] intSlot = new int[1]; //초기화
        if (itemLastIdx == -1)   //아예 아이템이 없는 상태임
        {
            //저장된 아이템이 아무것도 없음
            Debug.Log("불러올 아이템이 아무것도 없음");
        } else
        {
            intSlot = new int[compSlot.Count];   //거래로 인한 슬롯 부재만큼 줄여야함..
            items = new Item[intSlot.Length];

            for (int i = 0; i < compSlot.Count; i++)
            {
                int tempIdx = compSlot[i];
                intSlot[i] = tempIdx;
                items[i] = allItemLists[tempIdx - 1];
            }

            baseInven.BringAllItems(intSlot, gold);

            Debug.Log("슬롯 크기 : " + intSlot.Length + ", 슬롯 내용 : " + intSlot);
        }

        

        helmetIdx = int.Parse(bytes[5]);
        armorIdx = int.Parse(bytes[6]);
        shoulderIdx = int.Parse(bytes[7]);
        gloveIdx = int.Parse(bytes[8]);
        pantsIdx = int.Parse(bytes[9]);
        shoesIdx = int.Parse(bytes[10]);
        accIdx = int.Parse(bytes[11]);

        


        int[] idxs = new int[9];
        idxs[0] = BlankFinder(subIdx);
        idxs[1] = BlankFinder(mainIdx);
        idxs[2] = BlankFinder(helmetIdx);
        idxs[3] = BlankFinder(armorIdx);
        idxs[4] = BlankFinder(shoulderIdx);
        idxs[5] = BlankFinder(gloveIdx);
        idxs[6] = BlankFinder(pantsIdx);
        idxs[7] = BlankFinder(shoesIdx);
        idxs[8] = BlankFinder(accIdx);

        baseInven.Init(idxs);
        baseCraft.Init();

        MakeSlotsString(true);
    }

    int BlankFinder(int inp)        //거래로 인한 슬롯 부재를 찾아 그만큼 해당 장착 인덱스를 내림
    {
        int afterIdx = inp;

        foreach(int blankPoint in blankPoints)
        {
            if (afterIdx > blankPoint)
                afterIdx--;
        }

        return afterIdx;
    }

    void InitAllItemLists()
    {
        //피스톨 (무기)
        allItemLists.Add(new Item("기본 피스톨", ItemType.Weapon, Equipment.LightPistol, 10, 0));
        allItemLists.Add(new Item("크리스탈 피스톨", ItemType.Weapon, Equipment.CrystalPistol, 15, 0));
        allItemLists.Add(new Item("아이언 피스톨", ItemType.Weapon, Equipment.IronPistol, 30, 0));
        allItemLists.Add(new Item("미네랄 피스톨", ItemType.Weapon, Equipment.MineralPistol, 45, 0));
        allItemLists.Add(new Item("코어 피스톨", ItemType.Weapon, Equipment.CorePistol, 75, 0));
        allItemLists.Add(new Item("소울젬 피스톨", ItemType.Weapon, Equipment.SoulGemPistol, 110, 0));
        allItemLists.Add(new Item("레드스톤 피스톨", ItemType.Weapon, Equipment.RedStonePistol, 160, 0));
        //헬멧 (투구)
        allItemLists.Add(new Item("크리스탈 헬멧", ItemType.Helmet, Equipment.CrystalHelmet, 0, 10));
        allItemLists.Add(new Item("아이언 헬멧", ItemType.Helmet, Equipment.IronHelmet, 0, 20));
        allItemLists.Add(new Item("미네랄 헬멧", ItemType.Helmet, Equipment.MineralHelmet, 0, 30));
        allItemLists.Add(new Item("코어 헬멧", ItemType.Helmet, Equipment.CoreHelmet, 0, 50));
        allItemLists.Add(new Item("소울젬 헬멧", ItemType.Helmet, Equipment.SoulGemHelmet, 0, 70));
        allItemLists.Add(new Item("레드스톤 헬멧", ItemType.Helmet, Equipment.RedStoneHelmet, 0, 100));
        //아머 (갑옷)
        allItemLists.Add(new Item("크리스탈 아머", ItemType.Armor, Equipment.CrystalArmor, 0, 20));
        allItemLists.Add(new Item("아이언 아머", ItemType.Armor, Equipment.IronArmor, 0, 30));
        allItemLists.Add(new Item("미네랄 아머", ItemType.Armor, Equipment.MineralArmor, 0, 40));
        allItemLists.Add(new Item("코어 아머", ItemType.Armor, Equipment.CoreArmor, 0, 60));
        allItemLists.Add(new Item("소울젬 아머", ItemType.Armor, Equipment.SoulGemArmor, 0, 80));
        allItemLists.Add(new Item("레드스톤 아머", ItemType.Armor, Equipment.RedStoneArmor, 0, 110));
        //숄더 (어깨 방어구)
        allItemLists.Add(new Item("크리스탈 숄더", ItemType.Shoulder, Equipment.CrystalShoulder, 0, 10));
        allItemLists.Add(new Item("아이언 숄더", ItemType.Shoulder, Equipment.IronShoulder, 0, 20));
        allItemLists.Add(new Item("미네랄 숄더", ItemType.Shoulder, Equipment.MineralShoulder, 0, 30));
        allItemLists.Add(new Item("코어 숄더", ItemType.Shoulder, Equipment.CoreShoulder, 0, 50));
        allItemLists.Add(new Item("소울젬 숄더", ItemType.Shoulder, Equipment.SoulGemShoulder, 0, 70));
        allItemLists.Add(new Item("레드스톤 숄더", ItemType.Shoulder, Equipment.RedStoneShoulder, 0, 100));
        //글러브 (장갑)
        allItemLists.Add(new Item("크리스탈 글러브", ItemType.Glove, Equipment.CrystalGlove, 0, 10));
        allItemLists.Add(new Item("아이언 글러브", ItemType.Glove, Equipment.IronGlove, 0, 20));
        allItemLists.Add(new Item("미네랄 글러브", ItemType.Glove, Equipment.MineralGlove, 0, 30));
        allItemLists.Add(new Item("코어 글러브", ItemType.Glove, Equipment.CoreGlove, 0, 50));
        allItemLists.Add(new Item("소울젬 글러브", ItemType.Glove, Equipment.SoulGemGlove, 0, 70));
        allItemLists.Add(new Item("레드스톤 글러브", ItemType.Glove, Equipment.RedStoneGlove, 0, 100));
        //팬츠 (바지)
        allItemLists.Add(new Item("크리스탈 팬츠", ItemType.Pants, Equipment.CrystalPants, 0, 10));
        allItemLists.Add(new Item("아이언 팬츠", ItemType.Pants, Equipment.IronPants, 0, 20));
        allItemLists.Add(new Item("미네랄 팬츠", ItemType.Pants, Equipment.MineralPants, 0, 30));
        allItemLists.Add(new Item("코어 팬츠", ItemType.Pants, Equipment.CorePants, 0, 50));
        allItemLists.Add(new Item("소울젬 팬츠", ItemType.Pants, Equipment.SoulGemPants, 0, 70));
        allItemLists.Add(new Item("레드스톤 팬츠", ItemType.Pants, Equipment.RedStonePants, 0, 100));
        //슈즈 (신발)
        allItemLists.Add(new Item("크리스탈 슈즈", ItemType.Shoes, Equipment.CrystalShoes, 0, 10));
        allItemLists.Add(new Item("아이언 슈즈", ItemType.Shoes, Equipment.IronShoes, 0, 20));
        allItemLists.Add(new Item("미네랄 슈즈", ItemType.Shoes, Equipment.MineralShoes, 0, 30));
        allItemLists.Add(new Item("코어 슈즈", ItemType.Shoes, Equipment.CoreShoes, 0, 50));
        allItemLists.Add(new Item("소울젬 슈즈", ItemType.Shoes, Equipment.SoulGemShoes, 0, 70));
        allItemLists.Add(new Item("레드스톤 슈즈", ItemType.Shoes, Equipment.RedStoneShoes, 0, 100));
        //악세서리 (장신구)
        allItemLists.Add(new Item("크리스탈 오브", ItemType.Accessory, Equipment.CrystalAcc, 10, 10));
        allItemLists.Add(new Item("아이언 오브", ItemType.Accessory, Equipment.IronAcc, 20, 20));
        allItemLists.Add(new Item("미네랄 오브", ItemType.Accessory, Equipment.MineralAcc, 30, 30));
        allItemLists.Add(new Item("코어 오브", ItemType.Accessory, Equipment.CoreAcc, 50, 50));
        allItemLists.Add(new Item("소울젬 오브", ItemType.Accessory, Equipment.SoulGemAcc, 70, 70));
        allItemLists.Add(new Item("레드스톤 오브", ItemType.Accessory, Equipment.RedStoneAcc, 100, 100));
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
            for(int i=0; i<28; i++)
            {
                baseInven.slotItemInfos[i].pInven = this;
            }
            baseInven.craftInfo.pInven = this;
            
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

            StartCoroutine(WaitBasePost());
        }
    }


    IEnumerator WaitBasePost()
    {
        if (MailPost.GetInstance() == null)
        {
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(WaitBasePost());
        }
        else
        {
            basePost = MailPost.GetInstance();
            basePost.inven = baseInven;
            basePost.LoadPostList(PhotonNetwork.NickName);
        }
    }
}
