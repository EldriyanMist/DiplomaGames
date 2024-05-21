using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name;
    public int Health;
    public int MaxHealth;
    public int Level;
    public float Experience;
    public float MaxExperience;
    public int Strength;
    public int Agility;
    public string Background_info;



    //lets add some functions to our character class lie die take damage and heal

    public void Die()
    {
        Debug.Log("The character has died");
    }

    // Heal function should work after some time passed after taking damage

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;
    }

}
