using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    public Inventory baseInven;

    public Item[] items;

    public Item mainWeapon;
    public Item subWeapon;


    void Start()
    {
        
    }

    public void BringAllWeapon()
    {
        mainWeapon = items[baseInven.mainIdx];
        subWeapon = items[baseInven.subIdx];
    }

    public void BringAllItemIds() //기지 인벤의 아이템 리스트의 아이템 들을 전부 불러옴
    {
        int arrayCnt = baseInven.itemList.Count;    //가지고 있던 장비의 개수
        items = new Item[arrayCnt];    //개수 만큼 배열을 재생성

        for(int i=0; i<arrayCnt; i++)
        {
            items[i] = baseInven.GetNth(i).Value;

            Debug.Log(i + " : " + items[i].itemName);
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

    }
}
