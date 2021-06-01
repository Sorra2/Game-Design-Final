using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject hatch;
    public GameObject additional1;
    public GameObject additional2;
    public GameObject additional3;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(hatch);
        if(additional1 != null)
        {
            Destroy(additional1);
        }

        if(additional2 != null)
        {
            Destroy(additional2);
        }

        if(additional3 != null)
        {
            Destroy(additional3);
        }
    }
}
