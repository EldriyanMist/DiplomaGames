using UnityEngine;

public class UnifiedTestScript : MonoBehaviour
{
    public InventoryChest chestInventory;
    public InventoryPlayer npcInventory;
    public Item testItem;

    void Start()
    {
        npcInventory = GetComponent<InventoryPlayer>();
        if (npcInventory == null)
        {
            Debug.LogError("InventoryPlayer component not found on this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddTestItemToChest();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            TestNPCInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            TestChestInventory();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItemFromInventory();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PutItemInChest();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeItemFromChest();
        }
    }

    void AddTestItemToChest()
    {
        if (chestInventory != null && testItem != null)
        {
            bool wasAdded = chestInventory.AddItem(testItem);
            Debug.Log(wasAdded ? "Test item added to chest." : "Chest is full, item not added.");
        }
    }

    void TestNPCInventory()
    {
        if (npcInventory != null)
        {
            Item npcItem = npcInventory.GetCurrentItem();
            Debug.Log(npcItem != null ? "NPC Inventory Item: " + npcItem.itemName : "NPC Inventory is empty.");
        }
    }

    void TestChestInventory()
    {
        if (chestInventory != null)
        {
            Item[] chestItems = chestInventory.GetItems();
            string chestContents = "Chest Inventory:\n";
            for (int i = 0; i < chestItems.Length; i++)
            {
                chestContents += chestItems[i] != null ? $"Slot {i}: {chestItems[i].itemName}\n" : $"Slot {i}: empty\n";
            }
            Debug.Log(chestContents);
        }
    }

    void DropItemFromInventory()
    {
        if (npcInventory != null)
        {
            npcInventory.DropItem();
        }
    }

    void PutItemInChest()
    {
        if (npcInventory != null && chestInventory != null)
        {
            bool wasAdded = npcInventory.PutItemInChest(chestInventory);
            Debug.Log(wasAdded ? "Item added to chest from NPC inventory." : "Failed to add item to chest from NPC inventory.");
        }
    }

    void TakeItemFromChest()
    {
        if (npcInventory != null && chestInventory != null)
        {
            bool wasTaken = npcInventory.TakeItemFromChest(chestInventory, 0);
            Debug.Log(wasTaken ? "Item taken from chest and added to NPC inventory." : "Failed to take item from chest.");
        }
    }
}