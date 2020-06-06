using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public GunTest gun;

    public int index;
    private void OnTriggerEnter(Collider other)
    {
        if (!gun.photonView.IsMine)
            return;

        if(other.attachedRigidbody)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                gun.photonView.RPC("Restore", RpcTarget.AllViaServer);
                other.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, 10);
            }
        }
    }
}
