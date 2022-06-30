using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followCamera;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10;
    [SerializeField] private Vector3 offset;

    private Vector3 followPosition;

    private void FixedUpdate()
    {
        followPosition = target.position + offset;
        followCamera.position = Vector3.Lerp(followCamera.position, followPosition, Time.deltaTime * followSpeed);
    }
}
