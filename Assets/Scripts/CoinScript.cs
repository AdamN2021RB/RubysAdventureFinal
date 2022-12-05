using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.coins < 8)
            {

                controller.ChangeCoin(1);

                Destroy(gameObject);

                controller.PlaySound(collectedClip);

            }
        }
    }
}
