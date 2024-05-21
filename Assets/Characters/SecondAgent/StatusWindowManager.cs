using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// StatusWindowManager.cs
using TMPro; // Import the TextMesh Pro namespace

public class StatusWindowManager : MonoBehaviour
{
    public TMP_Text characterNameText;
    public TMP_Text healthText;
    public TMP_Text levelText;
    public Slider experienceSlider; // Assuming experience uses a standard UI Slider
    public TMP_Text strengthText;
    public TMP_Text agilityText;
    public TMP_Text Background_info_text;

    // This method updates the status window with character data
    public void UpdateStatusWindow(Character character)
    {
        characterNameText.text = character.Name;
        healthText.text = character.Health.ToString();
        levelText.text =  character.Level.ToString();
        experienceSlider.value = character.Experience / character.MaxExperience;
        strengthText.text = character.Strength.ToString();
        agilityText.text =  character.Agility.ToString();
        Background_info_text.text = character.Background_info;
    }
}
