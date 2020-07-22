using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreText_Floating : MonoBehaviour
{
    TextMeshProUGUI textComp;

    internal Vector3 startPos;

    float alpha = 1f;

    void Start()
    {
        textComp = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetInit()
    {
        textComp = GetComponentInChildren<TextMeshProUGUI>();
        transform.position = startPos;
        textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, alpha = 1f);
    }

    void Update()
    {
        alpha -= 0.01f;
        textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, alpha);

        if(alpha <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
