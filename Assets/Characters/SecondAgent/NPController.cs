using UnityEngine;
using UnityEngine.AI;

public class NPController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        // Get the Animator and NavMeshAgent components attached to the GameObject
        animator = GetComponent<Animator>();

        // Initialize lastPosition
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement direction
        Vector3 movement = transform.position - lastPosition;
        lastPosition = transform.position;

        float moveX = movement.x;
        float moveY = movement.z; // Assuming a top-down view where Y-axis is forward
        bool isMoving = movement.magnitude > 0.01f;

        // Update animator parameters
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetBool("IsMoving", isMoving);

        // Check if the agent is attacking (example condition)
    }

    public void Attack_animation()
    {
        // Play attack animation
        animator.SetTrigger("Attack");
    }
}
