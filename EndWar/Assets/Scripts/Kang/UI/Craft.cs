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
    public List<Sprite> weaponSprs;
    public List<Sprite> oreSprs;

    //제작법 리스트
    public List<CraftSet> craftList;

    //제작 아이템 이름


    //UI 상태 파라미터
    int viewIndex = 0;

    public PlayerInven pInven;

    void Start()
    {
        //테스트용 초기 값
        //oreCnts[(int)Gem.Crystal] = 60;     //크리스탈
        //oreCnts[(int)Gem.Iron] = 30;        //철
        //oreCnts[(int)Gem.Mineral] = 40;     //미네랄
        //oreCnts[(int)Gem.Core] = 50;        //코어
        //oreCnts[(int)Gem.SoulGem] = 40;     //소울젬
        //oreCnts[(int)Gem.RedStone] = 20;    //레드스톤

        craftList = new List<CraftSet>();
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
        if(Input.GetKeyDown(KeyCode.E))
        {
            LoadNextCraft();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadPrevCraft();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CraftItem();
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
    void RefreshOreTexts()  
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
        Weapon code = nowItem.itemCode;                //현재 제작아이템의 아이템 코드

        craftItem_Image.sprite = weaponSprs[(int)code - 1];  //제작할 아이템의 표시 이미지를 아이템 코드를 인덱스로 하여 이미지배열에서 불러옴
        craftItem_Name.text = nowItem.itemName;     //제작할 아이템의 표시 이름을 nowItem에서 불러옴

        for(int i=0; i<nowItem.requireOre.Length; i++)      //제작할 아이템의 요구 재료 종류만큼 for문 돌림.
        {
            requireImages[i].gameObject.SetActive(true);    // i번째 요구 재료 이미지 표시
            requireTexts[i].gameObject.SetActive(true);     // i번째 요구 재료 개수 텍스트 표시

            requireImages[i].sprite = oreSprs[nowItem.requireOre[i]];           // 요구 재료 이미지를 nowItem의 정보를 토대로 가져옴
            requireTexts[i].text = oreCnts[nowItem.requireOre[i]] + " / " + nowItem.requireCnt[i];  // 요구 재료 개수 텍스트를 표시함 (해당 광물 가진 재료 수 / 요구 재료 수)
        }
    }

    /** 다음 버튼 콜백 함수 **/
    public void LoadNextCraft()     
    {
        if (viewIndex + 1 == craftList.Count)
        {
            viewIndex = 0;      // 마지막 인덱스면 첫번째(0) 제작법을 불러옴
        } else
        {
            viewIndex++;
        }
        

        RefreshCraftInfo();     //제작 아이템 정보 화면에 갱신
    }

    /** 이전 버튼 콜백 함수 **/
    public void LoadPrevCraft()     
    {
        if (viewIndex == 0)
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

        inven.pInven.BringAllItem();
        pInven.BringAllGem();
        inven.pInven.SaveInven();
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

        SetCraftList_1((int)Gem.Crystal, 4);
        SetCraftList_1((int)Gem.Iron, 6);
        SetCraftList_1((int)Gem.Mineral, 8);
        SetCraftList_1((int)Gem.Core, 6);
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

    public void BringAllGems()
    {
        for (int i = 0; i < 6; i++)
        {
            oreCnts[i] = pInven.gems[i];
        }
    }
}
