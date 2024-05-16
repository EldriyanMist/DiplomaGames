using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public StatusWindowManager statusWindowManager;
    public GameObject statusWindow;
    private UIFollowCharacter uiFollowCharacter;

    void Start()
    {
        uiFollowCharacter = statusWindow.GetComponent<UIFollowCharacter>();
        Debug.Log("UIFollowCharacter component found: " + uiFollowCharacter);
    }

    void Update()
    {
        // Toggle the status window when the 'S' key is pressed
        if (Input.GetKeyDown(KeyCode.Z))
        {
            statusWindow.SetActive(!statusWindow.activeSelf);
        }
    }

    // Method to update the status window for a specific character
    public void UpdateStatusWindowForCharacter(Character character, Transform characterTransform)
    {
        statusWindow.SetActive(true);
        statusWindowManager.UpdateStatusWindow(character);
        uiFollowCharacter.characterTransform = characterTransform; // Set the character transform

        // Debug log to check if the character transform is updated
        Debug.Log("Updating status window for: " + character.Name);
    }
}


