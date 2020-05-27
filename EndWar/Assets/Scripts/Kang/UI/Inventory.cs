using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public LinkedList<Item> itemList;
    public List<Sprite> spriteList;

    public List<GameObject> slots;
    public GameObject itemPrefab;

    public InputField del_Input;

    bool isFirst = true;

    void Start()
    {
        itemList = new LinkedList<Item>();

        itemList.AddLast(new Item("그냥 권총", 1, 0));
        itemList.AddLast(new Item("쎈 권총", 1, 1));

        foreach(GameObject slot in slots)
        {
            GameObject ins = Instantiate(itemPrefab);
            ins.transform.parent = slot.transform;
            ins.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            ins.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            ins.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
            ins.SetActive(false);
        }

        RefreshUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            AddItem();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            RemoveItem(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RemoveItem(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RemoveItem(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RemoveItem(3);
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

    void RefreshUI()
    {
        AllOff();

        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject itemClone = slots[i].transform.GetChild(0).gameObject;
            itemClone.SetActive(true);

            Text nameText = itemClone.transform.GetChild(0).GetComponent<Text>();
            Text typeText = itemClone.transform.GetChild(1).GetComponent<Text>();

            itemClone.transform.GetChild(2).GetComponent<Image>().sprite = spriteList[GetNth(i).Value.itemId];

            nameText.text = GetNth(i).Value.itemName;
            typeText.text = GetNth(i).Value.itemType.ToString();
        }

        isFirst = false;
    }

    void AllOff()
    {
        if (slots[0].transform.childCount == 0)
            return;

        foreach (GameObject slot in slots)
        {
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void AddItem()
    {
        Item tempItem = new Item("임시템", 0, 1);
        if(itemList.Count == 28)
        {
            Debug.Log("인벤토리 꽉 참");
        } else
        {
            itemList.AddLast(tempItem);
        }

        RefreshUI();
    }

    public void RemoveItem(int idx) {
        if(GetNth(idx) != null)
            itemList.Remove(GetNth(idx));

        RefreshUI();
    }

    LinkedListNode<Item> GetNth(int idx) {
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
