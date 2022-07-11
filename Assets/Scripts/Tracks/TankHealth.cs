using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private bool rotateToCamera = false;

    [Header("Death tank properties")]
    [SerializeField] private Rigidbody tankHead;
    [SerializeField] private Collider oldTankHeadCollider;
    [SerializeField] private Collider newTankHeadCollider;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private float explosionForce;

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }

    private void FixedUpdate()
    {
        if (rotateToCamera) healthBar.transform.LookAt(playerCamera.position);
    }

    public void ToDamage(float damage)
    {
        health -= damage;

        healthBar.value = health;

        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    private void Death()
    {
        healthBar.gameObject.SetActive(false);
        oldTankHeadCollider.isTrigger = true;
        newTankHeadCollider.isTrigger = false;
        tankHead.isKinematic = false;
        tankHead.constraints = RigidbodyConstraints.None;
        tankHead.AddForce(transform.up, ForceMode.Impulse);
        deathEffect.Play();
        Destroy(transform.gameObject, 10);
    }
}
