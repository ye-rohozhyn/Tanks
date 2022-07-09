using UnityEngine;

public class Pursuer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 10;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        _transform.position = Vector3.Lerp(_transform.position, target.position, Time.deltaTime * smoothSpeed);
    }
}
