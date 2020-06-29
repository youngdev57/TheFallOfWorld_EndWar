using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_PostPackOpen : MonoBehaviour
{
    Button btn;
    public MailPost postManager;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate () { Action(); });
    }

    public void Action()
    {
        postManager.OpenPack(gameObject.name);
    }
}
