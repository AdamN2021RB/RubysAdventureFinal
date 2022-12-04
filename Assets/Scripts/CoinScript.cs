using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public AudioClip collectedClip;

    public ParticleSystem healthRestore;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.coins < 8)
            {

                controller.coins++;
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
                controller.PlayEffect(healthRestore);

            }
        }
    }
}
