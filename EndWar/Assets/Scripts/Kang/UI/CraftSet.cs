using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSet : MonoBehaviour
{
    internal string itemName;

    internal int[] requireOre;
    internal int[] requireCnt;

    internal Weapon itemCode;

    internal ItemType itemType;

    internal int attackPower;
    internal int defensePower;

    public CraftSet(string name, int number, ItemType type, Weapon code, int atk, int def, int ore1 = 0, int cnt1 = 0, int ore2 = 0, int cnt2 = 0, int ore3 = 0, int cnt3 = 0, int ore4 = 0, int cnt4 = 0)
    {
        itemName = name;
        itemCode = code;
        itemType = type;
        attackPower = atk;
        defensePower = def;

        int[] ores = {ore1, ore2, ore3, ore4};
        int[] cnts = {cnt1, cnt2, cnt3, cnt4};

        requireOre = new int[number];
        requireCnt = new int[number];

        for(int i=0; i<number; i++)
        {
            requireOre[i] = ores[i];
            requireCnt[i] = cnts[i];
        }
    }
}
