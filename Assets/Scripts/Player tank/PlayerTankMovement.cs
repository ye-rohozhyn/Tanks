using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class PlayerTankMovement : MonoBehaviour
{
    [SerializeField] private Transform tankBase;
    [SerializeField] private Transform tankHead;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private TankBaseInfo[] tracks;
    [SerializeField] private TankGunInfo[] guns;
    [SerializeField] private float tankGroundDrag = 6;
    [SerializeField] private float tankAirDrag = 2;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private LayerMask groundMask;

    //Saved variables
    [SerializeField] private int _trackIndex = 0;
    [SerializeField] private int _gunIndex = 0;

    //Tank base movement
    private Rigidbody _tankRigidbody;
    private Vector3 _movePosition;
    private Quaternion _moveRotation;
    private Animator _tankAnimator;
    private float _rotateAngle;
    private Transform[] _frontWhells;

    //Tank properties
    private float _tankBaseSpeed = 0;
    private float _tankBaseRotationSpeed = 0;
    private float _tankHeadSpeed = 0;
    private float _tankRecoil = 0;
    private bool isGrounded = false;
    private TankShooting tankShooting;

    //tank head rotation
    private Vector3 _lookDirection;
    private Quaternion _lookRotation;

    //Inputs
    private float _vertical;
    private float _horizontal;

    private void Start()
    {
        _tankRigidbody = GetComponent<Rigidbody>();
        _tankRigidbody.drag = tankGroundDrag;
        _tankRigidbody.centerOfMass = centerOfMass.position;

        _trackIndex = PlayerPrefs.GetInt("Active track", _trackIndex);
        _gunIndex = PlayerPrefs.GetInt("Active gun", _gunIndex);

        tracks[_trackIndex].transform.gameObject.SetActive(true);
        guns[_gunIndex].transform.gameObject.SetActive(true);

        tankShooting = guns[_gunIndex].GetComponent<TankShooting>();

        if (tracks[_trackIndex] != null)
        {
            _tankBaseSpeed = tracks[_trackIndex].tankBaseSpeed;
            _tankBaseRotationSpeed = tracks[_trackIndex].tankBaseRotationSpeed;
            if (tracks[_trackIndex].tankBaseType == TankBaseType.Track)
            {
                _tankAnimator = tracks[_trackIndex].animator;
            }
            else if (tracks[_trackIndex].tankBaseType == TankBaseType.Whells)
            {
                _frontWhells = tracks[_trackIndex].frontWhells;
                _rotateAngle = tracks[_trackIndex].rotateAngle;
            }
        }

        if (guns[_gunIndex] != null)
        {
            _tankHeadSpeed = guns[_gunIndex].GetTankGunSpeed();
            _tankRecoil = guns[_gunIndex].GetTankRecoil();
        }
    }

    private void Update()
    {
        Inputs();
        CheckGroud();
    }

    private void FixedUpdate()
    {
        if (_vertical != 0) TankMovement();
        LookTheTarget();
    }

    private void Inputs()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");

        if (_tankAnimator != null)
        {
            _tankAnimator.SetFloat("Factor", _vertical);
        }
        else
        {
            foreach (Transform whell in _frontWhells)
            {
                whell.localRotation = Quaternion.Lerp(whell.localRotation, Quaternion.Euler(0, _horizontal * _rotateAngle, 0), 0.1f);
            }
        }

        if (Input.GetMouseButtonDown(0) & tankShooting != null)
        {
            if (tankShooting.GetCurrentTimer() <= 0)
            {
                _tankRigidbody.AddForce(-tankHead.forward * _tankRecoil, ForceMode.VelocityChange);
            }

            tankShooting.Shoot();
        }
    }

    private void CheckGroud()
    {
        isGrounded = Physics.CheckSphere(tankBase.position, sphereRadius, groundMask);
        _tankRigidbody.drag = isGrounded ? tankGroundDrag : tankAirDrag;
    }

    private void TankMovement()
    {
        //Moving forward and backward
        _movePosition = tankBase.forward * _vertical * (isGrounded ? _tankBaseSpeed : _tankBaseSpeed / (tankGroundDrag / tankAirDrag));
        _tankRigidbody.AddForce(_movePosition, ForceMode.Acceleration);

        //Rotation the tank base
        _moveRotation = tankBase.rotation * Quaternion.Euler(Vector3.up * (_tankBaseRotationSpeed * _horizontal * Time.deltaTime));
        _tankRigidbody.MoveRotation(_moveRotation);
    }

    private void LookTheTarget()
    {
        _lookDirection = (lookTarget.localPosition - tankHead.localPosition).normalized;
        _lookRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0f, _lookDirection.z));
        tankHead.localRotation = Quaternion.Lerp(tankHead.localRotation, _lookRotation, Time.deltaTime * _tankHeadSpeed);
    }
}
