using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Controller : MonoBehaviour
{
    private bool isMoving = false;
    public bool IsMoving {
        set {
            isMoving = value;
            if (anim != null)
            {
                anim.SetBool("isMoving", value);
            }
        }
    }
    public float movespeed = 10f;
    public float maxSpeed = 20f;
    public float IdleFriction = 0.9f;

    Vector3 movementInput;
    NavMeshAgent navAgent;

    SpriteRenderer spriteRenderer;
    Animator anim;

    bool canMove = true;

    public SwordAttack swordAttack;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure components are assigned
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;

        // Get input for movement
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        if (movementInput != Vector3.zero)
        {
            Vector3 targetPosition = transform.position + movementInput.normalized * movespeed * Time.deltaTime;
            navAgent.SetDestination(targetPosition);

            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }

        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        UpdateAnimatorParameters();
    }

    void OnFire()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX)
            swordAttack.AttackLeft();
        else
            swordAttack.AttackRight();
    }

    public void EndSwordAttack()
    {
        swordAttack.StopAttack();
        UnlockMovement();
    }

    public void LockMovement()
    {
        canMove = false;
        if (navAgent != null)
        {
            navAgent.isStopped = true;
        }
    }

    public void UnlockMovement()
    {
        canMove = true;
        if (navAgent != null)
        {
            navAgent.isStopped = false;
        }
    }

    public void UpdateAnimatorParameters()
    {
        if (anim != null)
        {
            anim.SetFloat("MoveY", movementInput.y);
            anim.SetBool("MoveX", movementInput.x != 0);
        }
    }
}
