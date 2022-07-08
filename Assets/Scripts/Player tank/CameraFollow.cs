using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followCamera;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float topBorder, bottomBorder, leftBorder, rightBorder;

    private Vector3 followPosition;

    private void FixedUpdate()
    {
        followPosition = target.position + offset;

        if (followPosition.x > rightBorder || followPosition.x < leftBorder) followPosition.x = followCamera.position.x;
        if (followPosition.z > topBorder || followPosition.z < bottomBorder) followPosition.z = followCamera.position.z;

        followCamera.position = Vector3.Lerp(followCamera.position, followPosition, Time.deltaTime * followSpeed);
    }
}
