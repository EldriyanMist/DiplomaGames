using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isOpen = false;

    // Reference to the inventory UI
    public GameObject inventoryUI;

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpen = true;
        // Show the inventory UI
        inventoryUI.SetActive(true);
    }
}
