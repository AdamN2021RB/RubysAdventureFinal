using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunchkinController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 2.5f;

    bool angry = false;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    Animator animator;

 

    AudioSource voice;
    public AudioClip rage;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        voice = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        //makes the little guy move
        Vector2 position = rigidbody2D.position;

        //decides if the muchkin is moving sideways or up and down
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            //animator.SetFloat("Move X", 0);
            //animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            //animator.SetFloat("Move X", direction);
            //animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    public void Fix()
    {
        angry = true;
        voice.PlayOneShot(rage);
        speed = 6;
        animator.SetTrigger("Angry");
        
        //rigidbody2D.simulated = false;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (angry == true)
        {
            if (player != null)
        {
            player.ChangeHealth(-1);
        }
        }
    }
}
