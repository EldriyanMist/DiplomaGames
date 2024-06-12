using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

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
    private bool isMovingToInteractable = false;

    public NPController npcController;
    public AgentAPI agentAPI;
    public Character character;
    private bool isCharacterReady = false; // Flag to indicate if character is ready
    private bool isSavedOnAPI = false; // Flag to indicate if character ID is saved on API

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
        agentAPI = FindObjectOfType<AgentAPI>();
        character = GetComponent<Character>();

        if (attackHitbox == null)
        {
            Debug.LogError("Attack hitbox collider not found!");
        }

        if (agentAPI == null)
        {
            Debug.LogError("AgentAPI component not found!");
        }

        if (character == null)
        {
            Debug.LogError("Character component not found!");
        }
    }

    private void StartRoutines()
    {
        StartCoroutine(WaitForCharacterReady());
        StartCoroutine(VisibilityCheckRoutine());
    }

    IEnumerator WaitForCharacterReady()
    {
        while (true)
        {
            Debug.Log("Checking if character ID is set...");
            yield return new WaitForSeconds(checkInterval);
            if (character.Id != 0)
            {
                Debug.Log($"Character ID is set: {character.Id}");
                // Start coroutine to check if character ID is saved on the API
                yield return StartCoroutine(CheckCharacterIDOnAPI());
                if (isSavedOnAPI)
                {
                    Debug.Log("Character ID is saved on the API.");
                    isCharacterReady = true;
                    StartCoroutine(IdleAndMoveRoutine());
                    break;
                }
                else
                {
                    Debug.Log("Character ID is not saved on the API yet.");
                }
            }
            else
            {
                Debug.Log("Character ID is still not set.");
            }
        }
    }

    private IEnumerator CheckCharacterIDOnAPI()
    {
        Debug.Log("Checking character ID on the API...");
        string url = $"http://127.0.0.1:8000/agents/{character.Id}";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Character ID check on API success.");
                isSavedOnAPI = true;
            }
            else
            {
                Debug.LogError($"Character ID check failed: {webRequest.error}");
                isSavedOnAPI = false;
            }
        }
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

            if (!isCharacterReady)
            {
                Debug.Log("Character is not ready yet.");
                yield return null;
                continue;
            }

            if (!isMovingToInteractable)
            {
                Transform enemy = FindNearestEnemyWithTag("Slime");

                if (enemy != null)
                {
                    Debug.Log("Enemy found, starting attack routine.");
                    yield return StartCoroutine(MoveAndAttack(enemy));
                }
                else if (visibleDestinations.Count > 0)
                {
                    Debug.Log("No enemy found, getting destination from API.");
                    yield return StartCoroutine(GetDestinationFromAPI());
                }
                else
                {
                    Debug.Log("No visible destinations found.");
                }
            }
        }
    }

private IEnumerator GetDestinationFromAPI()
{
    // Prepare a simple hardcoded list of destinations for testing
    List<string> destinationNames = new List<string> { "home", "river", "slime meat", "lake" };

    // Manually convert the list of destination names to JSON array string
    string jsonData = "[\"" + string.Join("\",\"", destinationNames) + "\"]";
    Debug.Log($"JSON Data being sent: {jsonData}");

    string url = $"http://127.0.0.1:8000/agents/{character.Id}/next_destination";

    Debug.Log("Requesting destination from API...");
    using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
    {
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {webRequest.error}");
            Debug.LogError($"Response: {webRequest.downloadHandler.text}");
        }
        else if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Destination received from API successfully");
            string responseText = webRequest.downloadHandler.text;

            // Check the response
            Debug.Log($"Response Text: {responseText}");

            var responseData = JsonUtility.FromJson<Dictionary<string, string>>(responseText);

            if (responseData.TryGetValue("place", out string destinationName))
            {
                Debug.Log($"Destination received: {destinationName}");
                Transform destination = visibleDestinations.Find(d => d.name == destinationName);
                if (destination != null)
                {
                    Debug.Log($"Moving to destination: {destinationName}");
                    MoveToDestination(destination.position);
                }
                else
                {
                    Debug.LogError($"Destination not found: {destinationName}");
                }
            }
        }
        else
        {
            Debug.LogError($"Unexpected response status: {webRequest.responseCode}");
            Debug.LogError($"Response: {webRequest.downloadHandler.text}");
        }
    }
}

// Wrapper class to properly format the list for JSON serialization
[System.Serializable]
private class StringListWrapper
{
    public List<string> destinations;

    public StringListWrapper(List<string> destinations)
    {
        this.destinations = destinations;
    }
}


    private void MoveToDestination(Vector3 destination)
    {
        Debug.Log($"Setting destination: {destination}");
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
        Debug.Log("Attacking enemy.");
        npcController.Attack_animation();
        SlimeMovement slime = enemy.GetComponent<SlimeMovement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            ItemInteractable item = other.GetComponent<ItemInteractable>();
            if (item != null)
            {
                Debug.Log("Interacting with item.");
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
