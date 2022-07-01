using UnityEngine;

public class TankGunInfo : MonoBehaviour
{
    [SerializeField] private float tankHeadSpeed = 10;

    public float GetTankGunSpeed()
    {
        return tankHeadSpeed;
    }
}
