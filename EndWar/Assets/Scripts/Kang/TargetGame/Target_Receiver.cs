using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Receiver : MonoBehaviour
{
    public Target_Sender sender;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Respawn"))
        {
            ScoreManager.GetInstance().AddScore(-1);
            sender.Restore(other.gameObject);
            Debug.Log("TriggerEnter");
        }
    }
}
