using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public bool attack;
    public float moveSpeed = 5f;
    public float rotSpeed = 120f;

    private Transform tr;
    private PhotonView pv = null;
    private Animator avater;

    private float h, v;

    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        avater = GetComponent<Animator>();

        pv = GetComponent<PhotonView>();

        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        currPos = tr.position;
        currRot = tr.rotation;

        attack = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬플레이어의 정보 송신
        if (stream.isWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else // 원격 플레이어 정보 수신
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private void Update()
    {
        if (pv.isMine)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // 회전과 이동 처리
            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);

            //  로컬, 원격의 같은 플레이어 애니메이션 동기화를 위해 로컬, 원격 메소드를 동시 호출
            pv.RPC("SetAni_Speed",PhotonTargets.All, v);

            if (Input.GetButtonDown("Jump"))
            {
                pv.RPC("SetAni_Jump", PhotonTargets.All);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                // 모든 코루틴 멈춤
                StopAllCoroutines();
                // 포톤에 모든 동일한 객체 ( 내 캐릭터와 상대방 화면의 내 캐릭터에 모두 SetAni_Kick 메소드 호출 )
                pv.RPC("SetAni_Kick", PhotonTargets.All);
            }
        }
        else // 원격 플레이어 위치를 자연스럽게 이동 및 회전
        {
            tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 3f);
        }
    }

    [PunRPC]
    private void SetAni_Speed(float speed)
    {
        avater.SetFloat("speed", speed);
    }
    [PunRPC]
    private void SetAni_Jump()
    {
        avater.SetTrigger("jump");
    }
    [PunRPC]
    private void SetAni_Kick()
    {
        attack = true;
        avater.SetTrigger("kick");
        StartCoroutine(AttackReset());
    }
    [PunRPC]
    private void SetAni_Hit()
    {
        avater.SetTrigger("Hit");
    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(1f);
        attack = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 콜라이더에 걸린 객체의 태그가 Player 이고 해당 객체의 PlayerCtrl 컴포넌트의 attack 값이 true 일 때
        if (collision.gameObject.GetComponent<PlayerCtrl>().attack && collision.gameObject.tag == "Player")
        {
            // 포톤에 모든 동일한 객체 ( 내 캐릭터와 상대방 화면의 내 캐릭터에 모두 SetAni_Hit 메소드 호출 )
            pv.RPC("SetAni_Hit", PhotonTargets.All);
        }
    }
}
