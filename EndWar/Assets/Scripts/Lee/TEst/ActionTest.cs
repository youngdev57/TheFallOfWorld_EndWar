using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ActionTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType;      //모두 사용 ,왼손, 오른손
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean grabAction;

    private void Update()
    {
        if (GetTeleportDown())
        {
            Debug.Log("teleport");
        }
        if (GetGarb())
        {
            Debug.Log("Grab");
        }
    }

    public bool GetTeleportDown()
    {
        return teleportAction.GetStateDown(handType);
    }

    public bool GetGarb()
    {
        return grabAction.GetStateDown(handType);
    }
}
