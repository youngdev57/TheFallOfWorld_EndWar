using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointRepos : MonoBehaviour
{
    bool isWall = false;

    private void Update()
    {
        if (isWall)
        {
            transform.position = new Vector3(transform.parent.position.x + Random.Range(-50f, 50f), 0, transform.parent.position.z + Random.Range(-50f, 50f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 인식");
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("벽 인식");
            isWall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            isWall = false;
        }
    }
}
