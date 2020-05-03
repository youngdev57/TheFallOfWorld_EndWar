using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCtrl : MonoBehaviour
{
    [PunRPC]
    public void GetDamage(float damage)
    {
        Debug.Log("아프다~~~~~ 행~복~~해~~~줘~~어~~~~" + this.gameObject.name + ", " + damage);
    }
}
