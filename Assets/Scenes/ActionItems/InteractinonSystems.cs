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
        //interactionPanel.SetActive(false);
    }


    public void ShowInteractionMenu(GameObject item, List<Action> actions)
{
    currentItem = item;
    interactionPanel.SetActive(true);
    PositionInteractionPanel(item);

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
        Debug.Log("Button created for action: " + action.actionName);
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
    HideInteractionMenu();
}


    private void PositionInteractionPanel(GameObject item)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(item.transform.position);
        Vector3 panelPosition = screenPosition;
        RectTransform panelRectTransform = interactionPanel.GetComponent<RectTransform>();

        // Adjust the position to be to the left of the object
        panelPosition.x += panelRectTransform.rect.width;
        panelPosition.y += screenPosition.y; // Maintain the same y position

        // Set the position of the interaction panel
        interactionPanel.transform.position = panelPosition;
    }
}
