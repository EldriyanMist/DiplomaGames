using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinAI : MonoBehaviour
{
    public Transform target; // The NPC agent to attack
    public float attackDistance = 100.0f; // Distance within which the goblin will attack
    public float chaseDistance = 0; // Distance within which the goblin will chase the NPC
    public float attackRate = 1.0f; // Time between attacks
    private float nextAttackTime = 0.0f;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= chaseDistance)
        {
            ChaseTarget();
        }
        else
        {
            Idle();
        }

        if (distanceToTarget <= attackDistance && Time.time >= nextAttackTime)
        {
            AttackTarget();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        animator.SetBool("isRunning", true);
        Debug.Log("Running");


    }

    void Idle()
    {
        navMeshAgent.ResetPath();
        animator.SetBool("isRunning", false);
    }

    void AttackTarget()
    {
        navMeshAgent.ResetPath();
        transform.LookAt(target);
        animator.SetTrigger("attack");

        // Add damage logic here
        //NPCHealth npcHealth = target.GetComponent<NPCHealth>();
        //if (npcHealth != null)
        //{
        //    npcHealth.TakeDamage(10); // Example damage value
        //}
    }
}
