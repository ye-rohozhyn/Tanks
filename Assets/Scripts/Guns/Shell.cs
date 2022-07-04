using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private float damage = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<HealthEnemy>().ToDamage(damage);
            transform.gameObject.SetActive(false);
        }
        else if (collision.transform.tag != "Player")
        {
            transform.gameObject.SetActive(false);
        }
    }
}
