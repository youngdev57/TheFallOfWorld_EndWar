using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop_FlightMove : MonoBehaviour
{
    public float speed = 150f;
    public float maxDist = 30000f;
    float distance = 0f;
    Vector3 originPosition;

    void Start()
    {
        originPosition = this.transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        distance += speed * Time.deltaTime;

        if(distance >= maxDist)
        {
            distance -= maxDist;
            this.transform.position = originPosition;
        }
    }
}
