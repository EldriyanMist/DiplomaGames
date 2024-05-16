using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float visionRadius = 10f;
    public float checkInterval = 2f; // How often to check for visible destinations
    public LayerMask obstacleLayer; // Set this in the Inspector to match your environment's obstacles
    public GameObject activitySymbol; // The symbol that appears over the NPC
    public string activityDestinationTag = "Fireplace"; // Tag to trigger the activity symbol

    private NavMeshAgent agent;
    private List<Transform> visibleDestinations = new List<Transform>();
    private float idleTime = 3.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        activitySymbol.SetActive(false);  // Ensure the symbol is initially hidden
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
    Debug.Log("Updating visible destinations...");

    foreach (GameObject destination in GameObject.FindGameObjectsWithTag("Destination"))
    {
        if (IsDestinationVisible(destination.transform))
        {
            visibleDestinations.Add(destination.transform);
            Debug.Log($"Visible Destination Added: {destination.name}");
        }
    }

    // If you want to print all visible destinations after the update
    if (visibleDestinations.Count > 0)
    {
        Debug.Log("Currently visible destinations:");
        foreach (var dest in visibleDestinations)
        {
            Debug.Log(dest.name);
        }
    }
    else
    {
        Debug.Log("No visible destinations currently.");
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

            if (visibleDestinations.Count > 0)
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
        Transform destinationTransform = visibleDestinations[randomIndex];

        if (destinationTransform.CompareTag(activityDestinationTag))
    {
        activitySymbol.SetActive(true); // Show the symbol
    }
    else
    {
        activitySymbol.SetActive(false); // Hide the symbol
    }

    return destinationTransform;
    }

    void MoveToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow; // Set the color of the gizmo
        Gizmos.DrawWireSphere(transform.position, visionRadius); // Draw a wire sphere representing the vision radius
        }
}



