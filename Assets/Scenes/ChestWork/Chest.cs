using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject chestInventoryPanel; // Reference to the UI panel

    private bool isOpen = false;

    void Start()
    {
        chestInventoryPanel.SetActive(false); // Ensure the panel is hidden at start
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
