using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCtrl : MonoBehaviour
{
    public GameObject craftCanvas;

    public void ShowCraftUI()
    {
        craftCanvas.SetActive(!craftCanvas.activeSelf);
    }
}
