using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Helmet,
    Shoulder,
    Glove,
    Armor,
    Pants,
    Shoes,
    Accessory
}

public class Item : MonoBehaviour
{
    public string itemName;     //아이템의 게임 내 표시 이름
    public ItemType itemType;        //아이템의 타입 (0-무기, 1-머리, 2-어깨, 3-장갑, 4-갑옷, 5-바지, 6-신발, 7-악세서리)
    public Weapon itemId;          //아이템의 고유 번호

    public int attackPower;     //아이템의 공격력
    public int defensePower;    //아이템의 방어력

    public int power;           //아이템의 총 전투력

    public Item(string name, ItemType type, Weapon id, int atk, int def) {
        this.itemName = name;
        this.itemType = type;
        this.itemId = id;
        this.attackPower = atk;
        this.defensePower = def;
        this.power = GetPower();
    }

    public void SetAttackPower(int atk)
    {
        this.attackPower = atk;
    }

    public void SetDefencePower(int def)
    {
        this.defensePower = def;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetDefencePower()
    {
        return defensePower;
    }

    public int GetPower()   //아이템 전투력 반환
    {
        power = (attackPower + defensePower) * 2;

        return power;
    }
}

public enum Weapon
{
    None,
    LightPistol,
    CrystalPistol,
    IronPistol,
    MineralPistol,
    CorePistol,
    SoulGemPistol,
    RedStonePistol
}
