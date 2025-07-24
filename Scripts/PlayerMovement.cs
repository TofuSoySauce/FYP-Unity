using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    [SerializeField]
    private float fallMultiplier = 2.5f;

    [SerializeField]
    private float lowJumpMultiplier = 2f;

    private bool pushingRock;

    private void Awake()
    {
        //Get the components from the player object
        //This is done in awake to ensure that the components are available before any other script tries to access them
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float currentSpeed = pushingRock ? speed / 2 : speed; //if pushing a rock, move at half speed

        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new UnityEngine.Vector2(
            horizontalInput * currentSpeed,
            body.linearVelocity.y
        );

        //Flip the player sprite based on movement direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = UnityEngine.Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }

        // Better jump logic
        if (body.linearVelocity.y < 0)
        {
            body.linearVelocity +=
                UnityEngine.Vector2.up
                * Physics2D.gravity.y
                * (fallMultiplier - 1)
                * Time.deltaTime;
        }
        else if (body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity +=
                UnityEngine.Vector2.up
                * Physics2D.gravity.y
                * (lowJumpMultiplier - 1)
                * Time.deltaTime;
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.linearVelocity = new UnityEngine.Vector2(body.linearVelocity.x, speed);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool landedOnGround = false;
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Rock"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // means contact came from below, i.e. player landed on top
                {
                    landedOnGround = true;
                    break; // no need to check more contacts
                }
            }
        }
        if (landedOnGround)
        {
            grounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Math.Abs(contact.normal.x) > 0.5f) // means contact is on the side, i.e. player is pushing the rock
                {
                    pushingRock = true;
                    return; // no need to check more contacts
                }
            }
        }
        pushingRock = false; // if we didn't find a side contact, we are not pushing the rock
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rock"))
        {
            pushingRock = false;
        }
    }
}
