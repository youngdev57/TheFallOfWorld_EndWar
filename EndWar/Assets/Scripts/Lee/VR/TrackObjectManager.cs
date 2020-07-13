using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackObjectManager : MonoBehaviour
{
    void Awake()
    {
        if (GetComponent<Camera>())
        {
            GetComponent<Camera>().enabled = false;
            transform.Find("Camera (ears)").gameObject.SetActive(false);
        }

        if (GetComponent<SteamVR_TrackedObject>())
            GetComponent<SteamVR_TrackedObject>().enabled = false;

        if (GetComponent<SteamVR_Behaviour_Pose>())
            GetComponent<SteamVR_Behaviour_Pose>().enabled = false;
    }
}
