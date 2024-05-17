using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float visionRadius = 10f;
    public float checkInterval = 2f; // How often to check for visible destinations
    public LayerMask obstacleLayer; // Set this in the Inspector to match your environment's obstacles
    public float attackRange = 1.5f; // Adjust as needed

    private NavMeshAgent agent;
    private List<Transform> visibleDestinations = new List<Transform>();
    private float idleTime = 3.5f;

    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(VisibilityCheckRoutine());
        StartCoroutine(IdleAndMoveRoutine());
    }

    IEnumerator VisibilityCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            UpdateVisibleDestinations();
        }
    }

    void UpdateVisibleDestinations()
    {
        visibleDestinations.Clear();

        foreach (GameObject destination in GameObject.FindGameObjectsWithTag("Destination"))
        {
            if (IsDestinationVisible(destination.transform))
            {
                visibleDestinations.Add(destination.transform);
            }
        }

    }

    bool IsDestinationVisible(Transform destination)
    {
        Vector3 directionToDestination = destination.position - transform.position;
        float distanceToDestination = Vector3.Distance(transform.position, destination.position);

        if (distanceToDestination <= visionRadius)
        {
            if (!Physics.Raycast(transform.position, directionToDestination, distanceToDestination, obstacleLayer))
            {
                // No obstacle in the way
                return true;
            }
        }

        return false; // Destination is not visible
    }

    IEnumerator IdleAndMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);

            // Check for the nearest enemy with the "Slime" tag
            Transform enemy = FindNearestEnemyWithTag("Slime");
            if (enemy != null)
            {
                yield return StartCoroutine(MoveAndAttack(enemy));
            }
            else if (visibleDestinations.Count > 0)
            {
                Transform destination = ChooseRandomDestination();
                if (destination != null)
                {
                    MoveToDestination(destination.position);
                }
            }
        }
    }

    Transform ChooseRandomDestination()
    {
        if (visibleDestinations.Count == 0) return null;
        int randomIndex = Random.Range(0, visibleDestinations.Count);
        return visibleDestinations[randomIndex];
    }

    void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    Transform FindNearestEnemyWithTag(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    IEnumerator MoveAndAttack(Transform enemy)
    {
        while (Vector3.Distance(transform.position, enemy.position) > attackRange)
        {
            MoveToDestination(enemy.position);
            yield return null; // Wait for the next frame
        }

        // Attack logic (e.g., reduce enemy health)
        Attack();
    }


    void Attack()
    {
        animator.SetTrigger("Attack");
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Set the color of the gizmo
        Gizmos.DrawWireSphere(transform.position, visionRadius); // Draw a wire sphere representing the vision radius
    }
}
