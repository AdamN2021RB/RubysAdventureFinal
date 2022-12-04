using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogCollectable : MonoBehaviour
{
    public AudioClip collectedClip;

    public ParticleSystem cogRestore;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.cogCount < controller.maxHealth)
            {

                //controller.ChangeCogs(1);
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
                controller.PlayEffect(cogRestore);

            }
        }
    }
}
