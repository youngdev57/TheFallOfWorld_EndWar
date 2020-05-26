using UnityEngine;

public class JointPositioning : MonoBehaviour
{
    public ConfigurableJoint joint;
    public Transform target;
    public Vector3 offset;
    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.rotation;
    }
    void Update()
    {
        //joint.connectedAnchor = transform.rotation * offset + target.position - GameController.instance.player.CameraRig.position;
        //joint.targetRotation = Quaternion.Inverse(target.rotation);

        joint.configuredInWorldSpace = true;
        SetRotation(joint, target.rotation, startRotation);
        joint.connectedAnchor = joint.transform.rotation * offset + target.position - joint.connectedBody.position;
    }
    void SetRotation(ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation)
    {
        var right = joint.axis;
        var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
        var up = Vector3.Cross(forward, right).normalized;
        Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);
        Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

        resultRotation *= startRotation * Quaternion.Inverse(targetRotation);
        resultRotation *= worldToJointSpace;
        joint.targetRotation = resultRotation;
    }
}