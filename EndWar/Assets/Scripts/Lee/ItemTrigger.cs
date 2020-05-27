using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemTrigger : MonoBehaviourPun
{
    public Collider coll;

    int index = 0;

    [PunRPC]
    public void OnGrab(bool isGrab)
    {
        if (isGrab)
        {
            index++;
            coll.isTrigger = isGrab;
            GetComponent<Rigidbody>().useGravity = !isGrab;
        }
        else
        {
            index--;

            if (index == 0)
            {
                coll.isTrigger = isGrab;
                GetComponent<Rigidbody>().useGravity = !isGrab;
            }
        }
    }
}
