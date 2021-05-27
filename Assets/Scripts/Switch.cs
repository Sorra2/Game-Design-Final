using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject hatch;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(hatch);
    }
}
