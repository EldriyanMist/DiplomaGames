using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Character characterData;

    public ProgressBar progressBar;
    private bool isHealing = false;

    private bool isIdle = false;

void Start()
    {
        characterData = new Character();
        characterData.Name = "NPC";
        characterData.Health = 100;
        characterData.MaxHealth = 100;
        characterData.Level = 1;
        characterData.Experience = 0;
        characterData.MaxExperience = 100;
        characterData.Strength = 10;
        characterData.Agility = 5;
        characterData.Background_info = "This is an NPC character.";
    }


    void OnMouseDown()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.UpdateStatusWindowForCharacter(characterData, transform);
    }

    public void TakeDamage(int amount)
    {
        characterData.Health -= amount;
        Debug.Log(characterData.Name + " took " + amount + " damage. Health: " + characterData.Health);
        if (characterData.Health <= 0)
        {
            characterData.Die();
        }
        else
        {
            StartCoroutine(StartHealingAfterDelay(5f)); // Start healing after 5 seconds
        }
    }

    private IEnumerator StartHealingAfterDelay(float delay)
    {
        isHealing = false;
        yield return new WaitForSeconds(delay);
        Heal(2); // Heal 2 health points after the delay
    }

    public void Heal(int amount)
    {
        if (!isHealing)
        {
            isHealing = true;
            characterData.Health += amount;
            if (characterData.Health > characterData.MaxHealth)
            {
                characterData.Health = characterData.MaxHealth;
            }
        }
    }

    public void StartProgressBar(float duration)
{
    isIdle = true;
    progressBar.StartProgressBar(duration);
    StartCoroutine(WaitForProgressBar(duration));
}

private IEnumerator WaitForProgressBar(float duration)
{
    yield return new WaitForSeconds(duration);
    isIdle = false;
}


}
