using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemTrigger : MonoBehaviourPun
{
    [PunRPC]
    public void OnGrab(bool isGrab)
    {
        GetComponent<BoxCollider>().isTrigger = isGrab;
        GetComponent<Rigidbody>().useGravity = !isGrab;
    }
}
