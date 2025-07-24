using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the player collides with the enemy, deal damage
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
