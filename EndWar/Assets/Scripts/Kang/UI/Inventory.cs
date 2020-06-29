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

    public UI_ItemInfo[] slotItemInfos;
    public UI_CraftInfo craftInfo;

    [Space(5)]

    internal int mainIdx = -1;
    internal int subIdx = -1;
    internal int helmetIdx = -1, armorIdx = -1, shoulderIdx = -1, gloveIdx = -1, pantsIdx = -1, shoesIdx = -1, accIdx = -1;
    internal int removeIdx = -1;

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

    [Space(5)]

    //표적 이미지
    public Image mainTarget, subTarget, helmetTarget, armorTarget, shoulderTarget, gloveTarget, pantsTarget, shoesTarget, accTarget;
    
    public Image mainWeaponImage;
    public Image subWeaponImage;
    public Image helmetImage, armorImage, shoulderImage, gloveImage, pantsImage, shoesImage, accImage;

    public TextMeshProUGUI mainWeaponName, subWeaponName;

    public PlayerInven pInven;

    public GameObject removeDialog;

    public TextMeshProUGUI powerValueText;

    public GameObject itemInfo_UI;
    public TextMeshProUGUI itemInfo_Name, itemInfo_power, itemInfo_atk, itemInfo_def, itemInfo_IngreTxt;
    public Image itemInfo_Ingre;

    public TextMeshProUGUI dissolution_Txt;
    bool isDissolFadeOut = false;
    float dissol_Alpha;

    public TextMeshProUGUI gold_Txt;

    internal int invenGold = 0;


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

        int[] dummy = new int[1];
        Init(dummy, true);

        instance = this;
    }

    public void Init(int[] idxs, bool first = false)
    {
        RefreshUI();
        if (first)
            return;

        selectedEquip = ChangeTarget.SubWeapon;
        ChangeEquip(idxs[0], 1);
        selectedEquip = ChangeTarget.MainWeapon;
        ChangeEquip(idxs[1], 1);
        selectedEquip = ChangeTarget.Helmet;
        ChangeEquip(idxs[2], 1);
        selectedEquip = ChangeTarget.Armor;
        ChangeEquip(idxs[3], 1);
        selectedEquip = ChangeTarget.Shoulder;
        ChangeEquip(idxs[4], 1);
        selectedEquip = ChangeTarget.Glove;
        ChangeEquip(idxs[5], 1);
        selectedEquip = ChangeTarget.Pants;
        ChangeEquip(idxs[6], 1);
        selectedEquip = ChangeTarget.Shoes;
        ChangeEquip(idxs[7], 1);
        selectedEquip = ChangeTarget.Acc;
        ChangeEquip(idxs[8], 1);
    }

    private void Update()
    {
        if(isDissolFadeOut)
        {
            dissolution_Txt.alpha -= 0.01f;
            if(dissolution_Txt.alpha <= 0)
            {
                isDissolFadeOut = false;
                dissolution_Txt.gameObject.SetActive(false);
            }
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
        }

        RefreshUI();    //추가 후 UI 초기화~
    }

    public void RemoveItem(int idx) {           //인덱스를 이용해 링크드리스트 해당 자리의 아이템 삭제~~
        CheckEquipsAndClear(idx);   //우선 지울 아이템을 장비하고 있는지 확인 후 해당 장비 슬롯 비움

        if (GetNth(idx) != null)
        {
            Item item = GetNth(idx).Value;
            int oreType = PlayerInven.baseCraft.craftList[(int)item.itemId - 1].requireOre[0];
            int oreCnt = (PlayerInven.baseCraft.craftList[(int)item.itemId - 1].requireCnt[0]) / 2;
            PlayerInven.baseCraft.oreCnts[oreType] += oreCnt;   //분해하면 재료 절반 받음

            dissolution_Txt.gameObject.SetActive(true);
            isDissolFadeOut = true;
            dissolution_Txt.alpha = 1;

            string dissol_str = "";

            switch(oreType)
            {
                case 0:
                    dissol_str = "크리스탈";
                    break;
                case 1:
                    dissol_str = "아이언";
                    break;
                case 2:
                    dissol_str = "미네랄";
                    break;
                case 3:
                    dissol_str = "코어";
                    break;
                case 4:
                    dissol_str = "소울젬";
                    break;
                case 5:
                    dissol_str = "레드스톤";
                    break;
            }

            dissolution_Txt.text = dissol_str + " + " + oreCnt;

            itemList.Remove(GetNth(idx));
        }

        PlayerInven.baseCraft.RefreshOreTexts();

        RefreshUI();    //삭제 후 UI 초기화~
        pInven.BringAllItem();
        pInven.SaveInven();
    }

    public void CheckEquipsAndClear(int idx)   //아이템 삭제 시, 해당 아이템을 장비했었다면 장착 해제 시킴
    {
        if(mainIdx == idx)
            ClearEquip(ChangeTarget.MainWeapon);

        if (subIdx == idx)
            ClearEquip(ChangeTarget.SubWeapon);

        if (helmetIdx == idx)
            ClearEquip(ChangeTarget.Helmet);

        if (armorIdx == idx)
            ClearEquip(ChangeTarget.Armor);

        if (shoulderIdx == idx)
            ClearEquip(ChangeTarget.Shoulder);

        if (gloveIdx == idx)
            ClearEquip(ChangeTarget.Glove);

        if (pantsIdx == idx)
            ClearEquip(ChangeTarget.Pants);

        if (shoesIdx == idx)
            ClearEquip(ChangeTarget.Shoes);

        if (accIdx == idx)
            ClearEquip(ChangeTarget.Acc);
    }

    /** 장비칸 선택 함수 **/

    public void SelectMain()    //메인무기 슬롯 선택
    {
        selectedEquip = ChangeTarget.MainWeapon;
        AllOffTarget();
        mainTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectSub()     //서브무기 슬롯 선택
    {
        selectedEquip = ChangeTarget.SubWeapon;
        AllOffTarget();
        subTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectHelmet()
    {
        selectedEquip = ChangeTarget.Helmet;
        AllOffTarget();
        helmetTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectArmor()
    {
        selectedEquip = ChangeTarget.Armor;
        AllOffTarget();
        armorTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectShoulder()
    {
        selectedEquip = ChangeTarget.Shoulder;
        AllOffTarget();
        shoulderTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectGlove()
    {
        selectedEquip = ChangeTarget.Glove;
        AllOffTarget();
        gloveTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectPants()
    {
        selectedEquip = ChangeTarget.Pants;
        AllOffTarget();
        pantsTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectShoes()
    {
        selectedEquip = ChangeTarget.Shoes;
        AllOffTarget();
        shoesTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    public void SelectAcc()
    {
        selectedEquip = ChangeTarget.Acc;
        AllOffTarget();
        accTarget.gameObject.SetActive(true);

        UI_ToEquipChange();
    }

    //모든 타겟 이미지 끔 (장비 슬롯에 빨간색 테두리)
    void AllOffTarget()
    {
        mainTarget.gameObject.SetActive(false);
        subTarget.gameObject.SetActive(false);
        helmetTarget.gameObject.SetActive(false);
        armorTarget.gameObject.SetActive(false);
        shoulderTarget.gameObject.SetActive(false);
        gloveTarget.gameObject.SetActive(false);
        pantsTarget.gameObject.SetActive(false);
        shoesTarget.gameObject.SetActive(false);
        accTarget.gameObject.SetActive(false);
    }

    void UI_ToEquipChange()    //아이템 슬롯에 장비 교체 함수 장착
    {
        foreach (GameObject obj in slots)
        {
            Button btn = obj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate () { btn.GetComponent<Button_InvenSlot>().OnButtonChangeEquip(); });
        }
    }


    /** 장비 교체 함수 **/

    public void ChangeEquip(int idx, int doSave = 0)
    {
        if(idx == -1)       // 장비 슬롯이 빈칸일 때, DB에 -1으로 표시되어있음. 빈 슬롯 그대로 두면 되므로 리턴
        {
            return;
        }

        //다른 종류의 장비는 이럴일이 없으나 무기는 슬롯이 두개이므로 같은 장비를 두 슬롯에 동시에 낄 수 없도록 조치 (완전히 같은 장비를 메인에도 서브에도 끼는 경우 방지)
        if (idx == mainIdx || idx == subIdx && idx != -1)       
            return;

        int tempNum = (int)selectedEquip == 0 || (int)selectedEquip == 1 ? 0 : (int)selectedEquip - 1;  //아이템과 해당 장비 자리가 맞는지 확인할 int (무기 자리에는 무기만, 갑옷 자리에는 갑옷만)

        if (tempNum != (int)(GetNth(idx).Value.itemType))   //현재 선택된 장비 슬롯과 착용하려는 장비의 타입을 비교해 알맞는지 확인
        {
            Debug.Log(selectedEquip.ToString() + " 장비 슬롯에 올바르지 않은 아이템 타입을 장비하려 시도했음, 들어온 타입 : " + (int)GetNth(idx).Value.itemType);
            return;
        }


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
        {
            pInven.SaveInven();

        }
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
        int powerSum = 0;

        if (mainWeapon == Equipment.None)
            mainWeaponImage.sprite = null;
        else {
            mainWeaponImage.sprite = spriteList[(int)mainWeapon - 1];
            powerSum += GetNth(mainIdx).Value.GetPower();
        }

        if (subWeapon == Equipment.None)
            subWeaponImage.sprite = null;
        else
        {
            subWeaponImage.sprite = spriteList[(int)subWeapon - 1];
            powerSum += GetNth(subIdx).Value.GetPower();
        }

        if (helmet == Equipment.None)
            helmetImage.sprite = null;
        else
        {
            helmetImage.sprite = spriteList[(int)helmet - 1];
            powerSum += GetNth(helmetIdx).Value.GetPower();
        }

        if (armor == Equipment.None)
            armorImage.sprite = null;
        else
        {
            armorImage.sprite = spriteList[(int)armor - 1];
            powerSum += GetNth(armorIdx).Value.GetPower();
        }

        if (shoulder == Equipment.None)
            shoulderImage.sprite = null;
        else
        {
            shoulderImage.sprite = spriteList[(int)shoulder - 1];
            powerSum += GetNth(shoulderIdx).Value.GetPower();
        }

        if (glove == Equipment.None)
            gloveImage.sprite = null;
        else
        {
            gloveImage.sprite = spriteList[(int)glove - 1];
            powerSum += GetNth(gloveIdx).Value.GetPower();
        }

        if (pants == Equipment.None)
            pantsImage.sprite = null;
        else
        {
            pantsImage.sprite = spriteList[(int)pants - 1];
            powerSum += GetNth(pantsIdx).Value.GetPower();
        }

        if (shoes == Equipment.None)
            shoesImage.sprite = null;
        else
        {
            shoesImage.sprite = spriteList[(int)shoes - 1];
            powerSum += GetNth(shoesIdx).Value.GetPower();
        }

        if (acc == Equipment.None)
            accImage.sprite = null;
        else
        {
            accImage.sprite = spriteList[(int)acc - 1];
            powerSum += GetNth(accIdx).Value.GetPower();
        }

        pInven.KPM.power = powerSum;
        powerValueText.text = pInven.KPM.power.ToString();
        pInven.KPM.SaveStatus();
    }

    /** 플레이어 인벤 연동 **/

    public void BringAllItems(int[] intSlot, int gold)
    {
        for(int i=0; i<intSlot.Length; i++)
        {
            AddItem(intSlot[i]);
        }

        invenGold = gold;
        gold_Txt.text = invenGold + " G";
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
