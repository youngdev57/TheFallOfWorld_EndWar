using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_InvenSlot : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate () { OnButtonChangeEquip(); });
    }

    public void OnButtonChangeEquip()
    {
        Inventory inven = GetComponentInParent<Inventory>();
        inven.UI_ChangeWeapon(this.gameObject.name);
    }

    public void OnButtonRemoveItem()
    {
        Inventory inven = GetComponentInParent<Inventory>();
        inven.UI_RemoveItem(int.Parse(gameObject.name));
    }
}
