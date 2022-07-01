using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class PlayerTankMovement : MonoBehaviour
{
    [SerializeField] private Transform tankBase;
    [SerializeField] private Transform tankHead;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private TankBaseInfo[] tracks;
    [SerializeField] private TankGunInfo[] guns;
    [SerializeField] private float tankRigidbodyDrag = 6;

    //Save variables
    private int _trackIndex = 0;
    private int _gunIndex = 0;

    //Tank base movement
    private Rigidbody _tankRigidbody;
    private Vector3 _movePosition;
    private Quaternion _moveRotation;

    //Tank properties
    private float _tankBaseSpeed = 0;
    private float _tankBaseRotationSpeed = 0;
    private float _tankHeadSpeed = 0;

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

        _trackIndex = PlayerPrefs.GetInt("Active track", 0);
        _gunIndex = PlayerPrefs.GetInt("Active gun", 0);

        tracks[_trackIndex].transform.gameObject.SetActive(true);
        guns[_gunIndex].transform.gameObject.SetActive(true);

        if (tracks[_trackIndex] != null)
        {
            _tankBaseSpeed = tracks[_trackIndex].GetTankBaseSpeed();
            _tankBaseRotationSpeed = tracks[_trackIndex].GetTankBaseRotationSpeed();
        }

        if (guns[_gunIndex] != null)
        {
            _tankHeadSpeed = guns[_gunIndex].GetTankGunSpeed();
        }
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
        _movePosition = tankBase.forward * _vertical * _tankBaseSpeed;
        _tankRigidbody.AddForce(_movePosition, ForceMode.Acceleration);

        //Rotation the tank base
        _moveRotation = tankBase.rotation * Quaternion.Euler(Vector3.up * (_tankBaseRotationSpeed * _horizontal * Time.deltaTime));
        _tankRigidbody.MoveRotation(_moveRotation);
    }

    private void LookTheTarget()
    {
        _lookDirection = (tankHead.position - lookTarget.position).normalized;
        _lookRotation = Quaternion.LookRotation(new Vector3(-_lookDirection.x, 0f, -_lookDirection.z));
        tankHead.rotation = Quaternion.Lerp(tankHead.rotation, _lookRotation, Time.deltaTime * _tankHeadSpeed);
    }
}
