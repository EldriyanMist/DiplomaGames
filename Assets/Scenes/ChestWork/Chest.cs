using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
    public GameObject chestInventoryPanel; // Reference to the UI panel

    private bool isOpen = false;

    void Start()
    {
        // Start the coroutine to initialize the panel with a delay
        StartCoroutine(InitializeChestInventoryPanel());
    }

    IEnumerator InitializeChestInventoryPanel()
    {
        chestInventoryPanel.SetActive(true);
        yield return new WaitForEndOfFrame(); // Wait for one frame
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
