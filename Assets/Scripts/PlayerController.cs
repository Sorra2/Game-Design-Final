using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject character;
    public LayerMask groundLayer;


    void Update()
    {
        Jump();
        Run();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        movement.Normalize();
        transform.position += movement * Time.deltaTime * speed;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Grounded())
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 100f), ForceMode2D.Impulse);
        }
        
    }

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
}
