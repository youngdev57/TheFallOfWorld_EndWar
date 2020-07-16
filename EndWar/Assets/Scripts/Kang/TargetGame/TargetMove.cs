using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{
    public float speed;

    internal Target_Sender sender;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            //점수 상승
            ScoreManager.GetInstance().AddScore(10);
            sender.Restore(gameObject);
        }
    }
}
