using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class UILookAt : MonoBehaviour
{
    public Camera camera;

    void Start()
    {
        camera = SteamVR_Render.Top().camera;
    }

    void Update()
    {
        transform.LookAt(camera.transform.position);
    }
}
