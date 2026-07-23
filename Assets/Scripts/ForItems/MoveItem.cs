using UnityEngine;

public class MoveItem : MonoBehaviour
{
    public Transform cameraTransform;

    public Vector3 currentOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(currentOffset);
        transform.rotation = cameraTransform.rotation;
    }
}
