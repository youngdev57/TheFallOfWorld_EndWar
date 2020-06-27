using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class GunTest : MonoBehaviourPunCallbacks
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grapAction;

    public Transform muzzleTr;
    public GameObject muzzleEffect;

    public GameObject bullet;

    private List<GameObject> bulletArray;

    public AudioSource audioSource;
    public AudioClip[] sfxArray;

    [Space(10)]
    public Animator anim;

    int index = 0;

    [Space(10)]
    public int damage = 0;
    public float delay = 0.75f;
    float timer = 0f;

    bool isFire = false;
    bool canFire = true;
    //bool isRestore = true;
    

    void Start()
    {
        bulletArray = new List<GameObject>();
    }

    [PunRPC]
    void Fire()
    {
        if (!isFire || !canFire)
            return;

        RaycastHit hit;

        photonView.RPC("FireEffect", RpcTarget.AllBuffered);

        if (Physics.Raycast(muzzleTr.position, muzzleTr.right, out hit, 5000f))
        {
            if(hit.collider.attachedRigidbody)
            {
                Debug.Log("Hit : " + hit.collider.gameObject.name);
            }
        }

        canFire = false;
    }

    [PunRPC]
    IEnumerator FireEffect()
    {
        if (anim != null)
            anim.SetTrigger("Fire");

        muzzleEffect.SetActive(true);

        BulletPulling();
        audioSource.PlayOneShot(sfxArray[Random.Range(0, sfxArray.Length)]);
        index++;

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
            temp = PhotonNetwork.Instantiate(bullet.name, muzzleTr.position, Quaternion.identity);
            temp.GetComponent<Bullet>().damage = damage;
            bulletArray.Add(temp);
        }

        temp.transform.position = muzzleTr.position;
        temp.GetComponent<Rigidbody>().AddForce(muzzleTr.right * 8000f);
    }
    
    void Update()
    {
        if (!photonView.IsMine || PlayerManager.isDie == true)
            return;

        timer += Time.deltaTime;

        if(timer >= delay)
        {
            canFire = true;
            timer -= delay;
        }

        if (grapAction.GetState(handType) && canFire)
        {
            if (index >= bulletArray.Count)
                index = 0;

            isFire = true;
            //Fire();
            photonView.RPC("Fire", RpcTarget.AllViaServer);
        }

        if(grapAction.GetStateUp(handType))
        {
            isFire = false;
        }
    }
}
