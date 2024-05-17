using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the NPC
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the "U" key is pressed
        if (Input.GetKeyDown(KeyCode.U))
        {
            // Play the attack animation
            animator.SetTrigger("Attack");
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
