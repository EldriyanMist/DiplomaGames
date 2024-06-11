using UnityEngine;

public class CharacterUIController : MonoBehaviour
{
    public GameObject statusWindow;          // Reference to the status window UI panel
    public GameObject chestInventoryPanel;   // Reference to the inventory UI panel
    public GameObject healthHungerPanel;     // Reference to the Health and Hunger UI panel

    private int clickState = 0;              // 0: Both closed, 1: Status open, 2: Health and Hunger open, 3: Inventory open

    void Start()
    {
        // Ensure all panels are hidden at the start
        statusWindow.SetActive(false);
        chestInventoryPanel.SetActive(false);
        healthHungerPanel.SetActive(false);
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
                healthHungerPanel.SetActive(false);
                clickState = 1;
                break;
            case 1: // Second click: show health and hunger, hide others
                statusWindow.SetActive(false);
                chestInventoryPanel.SetActive(false);
                healthHungerPanel.SetActive(true);
                clickState = 2;
                break;
            case 2: // Third click: show inventory, hide others
                statusWindow.SetActive(false);
                chestInventoryPanel.SetActive(true);
                healthHungerPanel.SetActive(false);
                clickState = 3;
                break;
            case 3: // Fourth click: hide all
                statusWindow.SetActive(false);
                chestInventoryPanel.SetActive(false);
                healthHungerPanel.SetActive(false);
                clickState = 0;
                break;
        }
    }
}