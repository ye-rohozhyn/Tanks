using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform target;
    [SerializeField] private Transform enemyTankHead;
    [SerializeField] private TankBaseInfo track;
    [SerializeField] private TankGunInfo gun;
    [SerializeField] private float stopDistance = 15;
    [SerializeField] private float viewDistance = 25;
    [SerializeField] private float tankGroundDrag = 6;
    [SerializeField] private float tankAirDrag = 2;
    [SerializeField] private float sphereRadius = 0.1f;
    [SerializeField] private Transform centerOfMass;
    [SerializeField] private LayerMask groundMask;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private Vector3 _nextPoint;
    private Vector3[] path;
    private float _movemetSpeed = 10;
    private float _rotationSpeed = 5;
    private float _distanceToTarget = 0;
    private TankShooting _tankShooting;
    private float _tankRecoil = 0;
    private bool isGrounded = false;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.position;

        _movemetSpeed = track.tankBaseSpeed;
        _rotationSpeed = gun.GetTankGunSpeed();
        _tankRecoil = gun.GetTankRecoil();
        _tankShooting = gun.GetComponent<TankShooting>();
    }

    private void Update()
    {
        CheckGroud();
    }

    private void CheckGroud()
    {
        isGrounded = Physics.CheckSphere(_transform.position, sphereRadius, groundMask);
        _rigidbody.drag = isGrounded ? tankGroundDrag : tankAirDrag;
    }

    private void FixedUpdate()
    {
        _distanceToTarget = Vector3.Distance(_transform.position, target.position);

        if (_distanceToTarget <= viewDistance)
        {
            if (_distanceToTarget > stopDistance)
            {
                _nextPoint = GetNextPoint();
                _nextPoint.y = _transform.position.y;

                EnemyRotation(_transform, _nextPoint);
                MoveForvard();
            }

            EnemyRotation(enemyTankHead, target.position);

            if (_tankShooting.GetCurrentTimer() <= 0)
            {
                _rigidbody.AddForce(-_transform.forward * _tankRecoil, ForceMode.VelocityChange);
                _tankShooting.Shoot();
            }
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
        Vector3 movePosition = _transform.forward * 1f * (isGrounded ? _movemetSpeed : _movemetSpeed / (tankGroundDrag / tankAirDrag));
        _rigidbody.AddForce(movePosition, ForceMode.Acceleration);
    }

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
}
