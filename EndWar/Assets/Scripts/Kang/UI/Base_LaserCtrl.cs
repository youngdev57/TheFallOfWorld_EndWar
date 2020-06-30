using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Base_LaserCtrl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PhotonView>().IsMine && other.tag == "Player")
        {
            other.GetComponentsInChildren<UI_Laser>()[0].enabled = true;
            other.GetComponentsInChildren<UI_Laser>()[1].enabled = true;
            other.GetComponentsInChildren<UI_Laser>()[0].LaserOn();
            other.GetComponentsInChildren<UI_Laser>()[1].LaserOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PhotonView>().IsMine && other.tag == "Player")
        {
            other.GetComponentsInChildren<UI_Laser>()[0].LaserOff();
            other.GetComponentsInChildren<UI_Laser>()[1].LaserOff();
            other.GetComponentsInChildren<UI_Laser>()[0].enabled = false;
            other.GetComponentsInChildren<UI_Laser>()[1].enabled = false;
        }
    }
}
