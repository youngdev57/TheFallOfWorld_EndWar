using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class GunTest : MonoBehaviourPunCallbacks, IPunPrefabPool
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grapAction;

    public Transform muzzleTr;
    public GameObject muzzleEffect;

    public GameObject bulletEffect;

    private Bullet[] bulletArray;

    public AudioSource audioSource;
    public AudioClip[] sfxArray;

    int index = 0;

    float delay = 0.75f;
    float timer = 0f;

    bool isFire = false;
    bool canFire = true;
    //bool isRestore = true;
    

    void Start()
    {
        bulletArray = bulletEffect.GetComponentsInChildren<Bullet>(true);
        for (int i = 0; i < bulletArray.Length; i++)
        {
            bulletArray[i].gameObject.SetActive(false);

            bulletArray[i].gun = this;
            bulletArray[i].index = this.index++;
        }
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
        bulletArray[_index].gameObject.SetActive(true);
        muzzleEffect.SetActive(true);
        bulletArray[_index].GetComponent<Rigidbody>().AddForce(muzzleTr.right * 8000f);
        audioSource.PlayOneShot(sfxArray[Random.Range(0, sfxArray.Length)]);

        yield return new WaitForSeconds(0.15f);
        
        muzzleEffect.SetActive(false);

        yield return new WaitForSeconds(0.55f);
        photonView.RPC("Restore", RpcTarget.AllViaServer, _index);
        //Restore();
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
        if (!photonView.IsMine)
            return;

        timer += Time.deltaTime;

        if(timer >= delay)
        {
            canFire = true;
            timer -= delay;
        }

        if (grapAction.GetStateDown(handType) && canFire)
        {
            if (grapAction.GetState(handType) && canFire)
            {
                if (index >= bulletArray.Length)
                    index = 0;

                Debug.Log(index);
                isFire = true;
                //Fire();
                photonView.RPC("Fire", RpcTarget.AllViaServer);
                index++;
            }
        }

        if(grapAction.GetStateUp(handType))
        {
            isFire = false;
        }
    }

    public new GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        throw new System.NotImplementedException();
    }

    public void Destroy(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }
}
