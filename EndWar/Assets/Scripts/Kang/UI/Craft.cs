using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Enum 모음집
public enum Gem     //재료(보석) 종류
{
    Crystal,    //인덱스 0
    Iron,       //인덱스 1
    Mineral,    //인덱스 2
    Core,       //인덱스 3
    SoulGem,    //인덱스 4
    RedStone    //인덱스 5
}

public class Craft : MonoBehaviour
{
    private static Craft instance;
    public static Craft GetInstance()
    {
        return instance;
    }

    //연결된 인벤토리 UI
    public Inventory inven;

    //제작대 UI 게임오브젝트
    public GameObject craftPanel;

    //제작대 UI 상단에 있는 보유 재료 개수 표시하는 텍스트들
    public Text crystalText;
    public Text ironText;
    public Text mineralText;
    public Text coreText;
    public Text soulgemText;
    public Text redstoneText;

    public Text craftItem_Name;
    public Image craftItem_Image;

    //제작 요구 재료 이미지,텍스트
    public Image[] requireImages;
    public Text[] requireTexts;
 
    

    //가지고 있는 재료 개수 저장
    public int[] oreCnts = {0, 0, 0, 0, 0, 0}; //가지고 있는 재료 개수 배열 (종류 총 6가지 - 크리스탈, 철, 미네랄, 코어, 소울젬, 레드스톤)

    //제작법 이미지 리스트
    public List<Sprite> equipmentSprs;
    public List<Sprite> oreSprs;

    //제작법 리스트
    public List<CraftSet> craftList;

    //UI 상태 파라미터
    internal int viewIndex = 1;

    public PlayerInven pInven;

    void Start()
    {
        craftList = new List<CraftSet>();

        equipmentSprs = new List<Sprite>();
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_02/weapon_s02"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/weapon_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/weapon_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/weapon_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/weapon_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/weapon_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_08/weapon_s08"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/helmet_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/helmet_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/helmet_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/helmet_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/helmet_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_08/helmet_s08"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/armor_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/armor_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/armor_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/armor_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/armor_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_08/armor_s08"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/shoulders_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/shoulders_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/shoulders_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/shoulders_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/shoulders_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_08/shoulders_s08"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/gloves_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/gloves_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/gloves_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/gloves_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/gloves_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_08/gloves_s08"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/pants_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/pants_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/pants_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/pants_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/pants_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_08/pants_s08"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/boots_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/boots_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/boots_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/boots_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/boots_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_04/boots_s04"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_03/ammunition_s03"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_01/ammunition_s01"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_09/ammunition_s09"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_07/ammunition_s07"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_06/ammunition_s06"));
        equipmentSprs.Add(Resources.Load<Sprite>("Image/Equipment/set_04/ammunition_s04"));

        InitAllCraftLists();

        Init();

        instance = this;
    }

    public void Init()
    {
        RefreshOreTexts();
        RefreshCraftInfo();
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            LoadPrevCraft();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadNextCraft();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftItem();
        }

