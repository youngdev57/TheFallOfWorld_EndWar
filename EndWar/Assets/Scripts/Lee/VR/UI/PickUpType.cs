using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpType : MonoBehaviour
{
    public GameObject canvasUi;
    public GameObject skillManager;
    public GameObject pickUpType;

    public void OffObj()
    {
        canvasUi.SetActive(false);
        skillManager.SetActive(false);
        pickUpType.SetActive(true);
    }
}
