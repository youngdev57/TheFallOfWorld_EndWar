using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameButton
{
    Left,
    Center,
    Right
}

public class Button_SingleBaseUI : MonoBehaviour
{
    Button button;

    public GameButton gameButton;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { OnClick(); });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(gameButton == GameButton.Left)
            {
                OnClick();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (gameButton == GameButton.Center)
            {
                OnClick();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (gameButton == GameButton.Right)
            {
                OnClick();
            }
        }
    }

    public void OnClick() => LoadingManager.LoadScene(gameObject.name);
}
