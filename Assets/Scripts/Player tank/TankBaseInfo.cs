using UnityEngine;

public class TankBaseInfo : MonoBehaviour
{
    [SerializeField] private float tankBaseSpeed = 15;
    [SerializeField] private float tankBaseRotationSpeed = 30;
    [SerializeField] private Animator animator;

    public float GetTankBaseSpeed()
    {
        return tankBaseSpeed;
    }

    public float GetTankBaseRotationSpeed()
    {
        return tankBaseRotationSpeed;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
