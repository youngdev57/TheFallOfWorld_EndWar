using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;
using System.Text;

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

    public Equipment mainWeapon = Equipment.None;
    public Equipment subWeapon = Equipment.None;
    public Equipment helmet, armor, shoulder, glove, pants, shoes, acc;
    

    public int mainIdx = -1;
    public int subIdx = -1;
    public int helmetIdx = -1, armorIdx = -1, shoulderIdx = -1, gloveIdx = -1, pantsIdx = -1, shoesIdx = -1, accIdx = -1;
    public int removeIdx = -1;

    public enum ChangeTarget
    {
        MainWeapon,
        SubWeapon,
        Helmet,
        Armor,
        Shoulder,
        Glove,
        Pants,
        Shoes,
        Acc
    }

    public ChangeTarget selectedEquip = ChangeTarget.MainWeapon;

    //표적 이미지
    public Image mainTarget, subTarget, helmetTarget, armorTarget, shoulderTarget, gloveTarget, PantsTarget, shoesTarget, accTarget;
    
    public Image mainWeaponImage;
    public Image subWeaponImage;
    public Image helmetImage, armorImage, shoulderImage, gloveImage, pantsImage, shoesImage, accImage;

    public TextMeshProUGUI mainWeaponName, subWeaponName;

    public PlayerInven pInven;

    public GameObject removeDialog;

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
            ChangeEquip(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeEquip(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeEquip(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeEquip(3);
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
            ClearEquip(ChangeTarget.MainWeapon);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ClearEquip(ChangeTarget.SubWeapon);
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
        Item tItem = PlayerInven.allItemLists[itemId - 1];
        if (itemList.Count == 28)    //총 28개 까지 저장 가능하므로 넘으면 저장 불가~
        {
            Debug.Log("인벤토리 꽉 참");
        }
        else
        {
            //링크드 리스트 맨 마지막에 추가
            itemList.AddLast(new Item(tItem.itemName, tItem.itemType, tItem.itemId, tItem.attackPower, tItem.defensePower));

            StringBuilder str = new StringBuilder() ;
            for(int i=0; i<itemList.Count; i++)
            {
                str.Append(GetNth(i).Value.itemId);
            }
            Debug.Log("제작 후 리스트 : " + str);
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

    /** 장비칸 선택 함수 **/

    public void SelectMain()    //메인무기 슬롯 선택
    {
        selectedEquip = ChangeTarget.MainWeapon;
        mainTarget.gameObject.SetActive(true);
        subTarget.gameObject.SetActive(false);

        UI_ToWeaponChange();
    }

    public void SelectSub()     //서브무기 슬롯 선택
    {
        selectedEquip = ChangeTarget.SubWeapon;
        mainTarget.gameObject.SetActive(false);
        subTarget.gameObject.SetActive(true);

        UI_ToWeaponChange();
    }

    void UI_ToWeaponChange()    //아이템 슬롯에 장비 교체 함수 장착
    {
        foreach (GameObject obj in slots)
        {
            Button btn = obj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate () { btn.GetComponent<Button_InvenSlot>().OnButtonChangeWeapon(); });
        }
    }


    /** 장비 교체 함수 **/

    public void ChangeEquip(int idx, int doSave = 0)
    {
        if(idx == -1)
        {
            return;
        }

        if (idx == mainIdx || idx == subIdx && idx != -1)
            return;
        

        if (GetNth(idx) != null)
        {
            switch(selectedEquip)
            {
                case ChangeTarget.MainWeapon:
                    mainWeapon = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    //mainWeaponName.text = GetNth(idx).Value.itemName;
                    mainIdx = idx;
                    pInven.BringMainWeapon();
                    break;

                case ChangeTarget.SubWeapon:
                    subWeapon = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    //subWeaponName.text = GetNth(idx).Value.itemName;
                    subIdx = idx;
                    pInven.BringSubWeapon();
                    break;

                case ChangeTarget.Helmet:
                    helmet = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    helmetIdx = idx;
                    pInven.BringHelmet();
                    break;

                case ChangeTarget.Armor:
                    armor = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    armorIdx = idx;
                    pInven.BringArmor();
                    break;

                case ChangeTarget.Shoulder:
                    shoulder = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    shoulderIdx = idx;
                    pInven.BringShoulder();
                    break;

                case ChangeTarget.Glove:
                    glove = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    gloveIdx = idx;
                    pInven.BringGlove();
                    break;

                case ChangeTarget.Pants:
                    pants = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    pantsIdx = idx;
                    pInven.BringPants();
                    break;

                case ChangeTarget.Shoes:
                    shoes = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    shoesIdx = idx;
                    pInven.BringShoes();
                    break;

                case ChangeTarget.Acc:
                    acc = (Equipment)GetNth(idx).Value.itemId; Debug.Log((Equipment)GetNth(idx).Value.itemId);
                    accIdx = idx;
                    pInven.BringAcc();
                    break;
            }

            RefreshEquip();
        }
        
        if(doSave == 0)
            pInven.SaveInven();
    }

    public void UI_ChangeWeapon(string name)    //슬롯 버튼 이름을 int로 파싱해서 무기 교체
    {
        ChangeEquip(int.Parse(name) - 1);

        
    }

    public void UI_RemoveItem(int idx)  //UI에서 접근할 함수
    {
        removeIdx = idx - 1;        //삭제 하려는 아이템의 슬롯 위치 갱신

        ShowRemoveDialog();     //아이템 삭제 예,아니오 확인창 팝업
    }

    public void ConfirmRemove()         //아이템 삭제 수락
    {
        RemoveItem(removeIdx);

        HideRemoveDialog();
    }

    public void ShowRemoveDialog()      //아이템 삭제 확인창 보여줌
    {
        removeDialog.SetActive(true);
    }

    public void HideRemoveDialog()      //아이템 삭제 확인창 숨김
    {
        removeDialog.SetActive(false);
    }

    //들고있는 장비 해제
    public void ClearEquip(ChangeTarget target)
    {
        switch(target)
        {
            case ChangeTarget.MainWeapon:
                mainWeapon = Equipment.None;
                mainIdx = -1;   //비움   (인벤토리 어디에도 없기 때문에 -1)
                //mainWeaponName.text = "미장착";
                break;
            case ChangeTarget.SubWeapon:
                subWeapon = Equipment.None;
                subIdx = -1;
                //subWeaponName.text = "미장착";
                break;
            case ChangeTarget.Helmet:
                helmet = Equipment.None;
                helmetIdx = -1;
                break;
            case ChangeTarget.Armor:
                armor = Equipment.None;
                armorIdx = -1;
                break;
            case ChangeTarget.Shoulder:
                shoulder = Equipment.None;
                shoulderIdx = -1;
                break;
            case ChangeTarget.Glove:
                glove = Equipment.None;
                gloveIdx = -1;
                break;
            case ChangeTarget.Pants:
                pants = Equipment.None;
                pantsIdx = -1;
                break;
            case ChangeTarget.Shoes:
                shoes = Equipment.None;
                shoesIdx = -1;
                break;
            case ChangeTarget.Acc:
                acc = Equipment.None;
                accIdx = -1;
                break;
        }

        RefreshEquip();

        pInven.BringAllEquip();
        pInven.SaveInven();
    }

    //장비 장착 상태 갱신
    public void RefreshEquip()
    {
        if(mainWeapon == Equipment.None)
            mainWeaponImage.sprite = null;
        else
            mainWeaponImage.sprite = spriteList[(int)mainWeapon - 1];

        if (subWeapon == Equipment.None)
            subWeaponImage.sprite = null;
        else
            subWeaponImage.sprite = spriteList[(int)subWeapon - 1];

        if (helmet == Equipment.None)
            helmetImage.sprite = null;
        else
            helmetImage.sprite = spriteList[(int)helmet - 1];

        if (armor == Equipment.None)
            armorImage.sprite = null;
        else
            armorImage.sprite = spriteList[(int)armor - 1];

        if (shoulder == Equipment.None)
            shoulderImage.sprite = null;
        else
            shoulderImage.sprite = spriteList[(int)shoulder - 1];

        if (glove == Equipment.None)
            gloveImage.sprite = null;
        else
            gloveImage.sprite = spriteList[(int)glove - 1];

        if (pants == Equipment.None)
            pantsImage.sprite = null;
        else
            pantsImage.sprite = spriteList[(int)pants - 1];

        if (shoes == Equipment.None)
            shoesImage.sprite = null;
        else
            shoesImage.sprite = spriteList[(int)shoes - 1];

        if (acc == Equipment.None)
            accImage.sprite = null;
        else
            accImage.sprite = spriteList[(int)acc - 1];
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
