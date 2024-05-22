using UnityEngine;

public class CharacterUIController : MonoBehaviour
{
    public GameObject statusWindow;          // Reference to the status window UI panel
    public GameObject chestInventoryPanel;   // Reference to the inventory UI panel

    private int clickState = 0;              // 0: Both closed, 1: Status open, 2: Inventory open

    void Start()
    {
        // Ensure both panels are hidden at the start
        statusWindow.SetActive(false);
        chestInventoryPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        HandleClick();
    }

    void HandleClick()
    {
        switch (clickState)
        {
            case 0: // First click: show status window
                statusWindow.SetActive(true);
                chestInventoryPanel.SetActive(false);
                clickState = 1;
                break;
            case 1: // Second click: show inventory, hide status window
                statusWindow.SetActive(false);
                chestInventoryPanel.SetActive(true);
                clickState = 2;
                break;
            case 2: // Third click: hide both
                statusWindow.SetActive(false);
                chestInventoryPanel.SetActive(false);
                clickState = 0;
                break;
        }
    }
}