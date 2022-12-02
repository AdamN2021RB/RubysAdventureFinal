using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    //uses cog prefab
    public GameObject projectilePrefab;

    public int health { get { return currentHealth; } }
    int currentHealth;

    //timer for invincibility after taking damage
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    //where the sprite looks
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;
    public ParticleSystem particle;
    public ParticleSystem dmgEffect;

    //sound files
    public AudioClip cogShot;
    public AudioClip mainCharacterAbuse;
    public AudioClip trueEnding;
    public AudioClip loseEnding;

    //determines either wins or losses
    public GameObject winTextObject;
    public GameObject loseTextObject;


    //used for tracking score
    public static int robotfix = 0;
    public static bool stageTwo;
    public int count;
    public TextMeshProUGUI countNumbers;

    //checks for clear message for each stage
    public static int level;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        robotfix = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        count = robotfix;
        RobotCount();

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
            audioSource.PlayOneShot(cogShot);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                if (robotfix == 6)
                {
                    SceneManager.LoadScene("Stage2");
                    //winTextObject.SetActive(false);
                    stageTwo = true;
                }
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))

        {

            if (loseTextObject == true)

            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene

            }

        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        //if statement below deals with damage
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;


            if (currentHealth > 0)
            {
                Instantiate(dmgEffect, rigidbody2d.position + Vector2.up * 0f, Quaternion.identity);

                PlaySound(mainCharacterAbuse);



            }
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0)
        {
            //Destroy(this);
            loseTextObject.SetActive(true);
            PlaySound(loseEnding);

        }

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    public void PlayEffect(ParticleSystem effect)
    {
        //particle.Play(effect);
        Instantiate(effect, rigidbody2d.position + Vector2.up * 0f, Quaternion.identity);
    }

    void RobotCount()
    {
        //Projectile controller = other.GetComponent<Projectile>();
        countNumbers.text = "Robots Fixed: " + robotfix;

        //displays win text
        if (robotfix == 6)
        {
            //audioSource.PlayOneShot(trueEnding);
            stageTwo = true;
            winTextObject.SetActive(true);

        }

        if (robotfix == 5 && speed != 0.0f && stageTwo == true)
        {
            PlaySound(trueEnding);

            winTextObject.SetActive(true);


            speed = 0.0f;
        }
    }
}