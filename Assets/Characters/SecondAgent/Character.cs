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
    public string Status;

    void Awake()
    {
        Id = Random.Range(1, int.MaxValue); // Generate a random ID
    }

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
        SerializableCharacter serializableCharacter = new SerializableCharacter(this);
        return JsonUtility.ToJson(serializableCharacter);
    }

    private class SerializableCharacter
    {
        public int id;
        public string name;
        public int health;
        public int max_health;
        public int level;
        public float experience;
        public float max_experience;
        public int strength;
        public int agility;
        public string background_info;
        public string status;

        public SerializableCharacter(Character character)
        {
            id = character.Id;
            name = character.Name;
            health = character.Health;
            max_health = character.MaxHealth;
            level = character.Level;
            experience = character.Experience;
            max_experience = character.MaxExperience;
            strength = character.Strength;
            agility = character.Agility;
            background_info = character.Background_info;
            status = character.Status;
        }
    }
}
