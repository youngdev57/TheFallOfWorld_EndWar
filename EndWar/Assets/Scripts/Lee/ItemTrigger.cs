using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public Collider coll;

    int index = 0;

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
