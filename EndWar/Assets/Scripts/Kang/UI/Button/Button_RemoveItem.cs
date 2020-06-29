using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_RemoveItem : MonoBehaviour
{
    public List<Button> slotBtns;

    Inventory inven;

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        inven = GetComponentInParent<Inventory>();
    }

    public void OnClickRemoveBtn()
    {
        foreach(Button btn in slotBtns)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate() { btn.GetComponent<Button_InvenSlot>().OnButtonRemoveItem(); });
        }
    }
}
