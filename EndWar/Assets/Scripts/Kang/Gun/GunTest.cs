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

    public GameObject bulletEffect;
    public GameObject bullet;

    private List<GameObject> bulletArray;

    public AudioSource audioSource;
    public AudioClip[] sfxArray;

    int index = 0;

    public float delay = 0.75f;
    float timer = 0f;

    bool isFire = false;
    bool canFire = true;
    //bool isRestore = true;
    

    void Start()
    {
        /* bulletArray = bulletEffect.GetComponentsInChildren<Bullet>(true);
         for (int i = 0; i < bulletArray.Length; i++)
         {
             bulletArray[i].gameObject.SetActive(false);

             bulletArray[i].gun = this;
             bulletArray[i].index = this.index++;
         }*/
        bulletArray = new List<GameObject>();
    }

    [PunRPC]
    void Fire()
    {
        if (!isFire || !canFire)
            return;

        RaycastHit hit;

        //isRestore = false;
        photonView.RPC("FireEffect", RpcTarget.AllBuffered, index);
        //StartCoroutine(FireEffect());

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
    IEnumerator FireEffect(int _index)
    {
        //bulletArray[_index].gameObject.SetActive(true);
        //bulletArray[_index].GetComponent<Rigidbody>().AddForce(muzzleTr.right * 8000f);
        muzzleEffect.SetActive(true);

        BulletPulling();
        audioSource.PlayOneShot(sfxArray[Random.Range(0, sfxArray.Length)]);
        index++;

        yield return new WaitForSeconds(0.15f);
        
        muzzleEffect.SetActive(false);

        yield return new WaitForSeconds(0.55f);
        photonView.RPC("Restore", RpcTarget.AllViaServer, _index);
        //Restore();
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
            bulletArray.Add(temp);
            temp.GetComponent<Bullet>().index = bulletArray.Count - 1;
        }

        temp.GetComponent<Rigidbody>().AddForce(muzzleTr.right * 8000f);
    }

    [PunRPC]
    public void Restore(int _index)
    {
        //isRestore = true;
        GameObject bullet = bulletArray[_index].gameObject;
        bullet.transform.position = muzzleTr.position;
        bullet.SetActive(false);
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.transform.position = muzzleTr.position;
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
            if (grapAction.GetState(handType))
            {
                if (index >= bulletArray.Count)
                    index = 0;

                isFire = true;
                //Fire();
                photonView.RPC("Fire", RpcTarget.AllViaServer);
            }
        }

        if(grapAction.GetStateUp(handType))
        {
            isFire = false;
        }
    }
}
