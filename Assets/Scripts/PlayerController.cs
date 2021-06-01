using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //variables for jump and normal speed
    public float speed;
    public float jumpValue;
    public LayerMask groundLayer;
    
    //dash variables
    public float dashValue;
    public static int dashCharge = 0;
    public Text dashBar;

    //basic required objects
    public GameObject character;
    public Animator animator;
    private Rigidbody2D rb2D;

    //variables for attack function
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private int attackDamage = 3;

    //lives
    public Text lifeCounter;
    public static int lives = 3;


    //character always starts out facing right, this is used for dash and attack
    private static string facing = "right";

    //health values
    private int maxHealth = 3;
    private int currentHealth;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;


    void Start()
    {
        heart1.transform.GetComponent<SpriteRenderer>().enabled = true;
        heart2.transform.GetComponent<SpriteRenderer>().enabled = true;
        heart3.transform.GetComponent<SpriteRenderer>().enabled = true;
        transform.position = new Vector2(-8.2f, 2f);
        lifeCounter.text = "   x" + lives;
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;//need to change so player can move again
        GetComponent<Collider2D>().enabled = true;
        enabled = true;
        currentHealth = maxHealth;
        rb2D = transform.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //each of these are called every frame, can be changed if we want a delay
        Jump();
        Run();
        Debug.Log(currentHealth);
        
        if(dashCharge < 500)
        {
            dashCharge++;
            dashBar.text = "Dash: " + (dashCharge/5);
        }
        else
        {
            if(dashCharge == 500)
            {
                dashBar.text = "Ready!";
            }
            Dash();
        }
        Attack();

        //this is just basic movement code
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0f, 0f);
        movement.Normalize();

        //animations
        Animate(horizontal);

        //set movement for player
        transform.position += movement * Time.deltaTime * speed;

        //get direction player is facing
        facing = Facing(horizontal);

    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Grounded())
        {
            rb2D.velocity = Vector2.up * jumpValue;
        }
        
    }

    //Grounded() makes sure the character is on a block in the "ground" layer
    bool Grounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.25f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            speed *= 2;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            speed /= 2;
        }
    }

    void Dash()
    {
        //tempDash needed to prevent dashValue from changing
        float tempDash = dashValue;
        if (facing.Equals("left"))
        {
            tempDash *= -1;//multiply by -1 to make dash go to left
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(tempDash, 0f), ForceMode2D.Impulse);
            dashCharge = 0;
        }
    }

    string Facing(float horizontal)
    {
        //using the horizontal axis to determine which direction the character is facing, also useful for animations
        if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            return "left";
        }
        else if (horizontal > 0)
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

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        { 
            animator.SetTrigger("Attack1");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        //play damage anim
        currentHealth -= damage;

        animator.SetTrigger("Hurt");
        if(currentHealth == 2)
        {
            heart1.transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(currentHealth == 1)
        {
            heart2.transform.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            heart3.transform.GetComponent<SpriteRenderer>().enabled = false;
        }


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //die anim
        animator.SetTrigger("Death");

        //disable player
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;//need to change so that player doesn't fall off screen
        GetComponent<Collider2D>().enabled = false;
        enabled = false;

        if(lives >= 0)
        {
            Start();
            lives--;
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
        else if (horizontal == 0)
        {
            animator.SetInteger("AnimState", 0);
        }
    }
}
