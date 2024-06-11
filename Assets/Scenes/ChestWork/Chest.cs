using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestInventoryPanel; // Reference to the UI panel

    private bool isOpen = false;

    void Start()
    {
        // Temporarily activate and deactivate the panel to ensure it initializes properly
        chestInventoryPanel.SetActive(true);
        chestInventoryPanel.SetActive(false);
    }

    void OnMouseDown()
    {
        ToggleChest();
    }

    public void ToggleChest()
    {
        isOpen = !isOpen;
        chestInventoryPanel.SetActive(isOpen);
    }
}