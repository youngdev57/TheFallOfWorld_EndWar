using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Ship_to_earth : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 15f);
    }
}
