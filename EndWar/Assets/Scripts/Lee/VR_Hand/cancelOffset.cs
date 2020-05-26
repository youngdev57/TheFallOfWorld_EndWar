using UnityEngine;

public class cancelOffset : MonoBehaviour
{
    public Transform root;
    public Transform transformToOffset;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transformToOffset.rotation) * transform.rotation;
        transform.localPosition = Quaternion.Inverse(root.rotation) * -(transformToOffset.position - transform.position);

    }
}