using System.Numerics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    // reference
    private Animator anim;
    private Health playerHealth;
    private Enemy_Patrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<Enemy_Patrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown && PlayerInSight())
        {
            cooldownTimer = 0;
            anim.SetTrigger("meleeAttack"); //trigger attack animation
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight(); //if see the player, stop patrolling
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center
                + transform.right * range * transform.localScale.x * colliderDistance,
            new UnityEngine.Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            ),
            0,
            UnityEngine.Vector2.left,
            0,
            playerLayer
        );

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null; // Placeholder return value
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center
                + transform.right * range * transform.localScale.x * colliderDistance,
            new UnityEngine.Vector3(
                boxCollider.bounds.size.x * range,
                boxCollider.bounds.size.y,
                boxCollider.bounds.size.z
            )
        );
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Player damaged: " + damage);
        }
    }
}
