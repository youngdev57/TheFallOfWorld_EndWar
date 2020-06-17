using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public PlayerInven pInven;      //플레이어 인벤토리 정보 스크립트

    public int cristal = 0;
    public int iron = 0;
    public int mineral = 0;
    public int core = 0;
    public int soulGem = 0;
    public int redStone = 0;

    public void GetItme(int gem)
    {
        Gem item = (Gem)gem;
        switch (item)
        {
            case Gem.Crystal:
                cristal++;
                break;
            case Gem.Iron:
                iron++;
                break;
            case Gem.Mineral:
                mineral++;
                break;
            case Gem.Core:
                core++;
                break;
            case Gem.SoulGem:
                soulGem++;
                break;
            case Gem.RedStone:
                redStone++;
                break;
        }

        ModifyGemsLocal();
        pInven.SaveInven(false);    //플레이어 인벤을 통해 재료 변동 정보를 저장.. 재료 개수만 변동이 생겼기 때문에 베이스(기지)의 인벤 정보를 불러오거나 하지 않음
    }

    public void ModifyGemsLocal()   //플레이어 인벤의 잼 개수를 PlayerItem의 값으로 변경시켜줌
    {
        pInven.gems[0] = cristal;
        pInven.gems[1] = iron;
        pInven.gems[2] = mineral;
        pInven.gems[3] = core;
        pInven.gems[4] = soulGem;
        pInven.gems[5] = redStone;
    }

    public void LoadGemsLocal()     //로컬에 있는 플레이어 인벤에서 재료 개수 가져와 적용 (이미 플레이어 인벤에서 웹에 접근하여 로드했으므로 그걸 받아와 씀)
    {
        cristal = pInven.gems[0];
        iron = pInven.gems[1];
        mineral = pInven.gems[2];
        core = pInven.gems[3];
        soulGem = pInven.gems[4];
        redStone = pInven.gems[5];
    }

}
