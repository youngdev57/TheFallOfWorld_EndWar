using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CraftInfo : MonoBehaviour
{
    public PlayerInven pInven;

    Inventory inven;

    public void ShowItemInfo()
    {
        inven = PlayerInven.baseInven;

        inven.itemInfo_UI.SetActive(true);     //일단 UI 켬
        
        Craft craft = PlayerInven.baseCraft;

        int idx = craft.viewIndex;

        Item item = PlayerInven.baseInven.GetNth(idx).Value;

        inven.itemInfo_Name.text = item.itemName;
        inven.itemInfo_power.text = "전투력 : " + item.GetPower();
        inven.itemInfo_atk.text = "공격력 + " + item.GetAttackPower();
        inven.itemInfo_def.text = "방어력 + " + item.GetDefensePower();

        string ingreName = "";
        int ingreIdx = craft.craftList[(int)item.itemId - 1].requireOre[0];

        switch (ingreIdx)
        {
            case 0:
                ingreName = "크리스탈";
                break;
            case 1:
                ingreName = "아이언";
                break;
            case 2:
                ingreName = "미네랄";
                break;
            case 3:
                ingreName = "코어";
                break;
            case 4:
                ingreName = "소울젬";
                break;
            case 5:
                ingreName = "레드스톤";
                break;
        }

        inven.itemInfo_IngreTxt.text = "분해 시 얻을 수 있는 재료 :\n" + ingreName + " " + craft.craftList[(int)item.itemId - 1].requireCnt[0] + "개";
        inven.itemInfo_Ingre.sprite = craft.oreSprs[ingreIdx];
    }

    public void HideItemInfo()
    {
        inven.itemInfo_UI.SetActive(false);
    }
}
