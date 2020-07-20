using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target_Monster : MonoBehaviour
{
    internal bool isFold = true;

    internal Animator anim;

    IEnumerator overIE;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Bullet") && !isFold)
        {
            Fold(true);
        }
    }

    public void Fold(bool gainScore)
    {
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

        StopAllCoroutines();
    }

    public void Stand()
    {
        isFold = false;
        //Stand Animation
        anim.SetTrigger("Stand");

        StopAllCoroutines();

        overIE = TimeOver();
        StartCoroutine(overIE);
    }

    IEnumerator TimeOver()
    {
        yield return new WaitForSeconds(6f);

        if (!isFold)
            Fold(false);

        ScoreManager.GetInstance().AddScore(-5);
        ScoreManager.GetInstance().TargetRestore();
    }
}
