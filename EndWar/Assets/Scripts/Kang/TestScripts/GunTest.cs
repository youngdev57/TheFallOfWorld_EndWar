using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class GunTest : MonoBehaviourPunCallbacks
{
    public GameObject rightHand;

    private SteamVR_Input_Sources handType;
    private SteamVR_Action_Boolean grapAction;

    public Transform muzzleTr;
    public GameObject muzzleEffect;
    public GameObject bulletEffect;

    public AudioSource audioSource;
    public AudioClip[] sfxArray;

    float delay = 0.5f;
    float timer = 0f;

    bool isFire = false;
    bool canFire = true;
    

    void Start()
    {
        handType = SteamVR_Input_Sources.RightHand;
        grapAction = SteamVR_Actions.default_Grap;
    }

    void Fire()
    {
        if (!isFire)
            return;

        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(muzzleTr.position, muzzleTr.forward);

        StartCoroutine(FireEffect());

        if(Physics.Raycast(ray, out hit, 5000f))
        {
            if(hit.collider.attachedRigidbody)
            {
                Debug.Log("Hit : " + hit.collider.gameObject.name);
            }
        }

        
    }

    IEnumerator FireEffect()
    {
        bulletEffect.transform.localPosition = muzzleTr.localPosition;
        bulletEffect.SetActive(true);
        muzzleEffect.SetActive(true);
        bulletEffect.GetComponent<Rigidbody>().AddForce(muzzleTr.right * 8000f);
        audioSource.PlayOneShot(sfxArray[Random.Range(0, sfxArray.Length)]);

        yield return new WaitForSeconds(0.15f);

        muzzleEffect.SetActive(false);

        yield return new WaitForSeconds(0.15f);

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

        if(grapAction.GetLastState(handType) && canFire)
        {
            canFire = false;
            Fire();
            isFire = true;
        }

        if(grapAction.GetLastStateUp(handType))
        {
            isFire = false;
        }
    }
}
