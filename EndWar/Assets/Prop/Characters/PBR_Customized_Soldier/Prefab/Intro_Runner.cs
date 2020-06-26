using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Runner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 3f * Time.deltaTime);
    }
}
