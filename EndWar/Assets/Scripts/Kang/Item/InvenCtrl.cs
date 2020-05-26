using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenCtrl : MonoBehaviour
{
    public GameObject inventoryCanvas;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }
}
