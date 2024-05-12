using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform target;
    public Vector3 desiredPos;
    private float smoothSpeed = 0.1f;
    public Vector3 offset;

    void FixedUpdate()
    {
        desiredPos = target.transform.position + offset;
        Vector3 smoothendPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothendPos;
    }
}
