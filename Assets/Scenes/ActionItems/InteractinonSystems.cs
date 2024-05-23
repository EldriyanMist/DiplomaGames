using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    public GameObject interactionPanel;
    public Button actionButtonPrefab; // Prefab for action buttons
    private GameObject currentItem;
    private List<Button> currentButtons = new List<Button>();

    void Start()
    {
        interactionPanel.SetActive(false);
    }

    public void ShowInteractionMenu(GameObject item, List<Action> actions)
    {
        currentItem = item;
        interactionPanel.SetActive(true);
        interactionPanel.transform.position = Input.mousePosition;

        // Clear any existing buttons
        foreach (Button button in currentButtons)
        {
            Destroy(button.gameObject);
        }
        currentButtons.Clear();

        // Create buttons for each action
        foreach (Action action in actions)
        {
            Button newButton = Instantiate(actionButtonPrefab, interactionPanel.transform);
            newButton.GetComponentInChildren<Text>().text = action.actionName;
            newButton.onClick.AddListener(() => ExecuteAction(action));
            currentButtons.Add(newButton);
        }
    }

    public void HideInteractionMenu()
    {
        interactionPanel.SetActive(false);
    }

    private void ExecuteAction(Action action)
    {
        Debug.Log($"{action.actionName} executed on {currentItem.name}");
        // Add logic for each action type
        switch (action.actionType)
        {
            case ActionType.PickUp:
                // Pick up logic
                break;
            case ActionType.Examine:
                // Examine logic
                break;
            case ActionType.Use:
                // Use logic
                break;
            // Add more cases as needed
        }
        HideInteractionMenu();
    }
}
