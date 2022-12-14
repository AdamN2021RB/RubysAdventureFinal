using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();

        if (e != null)
        {
            e.Fix();
            RubyController.robotfix += 1;
        }

        SuperEnemyController d = other.collider.GetComponent<SuperEnemyController>();
        if (d != null)
        {
            d.Fix();
            RubyController.robotfix += 1;
        }

        MunchkinController c = other.collider.GetComponent<MunchkinController>();
        if (c != null)
        {
            c.Fix();
        }

        Destroy(gameObject);
    }
}
