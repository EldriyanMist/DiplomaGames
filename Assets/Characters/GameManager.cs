using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject statusWindowPrefab;
    private Dictionary<Character, GameObject> statusWindows = new Dictionary<Character, GameObject>();
    public AgentAPI agentAPI;
    public List<Character> characters; // List of characters in the scene

    void Start()
    {
        if (agentAPI == null)
        {
            Debug.LogError("AgentAPI reference is missing.");
            return;
        }

        foreach (Character character in characters)
        {
            if (character != null)
            {
                // Create a new agent for each character
                StartCoroutine(CreateAndInitializeAgent(character));
            }
            else
            {
                Debug.LogError("Character reference is missing.");
            }
        }
    }

    private IEnumerator CreateAndInitializeAgent(Character character)
    {
        agentAPI.CreateAgent(character);

        // Wait until the character ID is set
        while (character.Id == 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        // Update the agent after creation to ensure it's created first
        yield return new WaitForSeconds(2.0f);
        agentAPI.UpdateAgent(character.Id, character);
    }

    void Update()
    {
        // Any global update logic can go here
    }

    public void ShowStatusWindow(Character character, Transform characterTransform)
    {
        if (!statusWindows.ContainsKey(character))
        {
            GameObject newStatusWindow = Instantiate(statusWindowPrefab, transform);
            StatusWindowManager manager = newStatusWindow.GetComponent<StatusWindowManager>();
            manager.UpdateStatusWindow(character);
            UIFollowCharacter follow = newStatusWindow.GetComponent<UIFollowCharacter>();
            follow.characterTransform = characterTransform;
            statusWindows.Add(character, newStatusWindow);
        }

        statusWindows[character].SetActive(true);
    }
}
