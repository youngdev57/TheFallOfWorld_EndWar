using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPoint;

    public GameObject player;

    void Start()
    {
        RankManager.GetInstance().LoadFromJSON();
    }

    void Update()
    {  
        
    }
}
