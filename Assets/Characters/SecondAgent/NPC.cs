using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Character characterData;
    public GameObject statusWindow;
    private StatusWindowManager statusWindowManager;
    private AgentAPI agentAPI;

    public ProgressBar progressBar;
    private bool isHealing = false;
    private bool isIdle = false;

    void Start()
    {
        characterData = new Character
        {
            Name = "NPC",
            Health = 100,
            MaxHealth = 100,
            Level = 1,
            Experience = 0,
            MaxExperience = 100,
            Strength = 10,
            Agility = 5,
            Background_info = "This is an NPC character.",
            Position = transform.position
        };

        statusWindowManager = statusWindow.GetComponent<StatusWindowManager>();
        statusWindow.SetActive(false);

        agentAPI = FindObjectOfType<AgentAPI>();
        if (agentAPI == null)
        {
            Debug.LogError("AgentAPI component not found in the scene.");
            return;
        }

        Debug.Log("Creating agent...");
        agentAPI.CreateAgent(characterData); // Create agent on start
    }

    void OnMouseDown()
    {
        ToggleStatusWindow();
    }

    private void ToggleStatusWindow()
    {
        if (statusWindow.activeSelf)
        {
            statusWindow.SetActive(false);
        }
        else
        {
            statusWindow.SetActive(true);
            statusWindowManager.UpdateStatusWindow(characterData);
            UIFollowCharacter follow = statusWindow.GetComponent<UIFollowCharacter>();
            follow.characterTransform = transform;
        }
    }

    public void TakeDamage(int amount)
    {
        characterData.TakeDamage(amount);
        Debug.Log(characterData.Name + " took " + amount + " damage. Health: " + characterData.Health);
        if (characterData.Health <= 0)
        {
            characterData.Die();
        }
        else
        {
            StartCoroutine(StartHealingAfterDelay(5f));
        }
        agentAPI.UpdateAgent(characterData.Id, characterData); // Update agent on damage
    }

    private IEnumerator StartHealingAfterDelay(float delay)
    {
        isHealing = false;
        yield return new WaitForSeconds(delay);
        Heal(2);
    }

    public void Heal(int amount)
    {
        if (!isHealing)
        {
            isHealing = true;
            characterData.Heal(amount);
            agentAPI.UpdateAgent(characterData.Id, characterData); // Update agent on heal
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
