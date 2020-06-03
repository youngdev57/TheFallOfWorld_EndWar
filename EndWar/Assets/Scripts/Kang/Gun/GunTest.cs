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

    private GameObject bulletEffect;

    public AudioSource audioSource;
    public AudioClip[] sfxArray;

    float delay = 0.75f;
    float timer = 0f;

    bool isFire = false;
    bool canFire = true;
    bool isRestore = true;
    

    void Start()
    {
        bulletEffect = PhotonNetwork.Instantiate("BulletTrailer", muzzleTr.position, Quaternion.identity);
        bulletEffect.SetActive(false);

        if (!photonView.IsMine)
            return;

        bulletEffect.GetComponent<Bullet>().gun = this;
    }

    [PunRPC]
    void Fire()
    {
        if (!isFire || !canFire)
            return;

        RaycastHit hit;

        isRestore = false;
        StartCoroutine(FireEffect());

        if(Physics.Raycast(muzzleTr.position, muzzleTr.right, out hit, 5000f))
        {
            if(hit.collider.attachedRigidbody)
            {
                Debug.Log("Hit : " + hit.collider.gameObject.name);
            }
        }
        
        canFire = false;
    }

    IEnumerator FireEffect()
    {
        bulletEffect.SetActive(true);
        muzzleEffect.SetActive(true);
        bulletEffect.GetComponent<Rigidbody>().AddForce(muzzleTr.right * 8000f);
        audioSource.PlayOneShot(sfxArray[Random.Range(0, sfxArray.Length)]);

        yield return new WaitForSeconds(0.15f);

        muzzleEffect.SetActive(false);

        yield return new WaitForSeconds(0.55f);

        photonView.RPC("Restore", RpcTarget.AllViaServer);
        //Restore();
    }

    [PunRPC]
    public void Restore()
    {
        isRestore = true;
        bulletEffect.transform.position = muzzleTr.position;
        bulletEffect.SetActive(false);
        bulletEffect.GetComponent<Rigidbody>().velocity = Vector3.zero;
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

        if (isRestore)
            bulletEffect.transform.position = muzzleTr.position;

        if (grapAction.GetLastState(handType) && canFire)
        {
            isFire = true;
            //Fire();
            photonView.RPC("Fire", RpcTarget.AllViaServer);
        }

        if(grapAction.GetLastStateUp(handType))
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
