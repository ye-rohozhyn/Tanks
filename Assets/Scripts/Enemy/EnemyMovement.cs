using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Tank components")]
    [SerializeField] private Transform enemyTankHead;
    [SerializeField] private NavMeshAgent navMeshAgent;

    [Header("Tank info")]
    [SerializeField] private TankBaseInfo track;
    [SerializeField] private TankGunInfo gun;
    [SerializeField] private TankHealth tankHealth;

    [Header("Tank properties")]
    [SerializeField] private float stopDistance = 15;
    [SerializeField] private float viewDistance = 25;
    [SerializeField] private float tankGroundDrag = 6;
    [SerializeField] private float tankAirDrag = 2;
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private LayerMask groundMask;

    [Header("Target")]
    [SerializeField] private Transform target;

    //Tank properties
    private float _movemetSpeed = 10;
    private float _rotationSpeed = 5;
    private float _distanceToTarget = 0;
    private float _tankRecoil = 0;

    //Tank info
    private Transform _transform;
    private Rigidbody _rigidbody;
    private Animator _tankAnimator;
    private TankShooting _tankShooting;

    //Find path
    private Vector3 _nextPoint;
    private Vector3[] path;

    //Helper variables
    private bool isGrounded = false;
    private float _vertical = 0;
    private bool isLive = true;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;

        _movemetSpeed = track.tankBaseSpeed;
        _rotationSpeed = gun.GetTankGunSpeed();
        _tankRecoil = gun.GetTankRecoil();
        _tankShooting = gun.GetComponent<TankShooting>();
        if (track.tankBaseType == TankBaseType.Track)
        {
            _tankAnimator = track.animator;
        }
    }

    private void Update()
    {
        if (tankHealth.GetHealth() == 0) isLive = false;

        if (isLive)
        {
            CheckGroud();
        }

        TankAnimations();
    }

    private void FixedUpdate()
    {
        if (isLive)
        {
            MovementLogic();
        }
        else if (_vertical > 0)
        {
            _vertical -= Time.deltaTime;
        }
    }

    #region - Update functions -

    private void CheckGroud()
    {
        isGrounded = Physics.CheckSphere(_transform.position, sphereRadius, groundMask);
        _rigidbody.drag = isGrounded ? tankGroundDrag : tankAirDrag;
    }

    private void TankAnimations()
    {
        if (_tankAnimator != null)
        {
            _tankAnimator.SetFloat("Factor", _vertical);
        }
    }

    #endregion

    #region - Movement functions -

    private void MovementLogic()
    {
        _distanceToTarget = Vector3.Distance(_transform.position, target.position);

        if (_distanceToTarget <= viewDistance)
        {
            if (_distanceToTarget > stopDistance)
            {
                _nextPoint = GetNextPoint();
                _nextPoint.y = _transform.position.y;

                EnemyRotation(_transform, _nextPoint);
                if (_vertical < 1) _vertical += Time.deltaTime;
            }
            else if(_vertical > 0)
            {
                _vertical -= Time.deltaTime;
            }

            MoveForvard();
            EnemyRotation(enemyTankHead, target.position);

            if (_tankShooting.GetCurrentTimer() <= 0)
            {
                _rigidbody.AddForce(-_transform.forward * _tankRecoil, ForceMode.VelocityChange);
                _tankShooting.Shoot();
            }
        }
        else if (_vertical > 0)
        {
            _vertical -= Time.deltaTime;
        }
    }

    private void EnemyRotation(Transform rotationObject, Vector3 targetObject)
    {
        Vector3 lookDirection = (targetObject - rotationObject.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0f, lookDirection.z));
        rotationObject.rotation = Quaternion.Lerp(rotationObject.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
    }

    private void MoveForvard()
    {
        Vector3 movePosition = _transform.forward * _vertical * (isGrounded ? _movemetSpeed : _movemetSpeed / (tankGroundDrag / tankAirDrag));
        _rigidbody.AddForce(movePosition, ForceMode.Acceleration);
    }

    #endregion

    #region - Helper functions -

    private Vector3 GetNextPoint()
    {
        navMeshAgent.SetDestination(target.position);
        path = navMeshAgent.path.corners;

        List<float> distances = new List<float>();

        foreach(Vector3 point in path)
        {
            distances.Add(Vector3.Distance(_transform.position, point));
        }

        float min = distances[0];
        int minIndex = 0;

        for (int i = 0; i < distances.Count; i++)
        {
            if (min > distances[i])
            {
                min = distances[i];
                minIndex = i;
            }
        }

        if (minIndex + 1 == distances.Count) return path[minIndex];
        else return path[minIndex + 1];
    }

    #endregion
}
