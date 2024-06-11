using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public float checkInterval = 2f;
    public LayerMask obstacleLayer;
    public float attackRange = 1.5f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private List<Transform> visibleDestinations = new List<Transform>();
    private float idleTime = 3.5f;
    private Collider2D attackHitbox;
    private NPC npc;
    private bool isMovingToInteractable = false;
    private ScriptedBehaviors scriptedBehaviors;

    void Start()
    {
        InitializeComponents();
        StartRoutines();
    }

    private void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        npc = GetComponent<NPC>();
        attackHitbox = GetComponentInChildren<Collider2D>();
        scriptedBehaviors = GetComponent<ScriptedBehaviors>();

        if (attackHitbox == null)
        {
            Debug.LogError("Attack hitbox collider not found!");
        }

        if (scriptedBehaviors == null)
        {
            Debug.LogError("ScriptedBehaviors component not found!");
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

        // Debug log to show visible destinations
        Debug.Log("Visible Destinations:");
        foreach (Transform dest in visibleDestinations)
        {
            Debug.Log(dest.name);
        }

        // Initialize scripted behaviors with the updated visible destinations
        if (scriptedBehaviors != null)
        {
            scriptedBehaviors.Initialize(visibleDestinations);
        }
    }

    private bool IsDestinationVisible(Transform destination)
    {
        Vector3 directionToDestination = destination.position - transform.position;

        // Assuming no obstacles for now
        return true;
    }

    IEnumerator IdleAndMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
            if (!isMovingToInteractable)
            {
                Transform enemy = FindNearestEnemyWithTag("Slime");

                if (enemy != null)
                {
                    yield return StartCoroutine(MoveAndAttack(enemy));
                }
                else
                {
                    // Test scripted behavior by name
                    if (scriptedBehaviors != null)
                    {
                        scriptedBehaviors.ExecuteScriptedMovement("MoveToFireplace");
                    }
                }
            }
        }
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
        // Implement your attack logic here
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            ItemInteractable item = other.GetComponent<ItemInteractable>();
            if (item != null)
            {
                isMovingToInteractable = true;
                StopAllCoroutines(); // Stop other routines while interacting
                agent.SetDestination(transform.position); // Stop movement
                item.InteractWithNPC(GetComponent<NPC>());
                StartCoroutine(HandleInteraction(item.InteractionDuration));
            }
        }
    }

    private IEnumerator HandleInteraction(float duration)
    {
        yield return new WaitForSeconds(duration);
        isMovingToInteractable = false;
        StartRoutines(); // Resume routines after interaction
    }

    private void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private Transform ChooseRandomVisibleDestinationByName()
    {
        if (visibleDestinations.Count == 0) return null;
        int randomIndex = Random.Range(0, visibleDestinations.Count);
        return visibleDestinations[randomIndex];
    }

    void OnDrawGizmos()
    {
        // Draw gizmos for debug purposes
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 100f); // Arbitrary large radius for full map vision
    }
}
