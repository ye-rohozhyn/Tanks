using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private float damage = 50f;

    [Header("Explosion")]
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private string[] ignoreTags;

    private void Explode()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in overlappedColliders)
        {
            Rigidbody colliderRigidbody = collider.attachedRigidbody;
            if (colliderRigidbody)
            {
                colliderRigidbody.AddExplosionForce(force, transform.position, radius, 1f);
            }
        }

        if (explosionEffect.isStopped)
        { 
            explosionEffect.transform.position = transform.position;
            explosionEffect.Play();
        }

        transform.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, radius / 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        string collisionTag = collision.transform.tag;

        foreach (string tag in ignoreTags)
        {
            if (collisionTag == tag) return;
        }

        if (collisionTag == "Enemy") collision.transform.GetComponent<HealthEnemy>().ToDamage(damage);
        Explode();
    }
}
