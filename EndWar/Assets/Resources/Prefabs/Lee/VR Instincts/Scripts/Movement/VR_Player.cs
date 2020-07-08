using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;
public class VR_Player : MonoBehaviourPun
{
    private Vector2 trackpad;
    private Vector3 moveDirection;
    private CapsuleCollider CapCollider;
    private Rigidbody RBody;

    public SteamVR_Input_Sources MovementHand;//Set Hand To Get Input From
    public SteamVR_Action_Vector2 TrackpadAction;
    public SteamVR_Action_Boolean JumpAction;
    public SteamVR_Action_Boolean openUI;

    [Space(5)]
    public float jumpHeight;
    public float MovementSpeed;
    public float Deadzone;//the Deadzone of the trackpad. used to prevent unwanted walking.
    public GameObject Head;
    public GameObject AxisHand;//Hand Controller GameObject
    public PhysicMaterial NoFrictionMaterial;
    public PhysicMaterial FrictionMaterial;
    public bool TouchingGround;
    public GameObject skillUICanvas;

    private void Start()
    {
        CapCollider = GetComponent<CapsuleCollider>();
        RBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!photonView.IsMine || PlayerManager.isDie == true)
            return;

        Rigidbody RBody = GetComponent<Rigidbody>();
        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward* trackpad.magnitude;//get the angle of the touch and correct it for the rotation of the controller
        updateInput();
        updateCollider();
        if (trackpad.magnitude > Deadzone)
        {//make sure the touch isn't in the deadzone and we aren't going to fast.

            CapCollider.material = NoFrictionMaterial;
            if (TouchingGround) {
                if (JumpAction.GetStateDown(MovementHand))
                {
                    float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * 9.81f);
                    RBody.AddForce(0, jumpSpeed, 0, ForceMode.VelocityChange);
                    //RBody.AddForce(moveDirection.x * MovementSpeed - RBody.velocity.x, 0, moveDirection.z * MovementSpeed - RBody.velocity.z, ForceMode.VelocityChange);
                }
                RBody.AddForce(moveDirection.x * MovementSpeed-RBody.velocity.x, 0, moveDirection.z * MovementSpeed-RBody.velocity.z, ForceMode.VelocityChange);
            }
            else
            {
                RBody.AddForce(moveDirection.x*MovementSpeed/( Mathf.Sqrt(2 * jumpHeight * 9.81f)/(9.81f)) * Time.fixedDeltaTime, 0, moveDirection.z *MovementSpeed/ (Mathf.Sqrt(2 * jumpHeight * 9.81f) / (9.81f)) * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
        else
        {
            CapCollider.material = FrictionMaterial;
        }

        if (openUI.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            photonView.RPC("OpenUI", RpcTarget.AllBuffered, null);
        }
    }

    public static float Angle(Vector2 p_vector2)
    {
        
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }
    private void updateInput()
    {
        if(TrackpadAction.GetActive(MovementHand)) trackpad = TrackpadAction.GetAxis(MovementHand);
        if (TouchingGround && TrackpadAction.GetAxis(MovementHand) == Vector2.zero)
            RBody.velocity = Vector3.zero;
    }
    private void updateCollider()
    {
        CapCollider.height = Head.transform.localPosition.y;
        CapCollider.center = new Vector3(Head.transform.localPosition.x, Head.transform.localPosition.y / 2, Head.transform.localPosition.z);
    }

    [PunRPC]
    void OpenUI()
    {
        skillUICanvas.SetActive(true);
        this.enabled = false;
    }

    void OnCollisionStay(Collision collision)
    {
        int layerMask = 10;
        if (collision.gameObject.layer == layerMask)
            TouchingGround = true;
    }

    void OnCollisionExit(Collision collision)
    {
        int layerMask = 10;
        if (collision.gameObject.layer == layerMask)
            TouchingGround = false;
    }
}
