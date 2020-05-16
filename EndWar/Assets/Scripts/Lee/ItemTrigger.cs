using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemTrigger : MonoBehaviourPun
{
    public Collider coll;

    [PunRPC]
    public void OnGrab(bool isGrab)
    {
        coll.isTrigger = isGrab;
        GetComponent<Rigidbody>().useGravity = !isGrab;
    }
}
