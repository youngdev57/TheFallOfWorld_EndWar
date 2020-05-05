using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerMovement : MonoBehaviour
{
    public float sensitivity = .1f;
    public float maxSpeed = 1f;

    public SteamVR_Action_Boolean movePress;
    public SteamVR_Action_Vector2 moveValue;

    float speed = 0f;
    public Transform cameraTr;
    void Start()
    {
        cameraTr = SteamVR_Render.Top().head;
    }

    void Update()
    {

        transform.rotation = Quaternion.Euler(new Vector3(0, cameraTr.eulerAngles.y, 0));
        CalculateMovement();
    }

    void CalculateMovement()
    {
        Vector3 orientationEuler = new Vector3(0f, 0f, 0f);
        Quaternion orientation = Quaternion.Euler(orientationEuler);
        Vector3 movement = Vector3.zero;

        if (movePress.GetStateUp(SteamVR_Input_Sources.Any))
            speed = 0;

        if (movePress.state)
        {
            speed = moveValue.axis.y * sensitivity;
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

            movement += orientation * (speed * transform.forward) * Time.deltaTime;

            speed = moveValue.axis.x * sensitivity;
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

            movement += orientation * (speed * transform.right) * Time.deltaTime;
        }
        transform.parent.position += movement;
    }
}
