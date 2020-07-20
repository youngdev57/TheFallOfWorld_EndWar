using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Effect : MonoBehaviour
{
    public GameObject effect;
    public ScoreManager SCM;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Bullet"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            SCM.Play();
            Destroy(gameObject);
        }
    }
}
