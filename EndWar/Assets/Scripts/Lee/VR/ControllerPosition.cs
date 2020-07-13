using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerPosition : MonoBehaviour
{
    public int index = 1;
    public GameObject Head;

    ViveManager viveManager;
    void Awake()
    {
        FindVive();
        FindParent();
    }

    void Update()
    {
        switch (index)
        {
            case 0:
                transform.position = viveManager.origin.transform.position;
                transform.rotation = viveManager.origin.transform.rotation;
                break;
            case 1:
                transform.position = viveManager.head.transform.position;
                transform.rotation = viveManager.head.transform.rotation;
                break;
            case 2:
                transform.position = viveManager.leftHand.transform.position;
                transform.rotation = viveManager.leftHand.transform.rotation;
                break;
            case 3:
                transform.position = viveManager.rightHand.transform.position;
                transform.rotation = viveManager.rightHand.transform.rotation;
                break;
            case 4:
                transform.position = viveManager.leftHand.transform.position;
                break;
        }
    }

    public void FindVive()
    {
        ViveManager[] vive = FindObjectsOfType<ViveManager>(); 
        for (int i = 0; i < vive.Length; i++)
        {
            if (!vive[i].transform.Find(Head.name))
            {
                viveManager = vive[i];
                return;
            }
        }
    }

    public void FindParent()
    {
        Transform obj = viveManager.myBody.transform;

        switch (index)
        {
            case 2:
                obj = viveManager.leftHand.transform;
                break;
            case 3:
                obj = viveManager.rightHand.transform;
                break;
        }

        if (obj != null && !obj.Find(gameObject.name) && index != 0)
            transform.parent = obj;
    }
}
