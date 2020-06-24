using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Debug.Log("********************** 보스 맞음");
        }
    }
}
