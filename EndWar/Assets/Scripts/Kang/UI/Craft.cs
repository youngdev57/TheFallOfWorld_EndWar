using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    public Inventory inven;

    public GameObject craftPanel;

    public Text emeraldText;
    public Text opalText;
    public Text topazText;
    public Text orichalcumText;
    public Text obsidianText;

    public Text craftItem_Name;
    public Image craftItem_Image;

    //제작 요구 재료 이미지,텍스트
    public Image[] requireImages;
    public Text[] requireTexts;
 
    //Ores

    public int[] oreCnts = {0, 0, 0, 0, 0}; //emerald, opal, topaz, orichalcum, obsidian

    //제작법 이미지 리스트
    public List<Sprite> weaponSprs;
    public List<Sprite> oreSprs;

    //제작법 리스트
    public List<CraftSet> craftList;

    //제작 아이템 이름


    //UI 상태 파라미터
    int viewIndex = 0;

    void Start()
    {
        oreCnts[0] = 60; //emerald
        oreCnts[1] = 30; //opal
        oreCnts[2] = 40; //topaz
        oreCnts[3] = 50; //orichalcum
        oreCnts[4] = 40; //obsidian

        craftList = new List<CraftSet>();
        craftList.Add(new CraftSet("LSJ 피스톨", 1, 0, 0, 3));    //제작법 등록 (재료 종류 수, 완성품 아이템 코드, 재료번호1, 재료1 개수, 재료번호2 ... 재료4 개수)
        craftList.Add(new CraftSet("CMY-0507 피스톨",2, 1, 0, 4, 1, 2));

        RefreshOreTexts();
        RefreshCraftImages();
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

    void ShowCraftUI()
    {
        craftPanel.SetActive(!craftPanel.activeSelf);
    }

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

    void RefreshOreTexts()  //제작대 UI 상단 재료 갯수 갱신
    {
        emeraldText.text = oreCnts[0].ToString();
        opalText.text = oreCnts[1].ToString();
        orichalcumText.text = oreCnts[2].ToString();
        topazText.text = oreCnts[3].ToString();
        obsidianText.text = oreCnts[4].ToString();
    }

    void RefreshCraftImages()
    {
        HideRequirements();

        CraftSet nowItem = craftList[viewIndex];
        int code = nowItem.itemCode;

        craftItem_Image.sprite = weaponSprs[code];
        craftItem_Name.text = nowItem.itemName;

        for(int i=0; i<nowItem.requireOre.Length; i++)
        {
            requireImages[i].gameObject.SetActive(true);
            requireTexts[i].gameObject.SetActive(true);

            requireImages[i].sprite = oreSprs[nowItem.requireOre[i]];
            requireTexts[i].text = oreCnts[i] + " / " + nowItem.requireCnt[i];
        }
    }

    void LoadNextCraft()
    {
        if (viewIndex + 1 == craftList.Count)
        {
            viewIndex = 0;
        } else
        {
            viewIndex++;
        }
        

        RefreshCraftImages();
    }

    void LoadPrevCraft()
    {
        if (viewIndex == 0)
        {
            viewIndex = craftList.Count - 1;
        } else
        {
            viewIndex--;
        }

        RefreshCraftImages();
    }

    void CraftItem()
    {
        int applyCraft = 0;

        CraftSet nowItem = craftList[viewIndex];
        int reqNum = nowItem.requireOre.Length;

        for (int i=0; i< reqNum; i++)
        {
            if(nowItem.requireCnt[i] <= oreCnts[nowItem.requireOre[i]])
            {
                applyCraft++;
            }
        }

        if(applyCraft == reqNum)
        {
            for(int i=0; i<reqNum; i++)
            {
                oreCnts[nowItem.requireOre[i]] -= nowItem.requireCnt[i];
            }
        } else
        {
            return;
        }

        inven.AddItem(nowItem.itemCode, 0, nowItem.itemName);

        RefreshOreTexts();
        RefreshCraftImages();
    }
}
