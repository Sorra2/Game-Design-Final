using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables for jump, normal speed, and dash distance
    public float speed;
    public float dashValue;
    public float jumpValue;
    public LayerMask groundLayer;

    //basic required objects
    public GameObject character;
    public Animator animator;

    //variables for attack function
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;


    //character always starts out facing right, this is used for dash and attack
    private static string facing = "right";


    void Update()
    {
        //each of these are called every frame, can be changed if we want a delay
        Jump();
        Run();
        Dash();
        Attack();

        //this is just basic movement code
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0f, 0f);
        movement.Normalize();

        if (horizontal > 0)
        {
            animator.SetInteger("AnimState", 1);
        }
        else if(horizontal < 0)
        {
            animator.SetInteger("AnimState", 1);
        }
        else if (horizontal == 0)
        {
            animator.SetInteger("AnimState", 0);
        }

        transform.position += movement * Time.deltaTime * speed;

        //using the horizontal axis to determine which direction the character is facing, can also be useful for animations when we get there
        if(Input.GetAxis("Horizontal") < 0)
        {
            facing = "left";
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(Input.GetAxis("Horizontal") > 0)
        {
            facing = "right";
            transform.rotation = Quaternion.identity;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Grounded())
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpValue), ForceMode2D.Impulse);
        }
        
    }

    //Grounded() makes sure the character is on a block in the "ground" layer
    bool Grounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            animator.SetBool("Grounded", true);
            return true;
        }
        animator.SetBool("Grounded", false);
        return false;
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            speed += 10;
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            speed -= 10;
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
}
