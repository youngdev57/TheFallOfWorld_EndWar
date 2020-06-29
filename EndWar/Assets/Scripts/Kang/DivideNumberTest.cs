using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideNumberTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string test = "777";
        string[] wt = test.Split('^');

        Debug.Log(wt.Length + "길이");
        Debug.Log(wt[0]);
        Debug.Log(wt.Length == 1 ? "Cant" : wt[1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
