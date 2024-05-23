using UnityEngine;

public class TestGetInventory : MonoBehaviour
{
    public InventoryPlayer npcInventory;
    public InventoryChest chestInventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            TestNPCInventory();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            TestChestInventory();
        }
    }

    void TestNPCInventory()
    {
        Item npcItem = npcInventory.GetCurrentItem();
        if (npcItem != null)
        {
            Debug.Log("NPC Inventory Item: " + npcItem.itemName);
        }
        else
        {
            Debug.Log("NPC Inventory is empty.");
        }
    }

    void TestChestInventory()
    {
        Item[] chestItems = chestInventory.GetItems();
        string chestContents = "Chest Inventory:\n";
        for (int i = 0; i < chestItems.Length; i++)
        {
            if (chestItems[i] != null)
            {
                chestContents += "Slot " + i + ": " + chestItems[i].itemName + "\n";
            }
            else
            {
                chestContents += "Slot " + i + ": empty\n";
            }
        }
        Debug.Log(chestContents);
    }
}
