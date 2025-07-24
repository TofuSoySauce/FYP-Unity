using System.Numerics;
using UnityEngine;

public class Enemy_Patrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField]
    private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField]
    private float speed;
    private UnityEngine.Vector3 initialScale;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField]
    private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField]
    private Animator anim;

    private void Awake()
    {
        initialScale = enemy.localScale;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1); // Move left
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1); // Move right
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false); //no more animation when the object disabled or destroyed (e.g. when the enemy is dead)
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);

        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
        }
    }

    private void MoveInDirection(int direction)
    {
        idleTimer = 0; // Reset idle timer
        anim.SetBool("moving", true);

        //Make enemy face the direction of movement
        enemy.localScale = new UnityEngine.Vector3(
            Mathf.Abs(initialScale.x) * direction,
            initialScale.y,
            initialScale.z
        );

        //Move in the specified direction
        enemy.position = new UnityEngine.Vector3(
            enemy.position.x + Time.deltaTime * direction * speed,
            enemy.position.y,
            enemy.position.z
        );
    }
}
