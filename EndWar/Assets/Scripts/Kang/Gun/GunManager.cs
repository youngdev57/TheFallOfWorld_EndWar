﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GunManager : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grapAction;

    public Transform muzzleTr;
    public GameObject muzzleEffect;

    public GameObject bullet;
    public float speed;

    private List<GameObject> bulletArray;

    public AudioSource audioSource;
    public AudioClip[] sfxArray;

    [Space(10)]
    public Slider reloadingSlider;
    public Text bulletText;

    [Space(10)]
    public Animator anim;

    int bulletCount = 0;
    
    [Space(10)]
    public int reloadCount;
    public float reloadTime;
    public int damage = 0;
    public float delay = 0.75f;
    float timer = 0f;

    bool isFire = false;
    bool canFire = true;
    bool isReloading = false;
    //bool isRestore = true;
    

    void Start()
    {
        bulletArray = new List<GameObject>();
        reloadingSlider.maxValue = 1f;
        reloadingSlider.value = 1f;
    }

    void OnEnable()
    {
        if (isReloading)
            reloadingSlider.value = 0;
    }
    
    void Fire()
    {
        if (!isFire)
            return;

        RaycastHit hit;

        StartCoroutine(FireEffect());

        canFire = false;
    }

    IEnumerator FireEffect()
    {
        muzzleEffect.SetActive(true);

        BulletPulling();
        audioSource.PlayOneShot(sfxArray[Random.Range(0, sfxArray.Length)]);
        bulletCount++;

        if (bulletCount >= reloadCount)
        {
            isReloading = true;
            reloadingSlider.value = 0;
            reloadingSlider.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.15f);
        
        muzzleEffect.SetActive(false);
    }

    //총알 부족하면 소환 아니면 재활용함
    void BulletPulling()
    {
        GameObject temp = null;
        for (int i = 0; i < bulletArray.Count; i++)
        {
            if (!bulletArray[i].activeSelf)
            {
                temp = bulletArray[i];
                bulletArray[i].SetActive(true);
                break;
            }
        }
        if (temp == null)
        {
            temp = Instantiate(bullet, muzzleTr.position, Quaternion.identity);
            temp.GetComponent<Bullet>().damage = damage;
            bulletArray.Add(temp);
        }

        temp.transform.position = muzzleTr.position;
        temp.GetComponent<Rigidbody>().AddForce(muzzleTr.right * speed);
    }

    void Reloading()
    {
        if (isReloading)
        {
            reloadingSlider.value += Time.deltaTime / reloadTime;
            if (reloadingSlider.value >= reloadingSlider.maxValue)
            {
                isReloading = false;
                bulletCount = 0;
                reloadingSlider.value = 1f;
                reloadingSlider.gameObject.SetActive(false);
            }
        }
    }
    
    void Update()
    {
        if (PlayerManager.isDie == true)
            return;

        if(!canFire)
            timer += Time.deltaTime;

        if(timer >= delay)
        {
            canFire = true;
            timer = 0;
        }

        if (isReloading != true && canFire && grapAction.GetState(handType))
        {
            if (anim != null)
                anim.SetTrigger("Fire");

            isFire = true;
            canFire = false;
            Fire();
        }

        if(grapAction.GetStateUp(handType))
        {
            isFire = false;
        }

        Reloading();

        bulletText.text = string.Format("{0}", reloadCount - bulletCount);
    }
}
