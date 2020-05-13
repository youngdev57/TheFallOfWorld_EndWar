using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GunTest gun;

    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody)
        {
            gun.Restore();
        }
    }
}
