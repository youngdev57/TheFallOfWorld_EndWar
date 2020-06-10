using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MPBar : MonoBehaviour
{
    public bool isUseSkill = true;

    public Slider slider;
    public MPBar nextMpBar;
    public IEnumerator useSkillCoroutine;
    public IEnumerator chargingCoroutine;
    void Start()
    {
        isUseSkill = true;
        slider = GetComponent<Slider>();
        setSliderValue(5);
    }

    public void setSliderValue(int i)
    {
        slider.maxValue = i;
        slider.value = i;
    }

    public void OffCoroutine()
    {
        if (chargingCoroutine != null)
            StopCoroutine(chargingCoroutine);
    }

    public IEnumerator ChargingCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        while (true)
        {
            slider.value += .1f;
            yield return new WaitForSeconds(0.1f);
            if (slider.value == slider.maxValue)
            {
                isUseSkill = true;
                break;
            }
        }
        //float i = slider.value; i <= slider.maxValue; i = slider.value

        if (nextMpBar != null)
        {
            chargingCoroutine = nextMpBar.ChargingCoroutine(0.5f);
            StartCoroutine(chargingCoroutine);
        }
        yield return null;
    }
}
