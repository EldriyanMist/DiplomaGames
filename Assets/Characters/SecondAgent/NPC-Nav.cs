using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float visionRadius = 10f;
    public float checkInterval = 2f;
    public LayerMask obstacleLayer;
    public float attackRange = 1.5f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private List<Transform> visibleDestinations = new List<Transform>();
    private float idleTime = 3.5f;
    private Collider2D attackHitbox;

    public NPController npcController;

    void Start()
    {
        InitializeComponents();
        StartRoutines();
    }

    private void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        npcController = GetComponent<NPController>();
        attackHitbox = GetComponentInChildren<Collider2D>();
        
        if (attackHitbox == null)
        {
            Debug.LogError("Attack hitbox collider not found!");
        }
    }

    private void StartRoutines()
    {
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

    private void UpdateVisibleDestinations()
    {
        visibleDestinations.Clear();

        foreach (GameObject destination in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            if (IsDestinationVisible(destination.transform))
            {
                visibleDestinations.Add(destination.transform);
            }
        }
    }

    private bool IsDestinationVisible(Transform destination)
    {
        Vector3 directionToDestination = destination.position - transform.position;
        float distanceToDestination = Vector3.Distance(transform.position, destination.position);

        if (distanceToDestination <= visionRadius && 
            !Physics2D.Raycast(transform.position, directionToDestination, distanceToDestination, obstacleLayer))
        {
            return true;
        }

        return false;
    }

    IEnumerator IdleAndMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
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

    private Transform ChooseRandomDestination()
    {
        if (visibleDestinations.Count == 0) return null;
        int randomIndex = Random.Range(0, visibleDestinations.Count);
        return visibleDestinations[randomIndex];
    }

    private void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private Transform FindNearestEnemyWithTag(string tag)
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
            yield return null;
        }

        Attack(enemy);
    }

    private void Attack(Transform enemy)
    {
        npcController.Attack_animation();
        SlimeMovement slime = enemy.GetComponent<SlimeMovement>();
    }

        void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.CompareTag("Slime"))
        {
            SlimeMovement slime = other.GetComponent<SlimeMovement>();
        }
        else */
        if (other.CompareTag("Interactable")){
        ItemInteractable item = other.GetComponent<ItemInteractable>();
        if (item != null)
        {
            item.InteractWithNPC(GetComponent<NPC>());
        }
    }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}



