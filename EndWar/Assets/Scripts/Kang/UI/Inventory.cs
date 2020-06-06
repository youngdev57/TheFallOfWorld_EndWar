using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;
    public static Inventory GetInstance()
    {
        return instance;
    }

    public LinkedList<Item> itemList;
    public List<Sprite> spriteList;

    public List<GameObject> slots;
    public GameObject itemPrefab;

    public InputField del_Input;

    bool isFirst = true;

    public int nowWeaponIdx = 0;

    public Weapon mainWeapon = Weapon.None;
    public Weapon subWeapon = Weapon.None;

    public int mainIdx = -1;
    public int subIdx = -1;

    public enum ChangeTarget
    {
        MainWeapon,
        SubWeapon
    }

    public ChangeTarget selectedWeapon = ChangeTarget.MainWeapon;

    public Image mainTarget;
    public Image subTarget;
    public Image mainWeaponImage;
    public Image subWeaponImage;

    public TextMeshProUGUI mainWeaponName, subWeaponName;

    public PlayerInven pInven;

    void Start()
    {
        itemList = new LinkedList<Item>();

        foreach(GameObject slot in slots)
        {
            GameObject ins = Instantiate(itemPrefab);
            ins.transform.parent = slot.transform;
            ins.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            ins.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            ins.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            ins.SetActive(false);
        }

        Init();

        instance = this;
    }

    public void Init()
    {
        RefreshUI();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(3);
        }

        if(Input.GetKeyDown(KeyCode.Comma))
        {
            SelectMain();
        }

        if(Input.GetKeyDown(KeyCode.Period))
        {
            SelectSub();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            ClearWeapon(ChangeTarget.MainWeapon);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ClearWeapon(ChangeTarget.SubWeapon);
        }
    }

    void OnEnable()
    {
        if(!isFirst)
            RefreshUI();
    }

    void OnDisable()
    {
        AllOff();
    }

    void RefreshUI()        //UI 초기화 함수
    {
        AllOff();

        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject itemClone = slots[i].transform.GetChild(0).gameObject;
            itemClone.SetActive(true);

            Text nameText = itemClone.transform.GetChild(0).GetComponent<Text>();
            Text typeText = itemClone.transform.GetChild(1).GetComponent<Text>();

            itemClone.transform.GetChild(2).GetComponent<Image>().sprite = spriteList[(int)GetNth(i).Value.itemId - 1];

            nameText.text = GetNth(i).Value.itemName;
            typeText.text = GetNth(i).Value.itemType.ToString();
        }

        isFirst = false;
    }

    void AllOff()   //슬롯의 이미지를 다 끄는 함수 (초기화용)
    {
        if (slots[0].transform.childCount == 0)
            return;

        foreach (GameObject slot in slots)
        {
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void AddItem(int itemId)  //아이템 추가 함수
    {
        Item tItem = PlayerInven.allItemLists[itemId];
        if (itemList.Count == 28)    //총 28개 까지 저장 가능하므로 넘으면 저장 불가~
        {
            Debug.Log("인벤토리 꽉 참");
        }
        else
        {
            //링크드 리스트 맨 마지막에 추가
            itemList.AddLast(new Item(tItem.itemName, tItem.itemType, tItem.itemId, tItem.attackPower, tItem.defensePower));     
        }

        RefreshUI();    //추가 후 UI 초기화~
    }

    public void RemoveItem(int idx) {           //인덱스를 이용해 링크드리스트 해당 자리의 아이템 삭제~~
        if(GetNth(idx) != null)
            itemList.Remove(GetNth(idx));

        RefreshUI();    //삭제 후 UI 초기화~
        pInven.BringAllItem();
        pInven.SaveInven();
    }

    public void SelectMain()
    {
        selectedWeapon = ChangeTarget.MainWeapon;
        mainTarget.gameObject.SetActive(true);
        subTarget.gameObject.SetActive(false);
    }

    public void SelectSub()
    {
        selectedWeapon = ChangeTarget.SubWeapon;
        mainTarget.gameObject.SetActive(false);
        subTarget.gameObject.SetActive(true);
    }

    public void ChangeWeapon(int idx, int doSave = 0)
    {
        if(idx == -1)
        {
            return;
        }

        if (GetNth(idx) != null)
        {
            if (selectedWeapon == ChangeTarget.MainWeapon)
            {
                mainWeapon = (Weapon)GetNth(idx).Value.itemId + 1;
                mainWeaponName.text = GetNth(idx).Value.itemName;
                mainIdx = idx;
            }
            else
            {
                subWeapon = (Weapon)GetNth(idx).Value.itemId + 1;
                subWeaponName.text = GetNth(idx).Value.itemName;
                subIdx = idx;
            }

            RefreshWeapon();
        }

        pInven.BringAllWeapon();
        
        if(doSave == 0)
            pInven.SaveInven();
    }

    public void ClearWeapon(ChangeTarget target)
    {
        switch(target)
        {
            case ChangeTarget.MainWeapon:
                mainWeapon = Weapon.None;
                mainIdx = -1;   //맨주먹   (인벤토리 어디에도 없기 때문에 -1)
                mainWeaponName.text = "미장착";
                break;
            case ChangeTarget.SubWeapon:
                subWeapon = Weapon.None;
                subIdx = -1;    //맨주먹
                subWeaponName.text = "미장착";
                break;
        }

        RefreshWeapon();

        pInven.BringAllWeapon();
        pInven.SaveInven();
    }

    public void RefreshWeapon()
    {
        if(mainWeapon == Weapon.None)
        {
            mainWeaponImage.sprite = null;
        } else
        {
            mainWeaponImage.sprite = spriteList[(int)mainWeapon - 1];
        }

        if (subWeapon == Weapon.None)
        {
            subWeaponImage.sprite = null;
        }
        else
        {
            subWeaponImage.sprite = spriteList[(int)subWeapon - 1];
        }
    }

    /** 플레이어 인벤 연동 **/

    public void BringAllItems(int[] intSlot)
    {
        for(int i=0; i<intSlot.Length; i++)
        {
            AddItem(intSlot[i]);
        }
    }


    /** 스크립트 내부적으로 쓰이는 함수들 **/

    public LinkedListNode<Item> GetNth(int idx) {          //링크드리스트의 idx번째 아이템을 가져오는 함수 (기본적으로 없어서 만듬)
        int count = 0;
        LinkedListNode<Item> root = itemList.First;
        LinkedListNode<Item> target = null;
        
        while (root != null)
        {
            if (count == idx)
            {
                target = root;
            }
            root = root.Next;
            count++;
        }

        return target;
    }
}



public enum Defense
{
    None,
    Crystal,
    Iron,
    Crystalion,
    Mineral,
    Core,
    SoulGem,
    Redstone
}
