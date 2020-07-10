﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointRepos : MonoBehaviour
{
    bool isWall = false;

    private void Update()
    {
        if (isWall)
        {
            transform.position = new Vector3(transform.parent.position.x + Random.Range(-25f, 25f), transform.parent.position.y, transform.parent.position.z + Random.Range(-25f, 25f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
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
