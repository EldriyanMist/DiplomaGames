using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float moveSpeed = 2f;
    private Vector2 moveDirection;
    private bool isMoving = false;
    public GameObject collectableItemPrefab; // Reference to the collectable item prefab
    private Health health;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        // Freeze Z rotation
        rb.freezeRotation = true;

        StartCoroutine(ChangeState());
    }

    void Update()
    {
        if (isMoving)
        {
            MoveSlime();
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("is_moving", false);
        }

        // Check for 'D' key press to trigger die animation
        if (Input.GetKeyDown(KeyCode.D))
        {
            Die();
        }

        // Check for 'H' key press to trigger hit animation
        if (Input.GetKeyDown(KeyCode.H))
        {
            TriggerHit();
        }
    }

    void MoveSlime()
    {
        rb.velocity = moveDirection * moveSpeed;
        animator.SetBool("is_moving", true);
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            isMoving = !isMoving;

            if (isMoving)
            {
                ChangeDirection();
                yield return new WaitForSeconds(2f); // Move for 2 seconds
            }
            else
            {
                animator.SetBool("is_moving", false);
                animator.SetBool("isJumpingLeft", false); // Ensure isJumpingLeft is false when idle
                yield return new WaitForSeconds(2f); // Idle for 2 seconds
            }
        }
    }

    void ChangeDirection()
    {
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0:
                moveDirection = Vector2.up; // Move up
                animator.SetBool("isJumpingLeft", false);
                animator.SetBool("is_moving", true);
                break;
            case 1:
                moveDirection = Vector2.down; // Move down
                animator.SetBool("isJumpingLeft", false);
                animator.SetBool("is_moving", true);
                break;
            case 2:
                moveDirection = Vector2.left; // Move left
                animator.SetBool("isJumpingLeft", true);
                animator.SetBool("is_moving", true);
                break;
            case 3:
                moveDirection = Vector2.right; // Move right
                animator.SetBool("isJumpingLeft", false);
                animator.SetBool("is_moving", true);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with other objects
        if (collision.gameObject.tag == "Obstacle")
        {
            animator.SetBool("Hit", true);
            // Implement damage logic
        }
    }

    public void TriggerHit()
    {
        Debug.Log("TriggerHit called");
        animator.SetBool("Hit", true);
        animator.SetBool("isJumpingLeft", false);
        animator.SetBool("is_moving", false);
        StartCoroutine(ResetHitState());
    }

    IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("Hit", false);
    }

    public void Die()
    {
        animator.SetBool("Hit", true);
        animator.SetBool("Defeated", true);
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        // Wait until the die animation has finished
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Instantiate the collectable item
        Instantiate(collectableItemPrefab, transform.position, Quaternion.identity);

        // Destroy the slime object
        Destroy(gameObject);
    }

    // This method can be called by the NPC to apply damage to the Slime
    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
        TriggerHit();
    }
}
