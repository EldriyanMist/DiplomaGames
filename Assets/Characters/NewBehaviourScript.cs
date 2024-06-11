using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBehaviors : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private RandomMovement randomMovement;
    private InventoryPlayer playerInventory;
    private InventoryChest chest;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        randomMovement = GetComponent<RandomMovement>();
        playerInventory = GetComponent<InventoryPlayer>();
        chest = FindObjectOfType<InventoryChest>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
        }

        if (randomMovement == null)
        {
            Debug.LogError("RandomMovement component not found!");
        }

        if (playerInventory == null)
        {
            Debug.LogError("InventoryPlayer component not found!");
        }

        if (chest == null)
        {
            Debug.LogError("InventoryChest component not found!");
        }
    }

    public void ExecuteScriptedMovement(string behaviorName)
    {
        switch (behaviorName)
        {
            case "Patrol":
                StartCoroutine(PatrolBehavior());
                break;
            case "Guard":
                StartCoroutine(GuardBehavior());
                break;
            case "StoreInChest":
                StartCoroutine(StoreInChestBehavior());
                break;
            // Add more cases for other behaviors as needed
            default:
                Debug.LogWarning($"Behavior {behaviorName} not recognized.");
                break;
        }
    }

    private IEnumerator PatrolBehavior()
    {
        List<string> patrolPoints = new List<string> { "Fireplace02", "Tree01", "Statue" };

        while (true)
        {
            foreach (string pointName in patrolPoints)
            {
                Transform destination = FindVisibleDestinationByName(pointName);
                if (destination != null)
                {
                    agent.SetDestination(destination.position);
                    yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
                    yield return new WaitForSeconds(7f); // Wait at the patrol point for 7 seconds

                    // Wait if the progress bar is running
                    while (randomMovement.isProgressBarRunning)
                    {
                        yield return null;
                    }
                }
            }
        }
    }

    private IEnumerator GuardBehavior()
    {
        Transform guardPoint = FindVisibleDestinationByName("Fireplace");

        if (guardPoint != null)
        {
            while (true)
            {
                agent.SetDestination(guardPoint.position);
                yield return new WaitUntil(() => agent.remainingDistance < 0.1f);
                yield return new WaitForSeconds(7f); // Stay at the guard point for 7 seconds

                // Wait if the progress bar is running
                while (randomMovement.isProgressBarRunning)
                {
                    yield return null;
                }
            }
        }
    }

    private IEnumerator StoreInChestBehavior()
    {
        // Move to the chest's location
        Transform chestTransform = chest.transform;
        agent.SetDestination(chestTransform.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.1f);

        // Open the chest
        Chest chestScript = chest.GetComponent<Chest>();
        chestScript.ToggleChest();
        yield return new WaitForSeconds(1f); // Wait a moment to ensure the chest is open

        // Store the item in the chest
        if (playerInventory.PutItemInChest(chest))
        {
            Debug.Log("Item stored in chest successfully.");
        }
        else
        {
            Debug.Log("Failed to store item in chest.");
        }

        // Close the chest
        chestScript.ToggleChest();
    }

    private Transform FindVisibleDestinationByName(string name)
    {
        foreach (GameObject destination in GameObject.FindGameObjectsWithTag("Interactable"))
        {
            if (destination.name == name)
            {
                return destination.transform;
            }
        }
        return null;
    }
}
