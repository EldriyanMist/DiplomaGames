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

    public NPController npcController;
    public AgentAPI agentAPI;
    public Character character;

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
        agentAPI = GetComponent<AgentAPI>();
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
        int lastid_id = character.Id;
        while (true)
        {
            yield return new WaitForSeconds(idleTime);

            while (character.Id == lastid_id)
            {
                Debug.Log("characterid = " + character.Id + " lastid_id = " + lastid_id);
                // Wait until the character's ID is set
                Debug.Log("Waiting for character ID to be set...");
                yield return new WaitForSeconds(20.0f);
            }

            Transform enemy = FindNearestEnemyWithTag("Slime");

            if (enemy != null)
            {
                yield return StartCoroutine(MoveAndAttack(enemy));
            }
            else if (visibleDestinations.Count > 0)
            {
                yield return StartCoroutine(GetDestinationFromAPI());
            }
        }
    }

    private IEnumerator GetDestinationFromAPI()
    {
        // Prepare a hardcoded string of destinations
        string[] destinationNames = { "home", "river", "slime meat", "lake" };
        
        // Convert the list of destination names to JSON
        string jsonData = JsonUtility.ToJson(destinationNames);
        string url = $"http://127.0.0.1:8000/agents/{character.Id}/next_destination";

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
                    Transform destination = visibleDestinations.Find(d => d.name == destinationName);
                    if (destination != null)
                    {
                        MoveToDestination(destination.position);
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
        if (other.CompareTag("Interactable"))
        {
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
