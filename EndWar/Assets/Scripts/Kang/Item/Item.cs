using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int itemType;
    public int itemId;

    public Item(string name, int type, int id) {
        this.itemName = name;
        this.itemType = type;
        this.itemId = id;
    }
}
