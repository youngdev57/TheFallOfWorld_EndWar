using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro_Movie : MonoBehaviour
{
    public Image mainLogo;
    float logoAlpha = 0f;

    public float mainTimer;

    bool startLogoFadeIn = false;
    bool startLogoFadeOut = false;

    void Update()
    {
        mainTimer += Time.deltaTime;

        if(mainTimer >= 8f && mainTimer < 20f)
        {
            startLogoFadeIn = true;
        }

        if(startLogoFadeIn && !startLogoFadeOut)
        {
            float r = mainLogo.color.r;
            float g = mainLogo.color.g;
            float b = mainLogo.color.b;
            mainLogo.color = new Color(r, g, b, logoAlpha);

            if(logoAlpha < 1f)
                logoAlpha += 0.001f;
        }

        if(mainTimer >= 20f && mainTimer < 30f)
        {
            startLogoFadeOut = true;
            startLogoFadeIn = false;
            Debug.Log("페이드 아웃");
        }

        if(startLogoFadeOut && !startLogoFadeIn)
        {
            Debug.Log("페이드 아웃 진행 중 " + logoAlpha);
            float r = mainLogo.color.r;
            float g = mainLogo.color.g;
            float b = mainLogo.color.b;
            mainLogo.color = new Color(r, g, b, logoAlpha);

            if (logoAlpha > 0f)
                logoAlpha -= 0.001f;
        }

        transform.Rotate(Vector3.up * Time.deltaTime * 0.2f);
    }




}
