using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int Id;
    public string Name;
    public int Health;
    public int MaxHealth;
    public int Level;
    public float Experience;
    public float MaxExperience;
    public int Strength;
    public int Agility;
    public string Background_info;
    public Vector2 Position;

    public void Die()
    {
        Debug.Log("The character has died");
    }

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
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
