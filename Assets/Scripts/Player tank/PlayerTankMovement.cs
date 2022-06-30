using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class PlayerTankMovement : MonoBehaviour
{
    [Header("Tank model")]
    [SerializeField] private Transform tankBase;
    [SerializeField] private Transform tankHead;

    [Header("Tank properties")]
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float tankHeadSpeed = 10;
    [SerializeField] private float tankBaseSpeed = 15;
    [SerializeField] private float tankBaseRotationSpeed = 30;
    [SerializeField] private float tankRigidbodyDrag = 6;

    //Tank base movement
    private Rigidbody _tankRigidbody;
    private Vector3 _movePosition;
    private Quaternion _moveRotation;

    //tank head rotation
    private Vector3 _lookDirection;
    private Quaternion _lookRotation;

    //Inputs
    private float _vertical;
    private float _horizontal;

    private void Start()
    {
        _tankRigidbody = GetComponent<Rigidbody>();
        _tankRigidbody.drag = tankRigidbodyDrag;
    }

    private void FixedUpdate()
    {
        Inputs();
        if (_vertical != 0) TankMovement();
        LookTheTarget();
    }

    private void Inputs()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");
    }

    private void TankMovement()
    {
        //Moving forward and backward
        _movePosition = tankBase.forward * _vertical * tankBaseSpeed;
        _tankRigidbody.AddForce(_movePosition, ForceMode.Acceleration);

        //Rotation the tank base
        _moveRotation = tankBase.rotation * Quaternion.Euler(Vector3.up * (tankBaseRotationSpeed * _horizontal * Time.deltaTime));
        _tankRigidbody.MoveRotation(_moveRotation);
    }

    private void LookTheTarget()
    {
        _lookDirection = (tankHead.position - lookTarget.position).normalized;
        _lookRotation = Quaternion.LookRotation(new Vector3(-_lookDirection.x, 0f, -_lookDirection.z));
        tankHead.rotation = Quaternion.Lerp(tankHead.rotation, _lookRotation, Time.deltaTime * tankHeadSpeed);
    }
}
