using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ItemSpawner : MonoBehaviour
{
    public GameObject crystal;
    public GameObject iron;
    public GameObject mineral;

    public Transform[] crystalPos;
    public Transform[] ironPos;
    public Transform[] mineralPos;
    
    void Start()
    {
        for (int i = 0; i < crystalPos.Length; i++)
        {
            Instantiate(crystal, crystalPos[i].position, crystal.transform.rotation);
        }

        for (int i = 0; i < ironPos.Length; i++)
        {
            Instantiate(iron, ironPos[i].position, iron.transform.rotation);
        }

        for (int i = 0; i < mineralPos.Length; i++)
        {
            Instantiate(mineral, mineralPos[i].position, mineral.transform.rotation);
        }
    }

}
