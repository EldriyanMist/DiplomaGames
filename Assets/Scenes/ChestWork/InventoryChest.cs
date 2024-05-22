using UnityEngine;
using UnityEngine.UI;

public class InventoryChest : MonoBehaviour
{
    public GameObject[] slots; // Array of slots
    private Item[] items; // Array of items

    void Start()
    {
        items = new Item[slots.Length]; // Initialize the items array
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(false); // Ensure the slots are hidden at the start
        }
    }

    public bool AddItem(Item newItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                UpdateSlot(i);
                return true; // Indicate that the item was successfully added
            }
        }
        return false; // Indicate that the item was not added because the inventory is full
    }

    public Item RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < items.Length && items[slotIndex] != null)
        {
            Item removedItem = items[slotIndex];
            items[slotIndex] = null; // Remove the item from the slot
            slots[slotIndex].SetActive(false); // Hide the slot
            return removedItem; // Return the removed item
        }
        return null; // No item to remove
    }

    void UpdateSlot(int index)
    {
        Image slotImage = slots[index].GetComponent<Image>();
        slotImage.sprite = items[index].icon;
        slots[index].SetActive(true);
    }
}
