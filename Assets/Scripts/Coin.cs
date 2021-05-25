using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{

    public Text visualCounter;
    public CapsuleCollider2D coin;
    public BoxCollider2D player;
    private static int coinCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        visualCounter.text = "   x" + coinCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (coin.IsTouching(player))
        {
            GetCoin();
        }
    }

    void GetCoin()
    {
        coinCount++;

        if (coinCount < 100)
        {
        visualCounter.text = "   x" + coinCount;
        }
        else
        {
            visualCounter.text = "   x" + 0;
            coinCount = 0;
        }
        Destroy(gameObject);
    }

}