        if (Input.GetKeyDown(KeyCode.F7))    //치트입니다 ㅎㅎ 광물 20개씩 전부 증가
        {
            for (int i = 0; i < 6; i++)
                oreCnts[i] += 20;

            RefreshOreTexts();
            inven.pInven.SaveInven();
        }

    }

    /** 요구 재료 이미지, 텍스트 전부 SetActive(false) 하는 함수 **/
    void HideRequirements()     
    {
        foreach(Image img in requireImages)
        {
            img.gameObject.SetActive(false);
        }

        foreach (Text txt in requireTexts)
        {
            txt.gameObject.SetActive(false);
        }
    }

    /** 제작대 UI 상단 재료 갯수 갱신 함수 **/
    public void RefreshOreTexts()  
    {
        crystalText.text = GetCrystal().ToString();
        ironText.text = GetIron().ToString();
        coreText.text = GetCore().ToString();
        mineralText.text = GetMineral().ToString();
        soulgemText.text = GetSoulGem().ToString();
       redstoneText.text = GetRedStone().ToString();
    }

    /** 현재 제작 아이템의 이미지, 이름, 요구 재료 등을 화면에 갱신하는 함수 **/
    void RefreshCraftInfo()     
    {
        HideRequirements();     //요구 재료 초기화

        CraftSet nowItem = craftList[viewIndex];    //현재 보고 있는 제작법 인덱스의 제작아이템 정보 불러와서 nowItem에 저장
        Equipment code = nowItem.itemCode;                //현재 제작아이템의 아이템 코드

        craftItem_Image.sprite = equipmentSprs[(int)code - 1];  //제작할 아이템의 표시 이미지를 아이템 코드를 인덱스로 하여 이미지배열에서 불러옴
        craftItem_Name.text = nowItem.itemName;     //제작할 아이템의 표시 이름을 nowItem에서 불러옴

        for(int i=0; i<nowItem.requireOre.Length; i++)      //제작할 아이템의 요구 재료 종류만큼 for문 돌림.
        {
            requireImages[i].gameObject.SetActive(true);    // i번째 요구 재료 이미지 표시
            requireTexts[i].gameObject.SetActive(true);     // i번째 요구 재료 개수 텍스트 표시

            requireImages[i].sprite = oreSprs[nowItem.requireOre[i]];           // 요구 재료 이미지를 nowItem의 정보를 토대로 가져옴
            requireTexts[i].text = oreCnts[nowItem.requireOre[i]] + " / " + nowItem.requireCnt[i];  // 요구 재료 개수 텍스트를 표시함 (해당 광물 가진 재료 수 / 요구 재료 수)
        }

        Debug.Log("현재 크래프트 인덱스 : " + viewIndex); 
    }

    /** 다음 버튼 콜백 함수 **/
    public void LoadNextCraft()     
    {
        if (viewIndex + 1 == craftList.Count)
        {
            viewIndex = 1;      // 마지막 인덱스면 첫번째(0) 제작법을 불러옴
        } else
        {
            viewIndex++;
        }
        

        RefreshCraftInfo();     //제작 아이템 정보 화면에 갱신
    }

    /** 이전 버튼 콜백 함수 **/
    public void LoadPrevCraft()     
    {
        if (viewIndex == 1)
        {
            viewIndex = craftList.Count - 1;    // 인덱스가 0이면 맨 마지막 제작법을 불러옴
        } else
        {
            viewIndex--;
        }

        RefreshCraftInfo();     //제작 아이템 정보 화면에 갱신
    }

    /** 아이템 제작 버튼 콜백 함수 **/
    public void CraftItem()        
    {
        int applyCraft = 0;     //재료 개수 조건 충족시 마다 ++하여 마지막에 모든 조건 충족했는지 확인 할 것임

        CraftSet nowItem = craftList[viewIndex];    //제작법 리스트에서 현재 인덱스의 아이템 불러옴
        int reqNum = nowItem.requireOre.Length;     //제작하려는 아이템의 요구 재료 종류 수

        for (int i=0; i< reqNum; i++)   //요구 재료 종류만큼 포문 돌아감
        {
            if(nowItem.requireCnt[i] <= oreCnts[nowItem.requireOre[i]])     //요구 재료 수가 같거나 큼 (충족 시)
            {
                applyCraft++;   //충족 카운트 증가
            }
        }

        if(applyCraft == reqNum)            //원하는 모든 재료가 있는지 검사
        {
            for(int i=0; i<reqNum; i++)     //요구 재료 종류만큼 포문 돌아감
            {
                oreCnts[nowItem.requireOre[i]] -= nowItem.requireCnt[i];    //재료 요구 수만큼 현재 보유 수에서 뺌
            }
        } else
        {
            return;     //요구 재료가 없다면 함수 종료 ★★★★★★★★
        }

        inven.AddItem((int)nowItem.itemCode);   //제작한 아이템을 인벤토리에 추가

        RefreshOreTexts();      //UI 상단 보유 재료개수 텍스트 갱신
        RefreshCraftInfo();     //보고 있는 제작법 정보 갱신

        pInven.BringAllItem();
        pInven.BringAllGem();
        pInven.SaveInven();
    }

    /** 보석 개수 리턴 함수들 **/
    int GetCrystal()    //크리스탈 개수 리턴
    {
        return oreCnts[(int)Gem.Crystal];
    }
    int GetIron()       //철 개수 리턴
    {
        return oreCnts[(int)Gem.Iron];
    }
    int GetMineral()    //미네랄 개수 리턴
    {
        return oreCnts[(int)Gem.Mineral];
    }
    int GetCore()       //코어 개수 리턴
    {
        return oreCnts[(int)Gem.Core];
    }
    int GetSoulGem()    //소울젬 개수 리턴
    {
        return oreCnts[(int)Gem.SoulGem];
    }
    int GetRedStone()   //레드스톤 개수 리턴
    {
        return oreCnts[(int)Gem.RedStone];
    }

    int craftOrder = 0;

    void InitAllCraftLists()
    {
        //제작법 등록 (재료 종류 수, 완성품 아이템 코드, 재료이름1, 재료1 개수, 재료이름2 ... 재료4 개수)

        List<Item> list = PlayerInven.allItemLists;
        //무기
        SetCraftList_1((int)Gem.Crystal, 1);
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //투구
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //갑옷
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //숄더
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //글러브
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //팬츠
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //슈즈
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
        //악세서리
        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
        SetCraftList_1((int)Gem.SoulGem, 5);
        SetCraftList_1((int)Gem.RedStone, 6);
    }

    void SetCraftList_1(int gem1, int gem1cnt) //젬 종류가 하나인 제작서
    {
        int idx = craftOrder;
        List<Item> list = PlayerInven.allItemLists;
        craftList.Add(new CraftSet(list[idx].itemName, 1, list[idx].itemType, list[idx].itemId, list[idx].attackPower, 
            list[idx].defensePower, gem1, gem1cnt));
        craftOrder++;
    }
    void SetCraftList_2(int gem1, int gem1cnt, int gem2, int gem2cnt)  //젬 종류가 둘인 제작서
    {
        int idx = craftOrder;
        List<Item> list = PlayerInven.allItemLists;
        craftList.Add(new CraftSet(list[idx].itemName, 2, list[idx].itemType, list[idx].itemId, list[idx].attackPower,
            list[idx].defensePower, gem1, gem1cnt, gem2, gem2cnt));
        craftOrder++;
    }
    void SetCraftList_3(int gem1, int gem1cnt, int gem2, int gem2cnt, int gem3, int gem3cnt)   //젬 종류가 셋인 제작서
    {
        int idx = craftOrder;
        List<Item> list = PlayerInven.allItemLists;
        craftList.Add(new CraftSet(list[idx].itemName, 3, list[idx].itemType, list[idx].itemId, list[idx].attackPower,
            list[idx].defensePower, gem1, gem1cnt, gem2, gem2cnt, gem3, gem3cnt));
        craftOrder++;
    }

    //젬 종류가 넷인 제작서
    void SetCraftList_4(int gem1, int gem1cnt, int gem2, int gem2cnt, int gem3, int gem3cnt, int gem4, int gem4cnt)
    {
        int idx = craftOrder;
        List<Item> list = PlayerInven.allItemLists;
        craftList.Add(new CraftSet(list[idx].itemName, 4, list[idx].itemType, list[idx].itemId, list[idx].attackPower,
            list[idx].defensePower, gem1, gem1cnt, gem2, gem2cnt, gem3, gem3cnt, gem4, gem4cnt));
        craftOrder++;
    }

    /** 플레이어 인벤 경유 함수 **/

    public void BringAllGems()      //플레이어 인벤의 모든 젬 개수를 가져옴
    {
        for (int i = 0; i < 6; i++)
        {
            oreCnts[i] = pInven.gems[i];
        }
    }
}
