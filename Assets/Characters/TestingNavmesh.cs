using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SimpleNPCMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float moveRadius = 10f; // How far the NPC can move from its current position

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomLocation();
    }

    void MoveToRandomLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        // Try to find a random position on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out hit, moveRadius, 1))
        {
            finalPosition = hit.position;
        }

        // Move the agent to the random position
        agent.SetDestination(finalPosition);

        // Wait for 5 seconds before moving again
        StartCoroutine(WaitAndMove());
    }

    IEnumerator WaitAndMove()
    {
        yield return new WaitForSeconds(5);
        MoveToRandomLocation();
    }
}

