using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;
    private bool isAttacking = false;  // To track if the NPC is currently attacking
    public float attackCooldown = 1f;  // Cooldown period between attacks

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    public void UpdateMovementAnimation(Vector3 velocity)
    {
        if (velocity.magnitude > 0.1f)
        {
            animator.SetFloat("MoveX", velocity.x);
            animator.SetFloat("MoveY", velocity.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    public void TriggerAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        // Wait for the duration of the attack animation before allowing another attack
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length + attackCooldown);
        isAttacking = false;
    }
}
