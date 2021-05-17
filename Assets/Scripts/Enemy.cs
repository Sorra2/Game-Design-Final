using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    public float speed = 3;

    private static int attackCycle;
    private static string facing = "left";
    private Rigidbody2D rb2D;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 40;

    private static float waypoint1 = -5;
    private static float waypoint2 = -1;
    private static float destination = waypoint1;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        attackCycle = 0;
        rb2D = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 movement;
        attackCycle++;

        if(rb2D.position.x == destination && destination == waypoint1)
        {
            destination = waypoint2;
        }
        else if(rb2D.position.x == destination && destination == waypoint2)
        {
            destination = waypoint1;
        }

        if (waypoint1 == destination && rb2D.position.x != waypoint1)
        {
            rb2D.velocity = new Vector2(-10f, 0f);
        }
        else if(waypoint2 == destination && rb2D.position.x != waypoint2)
        {
            rb2D.velocity = new Vector2(10f, 0f);
        }


    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play damage anim
        animator.SetTrigger("Hurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //die anim
        animator.SetTrigger("Death");

        //disable enemy
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;//need to change so that enemy doesn't fall off screen
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

    }

    void Attack()
    {
        animator.SetTrigger("Attack1");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    string Facing(float horizontal)
    {
        //using the horizontal axis to determine which direction the character is facing, can also be useful for animations when we get there
        if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            return "left";
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.identity;
            return "right";
        }
        //last block is for if the player isn't moving
        else
        {
            return facing;
        }
    }

    void Animate(float horizontal)
    {
        //first 2 blocks for walking, last is for idle
        if (horizontal > 0)
        {
            animator.SetInteger("AnimState", 1);
        }
        else if (horizontal < 0)
        {
            animator.SetInteger("AnimState", 1);
        }
    }
}
