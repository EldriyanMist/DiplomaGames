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
            slots[i].AddComponent<Slot>().index = i;
        }
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
        Transform slotTransform = slots[index].transform;
        Image slotImage = slotTransform.GetChild(0).GetComponent<Image>(); // Assuming the first child is the item image
        slotImage.sprite = items[index]?.icon ?? null;
        slotImage.gameObject.SetActive(items[index] != null);
    }

    public void SwapItems(Slot slotA, Slot slotB)
    {
        int indexA = slotA.index;
        int indexB = slotB.index;

        Item temp = items[indexA];
        items[indexA] = items[indexB];
        items[indexB] = temp;

        UpdateSlot(indexA);
        UpdateSlot(indexB);
    }
}
