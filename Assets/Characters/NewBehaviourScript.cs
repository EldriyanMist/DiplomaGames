using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBehaviors : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private List<Transform> visibleDestinations;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found!");
        }
    }

    public void Initialize(List<Transform> visibleDestinations)
    {
        this.visibleDestinations = visibleDestinations;
    }

    public void ExecuteScriptedMovement(string name)
    {
        // Call specific scripted movement functions based on the name
        switch (name)
        {
            case "MoveToFireplace":
                MoveToInteractableByName("Fireplace");
                break;
            case "MoveToTree":
                MoveToInteractableByName("Tree01");
                break;
            case "MoveToStatue":
                MoveToInteractableByName("Statue");
                break;
            default:
                Debug.LogWarning($"Scripted behavior {name} not found.");
                break;
        }
    }

    private void MoveToInteractableByName(string name)
    {
        foreach (Transform dest in visibleDestinations)
        {
            if (dest.name == name)
            {
                agent.SetDestination(dest.position);
                Debug.Log($"Moving to {name} at position {dest.position}");
                return;
            }
        }
        Debug.Log($"No visible destination with the name {name} found.");
    }
}
