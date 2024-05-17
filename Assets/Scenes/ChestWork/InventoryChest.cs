using UnityEngine;
using UnityEngine.UI;

public class InventoryChest : MonoBehaviour
{
    public GameObject[] slots; // Array of slots
    private Item[] items; // Array of items

    void Start()
    {
        items = new Item[slots.Length]; // Initialize the items array
    }

    public void AddItem(Item newItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                UpdateSlot(i);
                break;
            }
        }
    }

    void UpdateSlot(int index)
    {
        Image slotImage = slots[index].GetComponent<Image>();
        slotImage.sprite = items[index].icon;
    }
}
