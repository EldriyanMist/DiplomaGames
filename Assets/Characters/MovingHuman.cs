using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;

public class MovingHuman : MonoBehaviour
{
    public int max_health = 100;
    public int current_health;

    public Healthbar healthBar;

    public float moveSpeed = 5f;
    public Animator animator;

    private Vector2 movement;
    private float idleTimer = 0f;
    private bool isIdle = false;
    
    public void TakeDamage(int damage)
    {
        current_health -= damage;
        healthBar.SetHealth(current_health);
    }
    
    void Start()
    {
        current_health = max_health;
        healthBar.SetMaxHealth(max_health);
        Debug.Log(healthBar == null ? "HealthBar is not set" : "HealthBar is set");

    }

    private void Update()
    {
        //test health bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
        // Input handling for movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Animation handling based on movement
        if (movement != Vector2.zero)
        {
            animator.SetBool("IsIdle", false); // Not idle anymore
            idleTimer = 0f; // Reset the idle timer

            // Set the appropriate trigger based on the direction of movement
            if (movement.x > 0)
            {
                animator.SetTrigger("WalkingRight");
            }
            else if (movement.x < 0)
            {
                animator.SetTrigger("WalkingLeft");
            }
            else if (movement.y > 0)
            {
                animator.SetTrigger("WalkingTop");
            }
            else if (movement.y < 0)
            {
                animator.SetTrigger("WalkingDown");
            }
        }
        else
        {
            // Handle idle timer
            HandleIdleTimer();
        }


    }

    private void FixedUpdate()
    {
        // Move the character by setting its position to a new position
        transform.position += (Vector3)movement.normalized * moveSpeed * Time.fixedDeltaTime;
    }

    private void HandleIdleTimer()
    {
        if (!isIdle)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= 5f) // You can change this value to whatever you deem appropriate for idle time
            {
                animator.SetBool("IsIdle", true);
                isIdle = true;
            }
        }

        if (movement != Vector2.zero)
        {
            isIdle = false;
            animator.SetBool("IsIdle", false);
        }
    }

}