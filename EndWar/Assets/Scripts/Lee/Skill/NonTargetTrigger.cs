using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NonTargetTrigger : MonoBehaviour
{
    public int speed;
    public int damage;
    public float seconds;

    void Start()
    {
        Destroy(gameObject, seconds);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(""))
        {

        }
    }
}
