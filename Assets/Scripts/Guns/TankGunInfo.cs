using UnityEngine;

[RequireComponent(typeof(TankShooting))]
public class TankGunInfo : MonoBehaviour
{
    [SerializeField] private float tankHeadSpeed = 10;
    [SerializeField] private float recoil = 500;

    public float GetTankGunSpeed()
    {
        return tankHeadSpeed;
    }

    public float GetTankRecoil()
    {
        return recoil;
    }
}
