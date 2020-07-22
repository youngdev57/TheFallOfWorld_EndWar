using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Monster : MonoBehaviour
{
    internal bool isFold = false;

    internal Animator anim;

    IEnumerator overIE;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Bullet") && !isFold && ScoreManager.GetInstance().gameStart)
        {
            Fold(true);
            StopCoroutine(overIE);
        }
    }

    public void Fold(bool gainScore)
    {
        Debug.Log("Fold 진입");
        isFold = true;

        if (gainScore)
        {
            ScoreManager.GetInstance().AddScore(10);
            ScoreManager.GetInstance().TargetRestore();
            GameObject fText = FloatingTextPool.GetInstance().GetFloatingText(transform.position);
            fText.transform.position = transform.position;
            fText.SetActive(true);
        }
        //Fold Animation
        anim.SetTrigger("Fold");
    }

    public void Stand()
    {
        Debug.Log("Stand 진입");
        isFold = false;
        //Stand Animation
        anim.SetTrigger("Stand");

        overIE = TimeOver();
        StartCoroutine(overIE);
    }

    IEnumerator TimeOver()
    {
        Debug.Log("타임 오버 진입");
        yield return new WaitForSeconds(6f);

        if (!isFold)
            Fold(false);

        Debug.Log("타임 오버 6초 대기 완료");
        ScoreManager.GetInstance().AddScore(-5);
        ScoreManager.GetInstance().TargetRestore();
    }
}
