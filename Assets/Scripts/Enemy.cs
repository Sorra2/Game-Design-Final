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
    public Rigidbody2D player;
    private Rigidbody2D rb2D;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public int attackDamage = 40;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        attackCycle = 0;
        rb2D = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        attackCycle++;

        if (attackCycle == 300)
        {
            Attack();
            attackCycle = 0;
        }

        if (player.position.x > rb2D.position.x && facing.Equals("left"))
        {
            transform.rotation = Quaternion.identity;
            facing = "right";
        }
        else if (player.position.x < rb2D.position.x && facing.Equals("right"))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            facing = "left";
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

}
