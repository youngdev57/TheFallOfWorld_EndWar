using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEnter : MonoBehaviour
{
    GameObject playerObj;

    public GameObject invalidStart;

    public Button start_btn, ready_btn, cancel_btn, invalidStartOK_btn;

    private void Start()
    {
        start_btn.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9))
        {
            EnterDungeon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerObj = other.gameObject;

            playerObj.GetComponentsInChildren<UI_Laser>()[0].enabled = true;
            playerObj.GetComponentsInChildren<UI_Laser>()[1].enabled = true;
            playerObj.GetComponentsInChildren<UI_Laser>()[0].LaserOn();
            playerObj.GetComponentsInChildren<UI_Laser>()[1].LaserOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject == playerObj)
            {
                playerObj.GetComponentsInChildren<UI_Laser>()[0].LaserOff();
                playerObj.GetComponentsInChildren<UI_Laser>()[1].LaserOff();
                playerObj.GetComponentsInChildren<UI_Laser>()[0].enabled = false;
                playerObj.GetComponentsInChildren<UI_Laser>()[1].enabled = false;

                playerObj = null;
            }
        }
            
    }

    public void EnterDungeon()
    {
        LoadingManager.LoadScene("Dungeon_Underground");
    }
}
