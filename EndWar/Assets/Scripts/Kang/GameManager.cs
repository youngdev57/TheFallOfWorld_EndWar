using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform startPoint;

    public GameObject player;

    void Awake()
    {
        DontDestroyOnLoad(this);    //씬 이동해도 삭제하지 않음

        player = Instantiate(playerPrefab, startPoint.position, Quaternion.identity);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            //던전 강제 이동
            LoadingManager.LoadScene("Dungeon_Underground");
        }           
    }
}
