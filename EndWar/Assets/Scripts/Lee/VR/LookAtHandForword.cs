using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class LookAtHandForword : MonoBehaviour
{
    [Space(5)]
    public Transform pivot;

    float speed = 0f;

    void Start(){}

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, pivot.eulerAngles.y, 0));
    }
}
