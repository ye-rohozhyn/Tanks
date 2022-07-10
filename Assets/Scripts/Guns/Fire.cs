using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private float damage = 0.1f;

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<TankHealth>().ToDamage(damage);
        }
    }
}
