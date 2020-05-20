using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class CustomBehaviour : SteamVR_Behaviour
{
    PhotonView myPv;

    new void Awake()
    {
        myPv = GetComponent<PhotonView>();
        myPv.RPC("C_Awake", RpcTarget.AllBuffered, null);
    }

    new void OnEnable()
    {
        myPv.RPC("C_OnEnable", RpcTarget.AllBuffered, null);
    }

    new void OnDisable()
    {
        myPv.RPC("C_OnDisable", RpcTarget.AllBuffered, null);
    }
    new void FixedUpdate()
    {
        myPv.RPC("C_FixedUpdate", RpcTarget.AllBuffered, null);
    }
    new void LateUpdate()
    {
        myPv.RPC("C_LateUpdate", RpcTarget.AllBuffered, null);
    }
    new void Update()
    {
        myPv.RPC("C_Update", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    void C_Awake()
    {
        base.Awake();
    }
    [PunRPC]
    void C_OnEnable()
    {
        base.OnEnable();
    }
    [PunRPC]
    void C_OnDisable()
    {
        base.OnDisable();
    }
    [PunRPC]
    void C_FixedUpdate()
    {
        base.FixedUpdate();
    }
    [PunRPC]
    void C_LateUpdate()
    {
        base.LateUpdate();
    }
        [PunRPC]
    void C_Update()
    {
        base.Update();
    }
}
