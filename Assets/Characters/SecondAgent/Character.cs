using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public int Endurance;
    public string Background_info;
    public string Status;

    void Awake()
    {
        Id = Random.Range(1, int.MaxValue); // Generate a random ID
    }

    // References to UI Elements
    public Slider healthSlider;
    public Slider expSlider;
    public TextMeshProUGUI textLvlName;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI backgroundInfoText;

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

    public void UpdateDataFromUI()
    {
        // Update Name and Level from textLvlName
        string fullText = textLvlName.text.Trim();
        int levelIndex = fullText.LastIndexOf(" - LVL.");

        if (levelIndex != -1)
        {
            // Extract name part and level part
            Name = fullText.Substring(0, levelIndex).Trim();
            string levelPart = fullText.Substring(levelIndex + 7).Trim(); // +7 to account for " - LVL."

            if (int.TryParse(levelPart, out int parsedLevel))
            {
                Level = parsedLevel;
            }
            else
            {
                Debug.LogError("Failed to parse level from text: " + levelPart);
            }
        }
        else
        {
            Debug.LogError(" - LVL. not found in text: " + fullText);
        }

        // Update Health
        Health = (int)healthSlider.value;
        MaxHealth = (int)healthSlider.maxValue;

        // Update Experience and MaxExperience
        Experience = expSlider.value;
        MaxExperience = expSlider.maxValue;

        // Update Strength, Agility, Endurance from respective TMP texts
        if (int.TryParse(strengthText.text.Replace("Strength: ", "").Trim(), out int parsedStrength))
        {
            Strength = parsedStrength;
        }
        if (int.TryParse(agilityText.text.Replace("Agility: ", "").Trim(), out int parsedAgility))
        {
            Agility = parsedAgility;
        }
        if (int.TryParse(enduranceText.text.Replace("Endurance: ", "").Trim(), out int parsedEndurance))
        {
            Endurance = parsedEndurance;
        }

        // Update Background_info
        Background_info = backgroundInfoText.text;
    }

    private void Start()
    {
        // Initial update from UI to ensure data is consistent at the start
        UpdateDataFromUI();
    }

    private void Update()
    {
        // Example: Call UpdateDataFromUI when the 'U' key is pressed
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateDataFromUI();
        }
    }

    private void OnEnable()
    {
        // Add listeners to automatically update the data when the UI changes
        healthSlider.onValueChanged.AddListener(delegate { UpdateDataFromUI(); });
        expSlider.onValueChanged.AddListener(delegate { UpdateDataFromUI(); });
        // Add listeners for other UI elements if necessary
    }

    private void OnDisable()
    {
        // Remove listeners to avoid memory leaks
        healthSlider.onValueChanged.RemoveListener(delegate { UpdateDataFromUI(); });
        expSlider.onValueChanged.RemoveListener(delegate { UpdateDataFromUI(); });
        // Remove listeners for other UI elements if necessary
    }
}