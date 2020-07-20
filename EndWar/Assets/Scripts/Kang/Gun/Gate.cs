using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Minigame_Target,
    Dungeon,
    Minigame_Grab,
    SnowField
}

public class Gate : MonoBehaviour
{
    public SceneName sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            switch(sceneName)
            {
                case SceneName.Minigame_Target:
                    LoadingManager.LoadScene("Minigame_Target");
                    break;
            }
        }
    }
}
