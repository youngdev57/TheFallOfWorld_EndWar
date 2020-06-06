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
            if(other.gameObject.name.Contains("Target"))
            {
                gun.photonView.RPC("Restore", Photon.Pun.RpcTarget.AllViaServer);
                other.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllBuffered, 10);
                Debug.Log("Restored!! " + other.gameObject.name);
                other.GetComponent<MeshRenderer>().material.color = Color.blue;
                other.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }
    }
}
