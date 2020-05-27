using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Craft craft;
    public CraftCtrl craftCtrl;
    public Inventory inven;
    public InvenCtrl invenCtrl;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            craftCtrl.ShowCraftUI();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            invenCtrl.ShowInventoryUI();
        }
    }
}
