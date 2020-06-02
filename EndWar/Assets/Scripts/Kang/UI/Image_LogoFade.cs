using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_LogoFade : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer target;

    float alpha = 0.0f;

    void Start()
    {
        target = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float r = target.color.r;
        float g = target.color.g;
        float b = target.color.b;

        alpha += 0.0014f;

        target.color = new Color(r, g, b, alpha);

        if (alpha >= 1)
            this.enabled = false;
    }
}
