using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyController : MonoBehaviour
{
    //used for bad cogs
    public GameObject badProjectilePrefab;
    Vector2 lookDirection = new Vector2(1, 0);
    bool cogShot = false;

    //timer variables used for spacing own cog shots
    public float timeBeforeAttack = 2.0f;
    float attackTimer;


    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;

    bool broken = true;

    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        //timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 3f, LayerMask.GetMask("Character"));

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if (!broken)
        {
            return;
        }

        if (cogShot == false)
        {
            BadLaunch();
            Debug.Log("Detecting Player");
        }

        if (cogShot == true)
        {
            //attackTimer -= Time.deltaTime;
            //if (attackTimer < 0)
                //cogShot = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);

        if (!broken)
        {
            return;
        }
    }

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-2);
        }
    }

    void BadLaunch()
    {
        GameObject projectileObject = Instantiate(badProjectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        BadProjectile badprojectile = projectileObject.GetComponent<BadProjectile>();
        badprojectile.BadLaunch(lookDirection, 300);
    }

}
