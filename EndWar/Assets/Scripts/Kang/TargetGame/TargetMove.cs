using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    Horizontal_Left,
    Horizontal_Right,
    Vertical_Up,
    Vertical_Down
}

public class TargetMove : MonoBehaviour
{
    public float speed;

    public GameObject breakEffectPrefab;
    GameObject breakEffect;

    internal Target_Sender sender;

    public TargetType type;

    Vector3 dir;

    void Start()
    {
        switch (type)
        {
            case TargetType.Horizontal_Left:
                dir = Vector3.forward;
                break;

            case TargetType.Horizontal_Right:
                dir = Vector3.back;
                break;

            case TargetType.Vertical_Up:
                dir = Vector3.up;
                break;

            case TargetType.Vertical_Down:
                dir = Vector3.down;
                break;
        }
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Contains("Bullet"))
        {
            //점수 상승
            ScoreManager.GetInstance().AddScore(10);
            ShowEffect();
            GameObject fText = FloatingTextPool.GetInstance().GetFloatingText(transform.position);
            fText.transform.position = transform.position;
            fText.SetActive(true);
            sender.Restore(gameObject);
        }
    }

    void ShowEffect()
    {
        if(breakEffect == null)
        {
            breakEffect = Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            breakEffect.transform.position = transform.position;
            breakEffect.GetComponent<ParticleSystem>().Play();
        }
    }
}
