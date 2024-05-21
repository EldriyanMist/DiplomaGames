using UnityEngine;
using UnityEngine.AI;

public class GoblinAnimation : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        Vector3 movement = transform.position - lastPosition;
        float speed = movement.magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // Set animation parameters based on speed
        if (speed > 0.1f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Determine direction based on NavMeshAgent's velocity
            Vector3 velocity = navMeshAgent.velocity;
            if (velocity.sqrMagnitude > 0.1f)
            {
                if (Mathf.Abs(velocity.z) > Mathf.Abs(velocity.x))
                {
                    if (velocity.z > 0)
                        animator.Play("Top_attack");
                    else
                        animator.Play("Down_attack");
                }
                else
                {
                    if (velocity.x > 0)
                        animator.Play("Right_attack");
                    else
                        animator.Play("Left_attack");
                }
            }
            else
            {
                animator.Play("Right_attack"); // Default attack animation if no direction is detected
            }
        }
    }
}
