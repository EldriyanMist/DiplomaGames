using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Nav : MonoBehaviour
{
    public enum State { Idle, MovingToNPC, VisitingBuilding, Eating }
    public State currentState = State.Idle;
    public Transform target;
    public Transform targetNPC;
    public Transform building;
    public Transform food;
    private NavMeshAgent agent;
    private float detectionRadius = 5.0f;
    private SphereCollider detectionCollider;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupDetectionCollider();
    }

    void SetupDetectionCollider()
    {
        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && currentState == State.Idle)
        {
            targetNPC = other.transform;
            currentState = State.MovingToNPC;
            MoveToTarget(targetNPC.position);
        }
    }
    

     void Update()
    {
        if (currentState == State.MovingToNPC && targetNPC != null)
        {
            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                StartCoroutine(PerformInteraction());
            }
        }
        // Add more conditions based on states like VisitingBuilding, Eating, etc.
    }

    IEnumerator PerformInteraction()
    {
        // Wait at the NPC location
        yield return new WaitForSeconds(5); // Wait time with the NPC

        // Decide next action: go to a building or eat
        DecideNextAction();
    }

     void DecideNextAction()
    {
        // Example decision-making (expand based on your game's logic)
        currentState = Random.Range(0, 2) == 0 ? State.VisitingBuilding : State.Eating;
        
        if (currentState == State.VisitingBuilding)
        {
            MoveToTarget(building.position);
        }
        else if (currentState == State.Eating)
        {
            MoveToTarget(food.position);
        }
    }

    void MoveToTarget(Vector3 target)
    {
        agent.destination = target;
    }

}
