using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractable : MonoBehaviour
{
    public GameObject interactionPanel; // Reference to the existing interaction panel
    public Button actionButtonPrefab; // Prefab for action buttons
    public List<Action> actions; // List of actions available for this item

    private List<Button> actionButtons = new List<Button>();

    void Start()
    {
        // Ensure the interaction panel is initially inactive
        interactionPanel.SetActive(false);

        // Create buttons for each action
        foreach (Action action in actions)
        {
            Button newButton = Instantiate(actionButtonPrefab, interactionPanel.transform);
            newButton.GetComponentInChildren<Text>().text = action.actionName;
            newButton.onClick.AddListener(() => ExecuteAction(action));
            actionButtons.Add(newButton);
        }
    }

    void Update()
    {
        // Update the position of the interaction panel to be near the item if it's active
        if (interactionPanel.activeSelf)
        {
            UpdatePanelPosition();
        }
    }

    void OnMouseDown()
    {
        // Toggle the visibility of the interaction panel
        interactionPanel.SetActive(!interactionPanel.activeSelf);
    }

    private void ExecuteAction(Action action)
    {
        Debug.Log($"{action.actionName} executed on {gameObject.name}");
        // Add logic for each action type
        switch (action.actionType)
        {
            case ActionType.PickUp:
                Debug.Log("Pick up logic executed.");
                // Pick up logic
                break;
            case ActionType.Examine:
                Debug.Log("Examine logic executed.");
                // Examine logic
                break;
            case ActionType.Use:
                Debug.Log("Use logic executed.");
                // Use logic
                break;
            // Add more cases as needed
        }
        interactionPanel.SetActive(false);
    }

    private void UpdatePanelPosition()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform canvasRectTransform = interactionPanel.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Center the panel vertically on the item's screen position
        screenPoint.y = Mathf.Clamp(screenPoint.y, canvasRectTransform.rect.yMin, canvasRectTransform.rect.yMax) + 50;

        // Position the panel on the right side of the item, adjusting according to canvas size
        screenPoint.x += (interactionPanel.GetComponent<RectTransform>().rect.width / 2) + 30;

        interactionPanel.transform.position = screenPoint;
    }
}
