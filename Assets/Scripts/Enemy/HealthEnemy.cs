using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField] private float Health = 100;

    public void ToDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Destroy(transform.gameObject);
    }
}
