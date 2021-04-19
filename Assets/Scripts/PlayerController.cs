using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public variables allow for easy testing, can be set to private once we find the sweet spot
    public float speed;
    public float dashValue;
    public float jumpValue;
    public GameObject character;
    public LayerMask groundLayer;
    //character always starts out facing right, this is used for dash and attack
    private static string facing = "right";


    void Update()
    {
        //each of these are called every frame, can be changed if we want a delay
        Jump();
        Run();
        Dash();

        //this is just basic movement code
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        movement.Normalize();
        transform.position += movement * Time.deltaTime * speed;

        //using the horizontal axis to determine which direction the character is facing, can also be useful for animations when we get there
        if(Input.GetAxis("Horizontal") < 0)
        {
            facing = "left";
        }
        else if(Input.GetAxis("Horizontal") > 0)
        {
            facing = "right";
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
            return true;
        }

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
}
