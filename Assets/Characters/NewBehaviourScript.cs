using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBehaviors : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private RandomMovement randomMovement;
    public InventoryPlayer playerInventory;
    public InventoryChest chest;
    public float interactionDistance = 2f; // Distance threshold for interacting with the chest
    public float detectionRange = 10f; // Range within which NPCs will be detected

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        randomMovement = GetComponent<RandomMovement>();
        playerInventory = GetComponent<InventoryPlayer>();

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
            Debug.LogError("InventoryChest component is not assigned!");
        }
    }

    void Update()
    {
        DetectAndMoveToNPCs();
    }

    public void ExecuteScriptedMovement(string behaviorName)
    {
        Debug.Log($"Executing scripted movement: {behaviorName}");
        switch (behaviorName)
        {
            case "Patrol":
                StartCoroutine(PatrolBehavior());
                break;
            case "Guard":
                StartCoroutine(GuardBehavior());
                break;
            case "CollectAndStore":
                StartCoroutine(CollectAndStoreBehavior());
                break;
            // Add more cases for other behaviors as needed
            default:
                Debug.LogWarning($"Behavior {behaviorName} not recognized.");
                break;
        }
    }

    private IEnumerator PatrolBehavior()
    {
        Debug.Log("Starting PatrolBehavior");
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
        Debug.Log("Starting GuardBehavior");
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

    private IEnumerator CollectAndStoreBehavior()
    {
        Debug.Log("Starting CollectAndStoreBehavior");
        // Collect one collectable item at a time
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");

        foreach (GameObject collectable in collectables)
        {
            if (collectable != null)
            {
                Debug.Log($"Moving to collectable: {collectable.name}");
                agent.SetDestination(collectable.transform.position);
                yield return new WaitUntil(() => agent.remainingDistance < 0.1f);

                // Wait if the progress bar is running
                while (randomMovement.isProgressBarRunning)
                {
                    yield return null;
                }

                // Collect the item
                Collectable collectableScript = collectable.GetComponent<Collectable>();
                if (collectableScript != null && playerInventory.AddItem(collectableScript.item))
                {
                    Debug.Log($"Collected item: {collectableScript.item.itemName}");
                    Destroy(collectable);

                    // Move to the chest and store the item
                    GameObject chestObject = GameObject.FindGameObjectWithTag("Chest");
                    if (chestObject != null)
                    {
                        Transform chestTransform = chestObject.transform;
                        agent.SetDestination(chestTransform.position);
                        yield return new WaitUntil(() => agent.remainingDistance < interactionDistance);

                        // Store the item in the chest
                        Debug.Log("Storing item in chest");
                        playerInventory.PutItemInChest(chest);
                    }
                    else
                    {
                        Debug.LogError("No GameObject with tag 'Chest' found!");
                    }
                }
            }
        }

        // Take one item from the chest after collecting all collectables
        if (playerInventory.GetCurrentItem() == null)
        {
            GameObject chestObject = GameObject.FindGameObjectWithTag("Chest");
            if (chestObject != null)
            {
                Transform chestTransform = chestObject.transform;
                agent.SetDestination(chestTransform.position);
                yield return new WaitUntil(() => agent.remainingDistance < interactionDistance);

                yield return new WaitForSeconds(1f);

                Debug.Log("Taking item from chest");
                playerInventory.TakeItemFromChest(chest, 0); // Assuming we're taking the first item in the chest
            }
            else
            {
                Debug.LogError("No GameObject with tag 'Chest' found!");
            }
        }
    }

    private void DetectAndMoveToNPCs()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in npcs)
        {
            if (npc != this.gameObject) // Avoid detecting itself
            {
                float distance = Vector3.Distance(transform.position, npc.transform.position);
                if (distance <= detectionRange)
                {
                    MoveToNPC(npc.transform);
                    break;
                }
            }
        }
    }

    private void MoveToNPC(Transform npcTransform)
    {
        Debug.Log($"Moving towards NPC: {npcTransform.name}");
        agent.SetDestination(npcTransform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            Collectable collectable = other.GetComponent<Collectable>();
            if (collectable != null)
            {
                bool wasCollected = CollectItem(collectable.item);
                if (wasCollected)
                {
                    Destroy(other.gameObject);
                }
            }
        }
        else if (other.CompareTag("NPC"))
        {
            Debug.Log($"Collided with NPC: {other.name}");
            // Add your logic for what happens when NPCs collide
        }
    }

    public bool CollectItem(Item item)
    {
        return playerInventory.AddItem(item);
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
