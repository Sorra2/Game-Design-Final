using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    
    public int maxHealth = 100;
    int currentHealth;
    public float knockback;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //play damage anim
        animator.SetTrigger("Hurt");
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, 0f), ForceMode2D.Impulse);
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
        //GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
