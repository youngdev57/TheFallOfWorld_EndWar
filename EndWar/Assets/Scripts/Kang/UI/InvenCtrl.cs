using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenCtrl : MonoBehaviour
{
    public GameObject inventoryCanvas;

    public void ShowInventoryUI()
    {
        inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
    }
}
