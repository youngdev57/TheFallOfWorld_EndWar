using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public Inventory inven;

    public GameObject craftPanel;

    void Start()
    {
        
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            ShowCraftUI();
        }
    }

    void ShowCraftUI()
    {
        craftPanel.SetActive(!craftPanel.activeSelf);
    }
}
