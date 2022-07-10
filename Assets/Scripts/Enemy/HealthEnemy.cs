using UnityEngine;
using UnityEngine.UI;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform playerCamera;

    private void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    private void FixedUpdate()
    {
        healthBar.transform.LookAt(playerCamera.position);
    }

    public void ToDamage(float damage)
    {
        health -= damage;

        healthBar.value = health;

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(transform.gameObject);
    }
}
