using System.Collections;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    //General
    public ShootingType shootingType = new ShootingType();
    public float lifeTime = 5f;
    private float _timer = 0;

    //OneGun
    public Rigidbody shell;
    public Transform startPosition;

    //General for ballistic and many guns
    public Rigidbody[] shells;
    public Transform[] startPositions;

    //BallisticGun
    public Transform target;
    public float shotAngle;
    public bool atIntervals = false;
    public float interval;
    private float _speed = 0;
    private bool blocked = false;

    //SplashGun
    public ParticleSystem splashEffect;
    public float length;
    public float timeReload;

    //General for one and many guns
    public float power;

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else if (_timer <= 0)
        {
            _timer = 0;
            ResetShells();
        }
    }

    #region - Shoot functions -

    public void Shoot()
    {
        if (_timer > 0 || blocked) return;

        switch (shootingType)
        {
            case ShootingType.OneGun:
                OneGunShoot();
                break;
            case ShootingType.ManyGuns:
                ManyGunsShoot();
                break;
            case ShootingType.BallisticGun:
                blocked = true;
                _speed = CalculateSpeedForShoot();
                StartCoroutine(BallisticGunShoot());
                break;
            case ShootingType.SplashGun:
                SplashGunShoot();
                break;
            default:
                break;
        }
    }

    private void OneGunShoot()
    {
        shell.transform.position = startPosition.position;
        shell.transform.gameObject.SetActive(true);
        shell.isKinematic = false;
        shell.transform.rotation = startPosition.rotation;

        shell.velocity = power * startPosition.forward;

        _timer = lifeTime;
    }

    private void ManyGunsShoot()
    {
        for (int i = 0; i < shells.Length; i++)
        {
            shells[i].transform.position = startPositions[i].position;
            shells[i].transform.gameObject.SetActive(true);
            shells[i].isKinematic = false;
            shells[i].transform.rotation = startPositions[i].rotation;

            shells[i].velocity = power * startPositions[i].forward;
        }

        _timer = lifeTime;
    }

    private IEnumerator BallisticGunShoot()
    {
        _timer = lifeTime;

        for (int i = 0; i < shells.Length; i++)
        {
            shells[i].transform.position = startPositions[i].position;
            shells[i].transform.gameObject.SetActive(true);
            shells[i].isKinematic = false;
            shells[i].transform.rotation = startPositions[i].rotation;
            shells[i].velocity = _speed * startPositions[i].forward;

            yield return new WaitForSeconds(atIntervals ? interval : 0);
        }
        
        blocked = false;
    }

    private void SplashGunShoot()
    {
        _timer = lifeTime;

        splashEffect.startLifetime = length;
        if (splashEffect.isStopped) { splashEffect.Play(); }
    }

    #endregion

    #region - Helper functions -

    private void ResetShells()
    {
        switch (shootingType)
        {
            case ShootingType.OneGun:

                if (shell.isKinematic) return;

                shell.isKinematic = true;
                shell.velocity = Vector3.zero;
                shell.transform.gameObject.SetActive(false);
                break;
            case ShootingType.ManyGuns:

                if (shells[0].isKinematic) return;

                for (int i = 0; i < shells.Length; i++)
                {
                    shells[i].isKinematic = true;
                    shells[i].velocity = Vector3.zero;
                    shells[i].transform.gameObject.SetActive(false);
                }
                break;
            case ShootingType.BallisticGun:

                if (shells[0].isKinematic) return;

                for (int i = 0; i < shells.Length; i++)
                {
                    shells[i].isKinematic = true;
                    shells[i].velocity = Vector3.zero;
                    shells[i].transform.gameObject.SetActive(false);
                }
                break;
            case ShootingType.SplashGun:
                if (splashEffect.isPlaying)
                {
                    splashEffect.Stop();
                    _timer = timeReload;
                }
                break;
            default:
                break;
        }
    }

    private float CalculateSpeedForShoot()
    {
        Vector3 fromTo = target.position - transform.position;

        float y = fromTo.y;
        fromTo.y = 0;

        float x = fromTo.magnitude;
        float angleInRadians = shotAngle * Mathf.PI / 180;

        float v2 = (Physics.gravity.y * Mathf.Pow(x, 2)) /
            (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));

        return Mathf.Sqrt(Mathf.Abs(v2));
    }

    public float GetCurrentTimer()
    {
        return _timer;
    }

    #endregion
}

public enum ShootingType
{
    OneGun,
    ManyGuns,
    BallisticGun,
    SplashGun
}