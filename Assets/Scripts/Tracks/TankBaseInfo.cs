using UnityEngine;

public class TankBaseInfo : MonoBehaviour
{
    public float tankBaseSpeed = 15;
    public float tankBaseRotationSpeed = 30;
    public TankBaseType tankBaseType;
    public Animator animator;
    public float rotateAngle;
    public Transform[] frontWhells;
}

public enum TankBaseType
{
    Track,
    Whells
}
