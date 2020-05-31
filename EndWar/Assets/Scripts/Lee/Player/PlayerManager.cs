using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    public int p_HP;    
    public int p_MP;
    public int p_DEF;



    [PunRPC]
    public void GetDamage(float damage)
    {
        Debug.Log("아프다~~~~~ 행~복~~해~~~줘~~어~~~~" + this.gameObject.name + ", " + damage);
    }
}
