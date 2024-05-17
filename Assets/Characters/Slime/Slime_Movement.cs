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

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

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
            animator.Play("Slime_idle");
        }

        // Check for 'D' key press to trigger die animation
        if (Input.GetKeyDown(KeyCode.D))
        {
            Die();
        }
    }

    void MoveSlime()
    {
        rb.velocity = moveDirection * moveSpeed;
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
                animator.Play("slime_jump");
                break;
            case 1:
                moveDirection = Vector2.down; // Move down
                animator.Play("slime_jump");
                break;
            case 2:
                moveDirection = Vector2.left; // Move left
                animator.Play("slime_jump_left");
                break;
            case 3:
                moveDirection = Vector2.right; // Move right
                animator.Play("slime_jump");
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle collision with other objects
        if (collision.gameObject.tag == "Obstacle")
        {
            animator.Play("slime_damage");
            // Implement damage logic
        }
    }

    public void Die()
    {
        animator.Play("slime_dead");
        //logging message 
        Debug.Log("Slime has died");
        // Start a coroutine to wait until the end of the animation
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
}
