using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Trigger die animation or any other logic
        SlimeMovement slimeMovement = GetComponent<SlimeMovement>();
        if (slimeMovement != null)
        {
            slimeMovement.Die();
        }
    }
}
