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
    public Equipment itemId;          //아이템의 고유 번호

    public int attackPower;     //아이템의 공격력
    public int defensePower;    //아이템의 방어력

    public int power;           //아이템의 총 전투력

    public Item(string name, ItemType type, Equipment id, int atk, int def) {
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

    public void SetDefensePower(int def)
    {
        this.defensePower = def;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public int GetDefensePower()
    {
        return defensePower;
    }

    public int GetPower()   //아이템 전투력 반환
    {
        power = (attackPower + defensePower) * 2;

        return power;
    }
}

public enum Equipment
{
    None,
    LightPistol,
    CrystalPistol,
    IronPistol,
    MineralPistol,
    CorePistol,
    SoulGemPistol,
    RedStonePistol,     //무기 끝
    CrystalHelmet,
    IronHelmet,
    MineralHelmet,
    CoreHelmet,
    SoulGemHelmet,
    RedStoneHelmet,     //투구 끝
    CrystalArmor,
    IronArmor,
    MineralArmor,
    CoreArmor,
    SoulGemArmor,
    RedStoneArmor,      //갑옷 끝
    CrystalShoulder,
    IronShoulder,
    MineralShoulder,
    CoreShoulder,
    SoulGemShoulder,
    RedStoneShoulder,   //어깨 끝
    CrystalGlove,
    IronGlove,
    MineralGlove,
    CoreGlove,
    SoulGemGlove,
    RedStoneGlove,      //장갑 끝
    CrystalPants,
    IronPants,
    MineralPants,
    CorePants,
    SoulGemPants,
    RedStonePants,      //바지 끝
    CrystalShoes,
    IronShoes,
    MineralShoes,
    CoreShoes,
    SoulGemShoes,
    RedStoneShoes,      //신발 끝
    CrystalAcc,
    IronAcc,
    MineralAcc,
    CoreAcc,
    SoulGemAcc,
    RedStoneAcc         //악세서리 끝
}
